using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerHealth : PlayerNetworkBehaviour
{
    private static PlayerHealth instance;
    public static PlayerHealth GetInstance() => instance;

    private readonly object healthLock = new object();
    private int _health;

    private readonly List<CancellationTokenSource> ctses = new List<CancellationTokenSource>();

    public override void OnStartServer()
    {
        base.OnStartServer();

        SetHealth(LobbySettings.GetInstance().Health);

        ChipAwayHealth();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        instance = this;
    }

    [TargetRpc]
    private void TargetHealthChanged(int v)
    {
        if (isLocalPlayer)
            UIHealth.GetInstance().SetValue(v);
    }

    private async void ChipAwayHealth()
    {
        var cts = new CancellationTokenSource();
        ctses.Add(cts);

        try
        {
            while(true)
            {
                var procSpeed = 1000; // default

                if (isOnHazard && isNight)
                    procSpeed = 250;
                else if (isOnHazard || isNight)
                    procSpeed = 500;

                await Task.Delay(procSpeed);

                if (cts.IsCancellationRequested)
                    cts.Token.ThrowIfCancellationRequested();

                lock(healthLock)
                {
                    // what stops reduction of _health?
                    if (_health <= 0 || isAvalanched || isInCamp)
                        continue;

                    AddHealth(-1);
                }
            }
        }
        catch (OperationCanceledException) // handles ThrowOperationCanceledException thrown from ThrowIfCancellationRequested
        {
            ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, "async ChipAwayHealth cancelled");
        }
        finally
        {
            cts.Dispose();
        }
    }

    protected override void OnPlayerFailRescueCallback(object sender, EventPlayerFailRescue e) => AddHealth(-10);

    [ServerCallback]
    public int GetHealth()
    {
        lock (healthLock)
            return _health;
    }

    [ServerCallback]
    private void SetHealth(int v)
    {
        lock (healthLock)
        {
            if (_health == v)
                return; //no changes

            _health = v;
            TargetHealthChanged(v);
            HealthEvents.GetInstance().eventHealthChanged?.Invoke(this, new EventHealthChanged(connectionToClient, _health));
        }
    }

    [ServerCallback]
    public void AddHealth(int v)
    {
        lock (healthLock)
        {
            var h = _health + v;

            if (h <= 0)
                h = 0;
            else if (h >= 100)
                h = 100;

            h = 95;

            SetHealth(h);
        }
    }

    [ServerCallback]
    protected override void OnAddHealthCallback(object sender, EventAddHealth e) => AddHealth(e.AddHealth);

    [ServerCallback]
    protected override void OnEnteredCampCallback(object sender, EventEnteredCamp e)
    {
        if (e.IsAddHealth)
        {
            var cn = e.Camp.GetCampNumber();
            int bonusHealth;

            if (cn == 0)
                bonusHealth = 80;
            else if (cn == 1)
                bonusHealth = 70;
            else if (cn == 2)
                bonusHealth = 60;
            else
                bonusHealth = 50;

            ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Granting {bonusHealth} to Player {e.Player.identity.netId}");
            AddHealth(bonusHealth);
        }
    }

    protected override void OnDestroyServer() => ctses?.ForEach(x => x?.Cancel());

}
