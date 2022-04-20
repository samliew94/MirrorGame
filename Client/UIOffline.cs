using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOffline : UIBase
{
    // singleton
    private static UIOffline instance;
    public static UIOffline getInstance() => instance;

    // dependencies
    private MyNetwork myNetwork;

    private bool btnPressed;

    public void Start()
    {
        instance = this;

        myNetwork = FindObjectOfType<MyNetwork>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (btnPressed)
            return;

        if (Input.GetKeyDown(KeyCode.H))
        {
            StartHost();
            btnPressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            StartClient();
            btnPressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            StartServer();
            btnPressed = true;
        }
    }

    

    public void StartHost() => myNetwork.StartHost();

    public void StartClient() => myNetwork.StartClient();

    public void StartServer() => myNetwork.StartServer();
}
