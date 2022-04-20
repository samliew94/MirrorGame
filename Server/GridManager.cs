using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GridManager : ISubscribeEvents
{
    private static readonly Lazy<GridManager> lazy = new Lazy<GridManager>(() => new GridManager());
    public static GridManager GetInstance() => lazy.Value;
    private GridManager() { }

    private List<Tile> tiles;

    private int width;
    private int height;

    public void Subscribe()
    {
        GameStateEvent gameStateEvent = GameStateEvent.GetInstance();
        gameStateEvent.eventGameStateChanged += onGameStateChanged;
    }

    private void onGameStateChanged(object sender, EventGameStateChanged e)
    {
        if (e.GameState == GameState.START)
            SpawnGrid();
    }

    private void SpawnGrid()
    {
        tiles = new List<Tile>();

        // read from lobby settings
        LobbySettings lobbySettings = LobbySettings.GetInstance();
        width = lobbySettings.width;
        height = lobbySettings.height;

        // initialize main grid
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Tile tile = new Tile(x, y);
                tiles.Add(tile);
            }
        }

        new SpawnCampStrategy(width, height, tiles);
        new SpawnInteractablesStrategy(width, height, tiles);
        new SpawnGoalStrategy(width, height, tiles);

        foreach (var t in tiles)
            t.Spawn();

        if (GameObject.FindObjectsOfType<Camp>().Length <= 1)
        {
            var camps = tiles.Where(t => t.GetTileCode() == TileCode.CAMP).ToList();

            string a = "";
        }

        BroadcastGrid();
    }

  
    private void BroadcastGrid()
    {
        string gridJson = GridToJson();

        foreach (var conn in NetworkServer.connections.Values)
        {
            var player = conn.identity.GetComponent<PlayerTileRenderer>();
            player.TargetDrawGrid(gridJson);
        }
    }

    private string GridToJson()
    {
        List<Dictionary<string, object>> map = new List<Dictionary<string, object>>();
        tiles.ForEach(tile => map.Add(tile.ToMap()));
        string gridJson = JsonUtil.toJson(map);
        ServerLog.Log("Grid looks like : ");
        ServerLog.Log(gridJson);
        return gridJson;
    }

    public IEnumerable<Tile> GetAdjacentTiles(Vector3 pos)
    {
        var playerPosX = pos.x;
        var playerPosY = pos.y;

        return tiles.Where(t =>
        {
            return t.x >= playerPosX - 1 && t.x <= playerPosX + 1 && t.y >= playerPosY - 1 && t.y <= playerPosY + 1; // get all within OuB;
        });
    }

    public IEnumerable<Tile> GetTiles() => tiles;


}