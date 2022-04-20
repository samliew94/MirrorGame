using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : UIBase
{
    // singleton
    private static UILobby instance;
    public static UILobby GetInstance() => instance;

    private UITransition _uiTransition;
    private UITransition uiTransition { get => _uiTransition ??= FindObjectOfType<UITransition>(); }

    private UILobbyColorsWindow _panelColorsWin;
    private UILobbyColorsWindow panelColorsWin { get => _panelColorsWin ??= FindObjectOfType<UILobbyColorsWindow>(); }

    private UILobbyPlayers _uILobbyPlayers;
    private UILobbyPlayers uiLobbyPlayers => _uILobbyPlayers ??= FindObjectOfType<UILobbyPlayers>();

    [SerializeField] private Button btnChangeColorId;
    [SerializeField] private Button btnCloseColorWindow;

    public void Awake()
    {
        instance = this;

        btnChangeColorId.onClick.AddListener(() => showPanelColorsWin(true));
        btnCloseColorWindow.onClick.AddListener(() => showPanelColorsWin(false));
    }

    private void showPanelColorsWin(bool v) => panelColorsWin.gameObject.SetActive(v);
    public void onOpenColorWindow() => panelColorsWin.gameObject.SetActive(true);
    public void onCloseColorWindow() => panelColorsWin.gameObject.SetActive(false);
}
