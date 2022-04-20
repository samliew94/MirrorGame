using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Camp : Interactable
{
    private List<NetworkConnection> playersGivenHealth; // players can only gain max health once.
    private int campNumber = -1; // undefined
    public override void OnStartServer()
    {
        playersGivenHealth = new List<NetworkConnection>();

        FindCampNumber();
    }

    private void FindCampNumber()
    {
        var tiles = GridManager.GetInstance().GetTiles();

        var camps = tiles.Where(t => t.GetTileCode() == TileCode.CAMP).OrderBy(o => o.x).ToList();

        campNumber = camps.FindIndex(t => t.x == transform.position.x);

        ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"{transform.name} campNumber = {campNumber} at {transform.position.x}");

    }

    protected override void onCustomTriggerEnterCallback(NetworkConnection conn, int triggerRingId)
    {        
        ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Trigger enter {transform.name} ringId={triggerRingId}");

        if (!playersGivenHealth.Contains(conn))
        {
            playersGivenHealth.Add(conn);
            CampEvents.GetInstance().eventEnteredCamp?.Invoke(this, new EventEnteredCamp(conn, this, true)); // health only given first time
        }
        else
            CampEvents.GetInstance().eventEnteredCamp?.Invoke(this, new EventEnteredCamp(conn, this, false));
    }

    protected override void onCustomTriggerExitCallback(NetworkConnection conn, int triggerRingId)
    {
        ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Trigger exit {transform.name} ringId={triggerRingId}");

        CampEvents.GetInstance().eventExitedCamp?.Invoke(this, new EventExitedCamp(conn, this));
    }

    public int GetCampNumber() => campNumber;

}
