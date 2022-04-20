using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public abstract class PlayerNetworkBehaviour : CustomNetworkBehaviour
{
    [SyncVar]
    protected bool isOnHazard;

    [SyncVar]
    protected bool isAvalanched;

    [SyncVar]
    protected bool isInCamp;

    [SyncVar]
    protected int time;

    [SyncVar]
    protected bool isNight;

    [SyncVar]
    protected int health;

    [SyncVar]
    protected bool isPlayingMiniGame;

    protected MiniGameName miniGameName = MiniGameName.NONE; // SERVER

    // LOCAL PLAYER
    private PlayerMove playerMove;

    // LOCAL PLAYER
    protected bool IsMoving
    {
        get
        {
            if (playerMove == null)
                ThrowNullError(this, MethodBase.GetCurrentMethod().Name, nameof(playerMove));

            return playerMove.GetIsMoving();
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        var gameStateEvent = GameStateEvent.GetInstance();
        gameStateEvent.eventGameStateChanged -= OnGameStateChanged;
        gameStateEvent.eventGameStateChanged += OnGameStateChanged;

        var hazardEvents = HazardEvents.GetInstance();
        hazardEvents.eventPlayerOnHazard -= OnPlayerOnHazard;
        hazardEvents.eventPlayerOnHazard += OnPlayerOnHazard;
        hazardEvents.eventPlayerRescued -= OnPlayerRescued;
        hazardEvents.eventPlayerRescued += OnPlayerRescued;
        hazardEvents.eventPlayerFailRescue -= OnPlayerFailRescue;
        hazardEvents.eventPlayerFailRescue += OnPlayerFailRescue;

        var avalancheEvents = AvalancheEvents.GetInstance();
        avalancheEvents.eventAvalanche -= OnAvalanche;
        avalancheEvents.eventAvalanche += OnAvalanche;
        avalancheEvents.eventAvalancheStopped -= OnAvalancheStopped;
        avalancheEvents.eventAvalancheStopped += OnAvalancheStopped;

        var campEvents = CampEvents.GetInstance();
        campEvents.eventEnteredCamp -= OnEnteredCamp;
        campEvents.eventEnteredCamp += OnEnteredCamp;
        campEvents.eventExitedCamp -= OnExitedCamp;
        campEvents.eventExitedCamp += OnExitedCamp;

        var dayTimeEvents = DayTimeEvents.GetInstance();
        dayTimeEvents.eventTimeChanged -= OnTimeChanged;
        dayTimeEvents.eventTimeChanged += OnTimeChanged;
        dayTimeEvents.eventIsNightChanged -= OnDayChanged;
        dayTimeEvents.eventIsNightChanged += OnDayChanged;

        var healthEvents = HealthEvents.GetInstance();
        healthEvents.eventHealthChanged -= OnHealthChanged;
        healthEvents.eventHealthChanged += OnHealthChanged;
        healthEvents.eventAddHealth -= OnAddHealth;
        healthEvents.eventAddHealth += OnAddHealth;

        var flagEvents = FlagEvents.GetInstance();
        flagEvents.eventEnteredFlag -= OnEnteredFlag;
        flagEvents.eventEnteredFlag += OnEnteredFlag;
        flagEvents.eventExitedFlag -= OnExitedFlag;
        flagEvents.eventExitedFlag += OnExitedFlag;

        var miniGameEvents = MiniGameEvents.GetInstance();
        miniGameEvents.eventPlayingMiniGame -= OnPlayingMiniGame;
        miniGameEvents.eventPlayingMiniGame += OnPlayingMiniGame;
    }

    protected virtual void OnGameStateChanged(object sender, EventGameStateChanged e) { }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        playerMove = GetComponent<PlayerMove>();
    }

    #region Hazard
    [ServerCallback]
    private void OnPlayerOnHazard(object sender, EventPlayerOnHazard e)
    {
        if (e.Player == connectionToClient)
        {
            //isLandedOnHazard = true;
            OnPlayerOnHazardCallback(sender, e);
        }
    }

    [ServerCallback]
    protected virtual void OnPlayerOnHazardCallback(object sender, EventPlayerOnHazard e) { }

    [ServerCallback]
    private void OnPlayerRescued(object sender, EventPlayerRescued e)
    {
        if (e.Rescuer == connectionToClient || e.Rescuee == connectionToClient)
        {
            if (e.Rescuee == connectionToClient)
            {
                isOnHazard = false;
                ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"isLandedOnHazard={isOnHazard}");
            }

            OnPlayerRescuedCallback(sender, e);
        }
    }

    [ServerCallback]
    protected virtual void OnPlayerRescuedCallback(object sender, EventPlayerRescued e) { }

    [ServerCallback]
    private void OnPlayerFailRescue(object sender, EventPlayerFailRescue e)
    {
        if (e.Player == connectionToClient)
            OnPlayerFailRescueCallback(sender, e);
    }

    [ServerCallback]
    protected virtual void OnPlayerFailRescueCallback(object sender, EventPlayerFailRescue e) { }

    #endregion

    #region Avalanche
    [ServerCallback]
    private void OnAvalanche(object sender, EventAvalanche e)
    {
        if (e.Conns.Contains(connectionToClient))
        {
            isAvalanched = true;
            OnAvalancheCallback(sender, e);
        }
    }

    [ServerCallback]
    protected virtual void OnAvalancheCallback(object sender, EventAvalanche e) { }

    [ServerCallback]
    private void OnAvalancheStopped(object sender, EventAvalancheStopped e)
    {
        if (e.Conns.Contains(connectionToClient))
        {
            isAvalanched = false;
            OnAvalancheStoppedCallback(sender, e);
        }
    }

    [ServerCallback]
    protected virtual void OnAvalancheStoppedCallback(object sender, EventAvalancheStopped e) { }
    #endregion

    #region Camp
    [ServerCallback]
    private void OnEnteredCamp(object sender, EventEnteredCamp e)
    {
        if (e.Player == connectionToClient)
        {
            isInCamp = true;
            OnEnteredCampCallback(sender, e);
        }
    }

    [ServerCallback]
    protected virtual void OnEnteredCampCallback(object sender, EventEnteredCamp e) { }

    [ServerCallback]
    private void OnExitedCamp(object sender, EventExitedCamp e)
    {
        if (e.Player == connectionToClient)
        {
            isInCamp = false;
            OnExitedCampCallback(sender, e);
        }
    }

    [ServerCallback]
    protected virtual void OnExitedCampCallback(object sender, EventExitedCamp e) { }

    #endregion

    #region Day Time
    [ServerCallback]
    protected virtual void OnTimeChanged(object sender, EventTimeChanged e)
    {
        time = e.Time;
    }

    [ServerCallback]
    protected virtual void OnDayChanged(object sender, EventIsNightChanged e)
    {
        isNight = e.IsNight;
    }

    #endregion

    #region Health
    private void OnHealthChanged(object sender, EventHealthChanged e)
    {
        if (e.Player == connectionToClient)
        {
            health = e.Health;
            OnHealthChangedCallback(sender, e);
        }
    }

    protected virtual void OnHealthChangedCallback(object sender, EventHealthChanged e) { }

    private void OnAddHealth(object sender, EventAddHealth e)
    {
        if (e.Player == connectionToClient)
            OnAddHealthCallback(sender, e);
    }

    protected virtual void OnAddHealthCallback(object sender, EventAddHealth e) { }

    #endregion

    #region Flag
    private void OnEnteredFlag(object sender, EventEnteredFlag e)
    {
        if (e.Player == connectionToClient)
            OnEnteredFlagCallback(sender, e);
    }
    protected virtual void OnEnteredFlagCallback(object sender, EventEnteredFlag e) { }
    private void OnExitedFlag(object sender, EventExitedFlag e)
    {
        if (e.Player == connectionToClient)
            OnExitedFlagCallback(sender, e);
    }
    protected virtual void OnExitedFlagCallback(object sender, EventExitedFlag e) { }

    #endregion

    #region MiniGame
    private void OnPlayingMiniGame(object sender, EventPlayingMiniGame e)
    {
        if (e.PlayerConn == connectionToClient)
        {
            isPlayingMiniGame = e.IsPlayingMiniGame;
            miniGameName = e.MiniGameGame;
            OnPlayingMiniGameCallback(sender, e);
        }
    }

    protected virtual void OnPlayingMiniGameCallback(object sender, EventPlayingMiniGame e) { }

    #endregion

    #region Logging

    [Command]
    public void CmdLog(string msg) => ServerLog.Log($"CmdLog {msg}");

    #endregion

    #region OnDestroys
    private void OnDestroy()
    {
        if (isServer)
            OnDestroyServer();
        if (isLocalPlayer)
            OnDestroyLocalPlayer();
    }

    protected virtual void OnDestroyServer() { }
    protected virtual void OnDestroyLocalPlayer() { }
    #endregion
}