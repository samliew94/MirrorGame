using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class DayTimeManager : ISubscribeEvents, IAsync
{
    private static readonly Lazy<DayTimeManager> lazy = new Lazy<DayTimeManager>(() => new DayTimeManager());
    public static DayTimeManager GetInstance() => lazy.Value;
    private DayTimeManager() { }

    private readonly object timeLock = new object();
    private int time; // 6 - 17 (day), 18 -> 23 -> 0 -> 5 is night

    private List<CancellationTokenSource> ctses;

    public void Subscribe()
    {
        GameStateEvent gameStateEvent = GameStateEvent.GetInstance();
        gameStateEvent.eventGameStateChanged -= onGameStateChanged;
        gameStateEvent.eventGameStateChanged += onGameStateChanged;
    }

    private void onGameStateChanged(object sender, EventGameStateChanged e)
    {
        if (e.GameState == GameState.START)
        {
            ctses = new List<CancellationTokenSource>();
            time = 6;
            StartDayTimeCycle();
        }
        else if (e.GameState == GameState.LOSE)
            KillAsyncFunctions();
    }

    private async void StartDayTimeCycle()
    {
        var cts = new CancellationTokenSource();
        ctses.Add(cts);

        try
        {
            int counter = 0;

            while (true)
            {
                await Task.Delay(1000); // update day time every 1 second

                if (cts.IsCancellationRequested)
                    cts.Token.ThrowIfCancellationRequested();

                if (counter == 3)
                {
                    counter = 0;
                    AddTime(1);
                }

                counter++;
            }
        }
        catch (Exception)
        {

        }
        finally
        {
            cts.Dispose();
        }
    }

    private void SetTime(int v)
    {
        lock (timeLock)
            time = v;

        DayTimeEvents.GetInstance().eventTimeChanged?.Invoke(this, new EventTimeChanged(time));

        var isNight = true;

        if (time > 5 && time < 18)
            isNight = false;

        DayTimeEvents.GetInstance().eventIsNightChanged?.Invoke(this, new EventIsNightChanged(isNight));
    }

    private void AddTime(int v)
    {
        lock (timeLock)
        {
            var t = time;

            t += v;

            if (t >= 24)
                t = 0;

            SetTime(t);
        }
    }

    public void KillAsyncFunctions() => ctses?.ForEach(x => x?.Cancel());
}
