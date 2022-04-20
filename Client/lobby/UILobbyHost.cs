using System;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyHost : MonoBehaviour
{
    private static UILobbyHost instance;
    public static UILobbyHost GetInstance() => instance;
    public void Start() => instance = this;

    [SerializeField] private Button btnStartGame;

    private void Awake() => btnStartGame.onClick.AddListener(() => StartGame());

    private void Update()
    {
        if (Input.GetKey(KeyCode.S))
            StartGame();
    }

    private void StartGame()
    {
        btnStartGame.interactable = false;
        PlayerLobbyHost.GetInstance()?.CmdStartGame();
    }

    public void HostChanged(bool isHost)
    {
        btnStartGame.gameObject.SetActive(isHost);
        btnStartGame.interactable = isHost ? true : false;
    }
}