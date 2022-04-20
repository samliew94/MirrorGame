using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.TextMeshProUGUI;

public class UIDebug : MonoBehaviour
{
    private static UIDebug instance;
    private GameObject content;
    private GameObject panel;
    [SerializeField] private TMPro.TextMeshProUGUI txtDebug;

    private float timeElapsed;

    public static UIDebug GetInstance() => instance;

    public void Awake()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);

        panel = transform.Find("Panel").gameObject;
        content = panel.transform.Find("content").gameObject;
    }

    private void Update()
    {
        if (timeElapsed <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                panel.SetActive(!panel.activeSelf);
                timeElapsed = .1f;
            }
        }
        else
        {
            timeElapsed -= Time.deltaTime;
        }

        
    }

    // Server
    public void SendServerMsg(string msg)
    {
        var newTxtDebug = Instantiate(txtDebug, content.transform);
        newTxtDebug.color = Color.red;
        newTxtDebug.text = msg;
    }

    // Client
    public void SendClientMsg(string msg)
    {
        var newTxtDebug = Instantiate(txtDebug, content.transform);
        newTxtDebug.color = Color.green;
        newTxtDebug.text = msg;
    }
}
