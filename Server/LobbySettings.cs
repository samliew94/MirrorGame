using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LobbySettings
{
    private static readonly Lazy<LobbySettings> lazy = new Lazy<LobbySettings>(() => new LobbySettings());
    public static LobbySettings GetInstance() => lazy.Value;
    private LobbySettings() { }

    public int width = 20;
    public int height = 20;
    public float shoveCd = .2f;
    public float flareCd = .2f;
    public float FlagCd = .2f;
    public int remainingFlares = 3;
    public int RemainingFlags = 99;
    public int Health = 100;
    public float campCd = .2f;
    public float rescueCd = .2f;
    public float braceCd = .2f;
}
