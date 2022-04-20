using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TileCode : byte
{
    WALKABLE,
    BLOCKED,
    CAMP,
    HAZARD,
    GOAL,
}

public class Tile
{
    public readonly int x;
    public readonly int y;
    private TileCode tileCode = TileCode.WALKABLE;
    private MyNetwork network;

    public Tile(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public bool GetIsOccupied()
    {
        if (tileCode == TileCode.HAZARD || tileCode == TileCode.CAMP)
            return true;
        
        return false;
    }

    public void Spawn()
    {
        network ??= MyNetwork.getInstance();
        
        string prefabName = null;

        if (tileCode == TileCode.CAMP)
            prefabName = "Camp";
        else if (tileCode == TileCode.HAZARD)
            prefabName = "Hazard";
        else if (tileCode == TileCode.GOAL)
            prefabName = "Goal";
        else
            return; // invalid maybe

        var prefab = GameObject.Instantiate(network.findSpawnPrefabByName(prefabName));
        prefab.transform.position = new Vector2(x, y);
        NetworkServer.Spawn(prefab);
    }

    public TileCode GetTileCode() => tileCode;
    public void SetTileCode(TileCode symbol) => this.tileCode = symbol;

    public Dictionary<string, object> ToMap()
    {
        return new Dictionary<string, object>
        {
            { "x", x },
            { "y", y },
            { "tileCode", tileCode },
        };
    }

    public IEnumerable<Tile> GetAdjacentTiles()
    {
        var tiles = GridManager.GetInstance().GetTiles();

        return tiles.Where(t => 
            t.x >= x - 1 && t.x <= x + 1 &&
            t.y >= y - 1 && t.y <= y + 1);
    }
}

