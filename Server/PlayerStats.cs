using Mirror;
using System;
using System.Collections.Generic;

public class PlayerStats : Dictionary<NetworkConnection, PlayerStat>
{
    public PlayerStats() { }

    public IEnumerable<PlayerStat> Get() => Values;

    public PlayerStat Get(NetworkConnection conn)
    {
        this[conn] ??= new PlayerStat();

        return this[conn];
    }

    public PlayerStat SaveOrUpdateRescue(NetworkConnection conn, int v)
    {
        this[conn].rescueCount = v;

        return this[conn];
    }
}

public class PlayerStat
{
    public int rescueCount; // how many times did this player rescue another player?
    public int landedOnHazardCount; // how many times did this player land on a hazard?
    public int avalanchedCount; // how many times did this player get avalanched?
    public int brancesCollected; // how many branches did this player amass?
    public int campsMade; // how many camps did this player contributed towards?
    public int shoveCount; // how many times did this player shove?
}