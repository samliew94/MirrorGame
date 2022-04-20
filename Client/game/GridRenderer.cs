using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GridRenderer : MonoBehaviour
{
    private static GridRenderer instance;
    public static GridRenderer GetInstance() => instance;
    public void Start() => instance = this;

    [SerializeField] private GameObject tilePrefab;

    private int maxX, maxY;

    private List<TileMB> tiles;

    /// <summary>
    /// given a List of Dictionary (json), draw the grid on screen
    /// </summary>
    public void DrawGrid(string gridJson)
    {
        tiles = new List<TileMB>();
        var listMap = JsonUtil.JsonToListMap(gridJson);

        foreach (var map in listMap)
        {
            int x = (int)(long) map["x"];
            int y = (int)(long) map["y"];
            var tileCode = (TileCode) Convert.ToByte(map["tileCode"]);

            TileMB tileMB = Instantiate(tilePrefab, transform).GetComponent<TileMB>();
            tileMB.SetTileCode(tileCode);
            tileMB.SetPosition(x, y);
            tileMB.SetColorAndAlpha(x, y);
            tiles.Add(tileMB);
        }

        maxX = tiles.Max(t => (int) t.transform.position.x);
        maxY = tiles.Max(t => (int) t.transform.position.y);
    }

    /// <summary>
    /// given the dimensions, draw the grid on screen
    /// </summary>
    public void DrawGrid(int width, int height)
    {
        tiles = null;

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                var prefab = Instantiate(tilePrefab, transform);
                var tile = prefab.GetComponent<TileMB>();
                tile.transform.position = new Vector2(x, y);
                tile.SetColorAndAlpha(x, y);
            }
    }

    public bool IsTargetOutOfBounds(Vector2 targetPos)
    {
        var x = targetPos.x;
        var y = targetPos.y;

        if (!(x >= 0 && x <= maxX && y >= 0 && y <= maxY)) // if NOT within boundary
            return true;

        if (tiles.FirstOrDefault(t=>t.transform.position.x == x && t.transform.position.y == y && t.GetTileCode() == TileCode.BLOCKED))
            return true;

        return false;
    }

    public void ShowTiles(params Vector2[] tilesPos)
    {
        foreach (var pos in tilesPos)
        {
            var tile = GetTiles().FirstOrDefault(t => t.transform.position.x == pos.x && t.transform.position.y == pos.y);
            tile?.Show(true);
        }
    }

    public void ShowTiles(bool permaReveal, params Vector2 [] tilesPos)
    {
        foreach (var pos in tilesPos)
        {
            var tile = GetTiles().FirstOrDefault(t => t.transform.position.x == pos.x && t.transform.position.y == pos.y);

            tile?.SetPermaReveal(permaReveal);
            tile?.Show(true);
        }
    }

    public void HideTiles(params Vector2 [] tilesPos)
    {
        foreach (var pos in tilesPos)
        {
            var tile = GetTiles().FirstOrDefault(t => t.transform.position.x == pos.x && t.transform.position.y == pos.y);

            if (tile != null)
                if (!tile.GetPermaReveal())
                    tile.Show(false);
        }
    }

    private List<TileMB> GetTiles() => tiles ??= FindObjectsOfType<TileMB>().ToList();
}
