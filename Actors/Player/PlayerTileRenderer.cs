using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerTileRenderer : NetworkBehaviour
{
    private static PlayerTileRenderer instance;
    public static PlayerTileRenderer GetInstance() => instance;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        instance = this;
    }
    /// <summary>
    /// Instructs client to draw the initial grid on map.
    /// </summary>
    [TargetRpc]
    public void TargetDrawGrid(string gridJson) => GridRenderer.GetInstance().DrawGrid(gridJson);
    //public void TargetDrawGrid(int width, int height) => gridRenderer.DrawGrid(width, height);
    
}
