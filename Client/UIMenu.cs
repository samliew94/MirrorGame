using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    private GameObject panel;
    private Button btnExit;
    private Button btnRestart;

    private float timeElapsed;

    private void Awake()
    {
        panel = transform.Find("panel").gameObject;
        btnExit = panel.transform.Find("btnExit").GetComponent<Button>();
        btnRestart = panel.transform.Find("btnRestart").GetComponent<Button>();

        btnExit.onClick.AddListener(OnExit);
        btnRestart.onClick.AddListener(OnRestart);
    }

    private void OnExit()
    {
        
    }

    private void OnRestart()
    {
        // if (PlayerLobbyNB.GetInstance() != null)
        // {
        //     //localPlayer (could be host)
        //     PlayerLobbyNB.GetInstance().Restart();
        // }
        // else
        //     MyNetwork.GetInstance().StopHost();

    }


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            panel.SetActive(!panel.activeSelf);

    }
}
