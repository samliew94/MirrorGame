using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWaitingPlayersPlayerItem : MonoBehaviour
{
    public Image imgStateNotReady;
    public Image imgStateReady;
    public Text txtPlayerName;

    public void setState(int state)
    {
        imgStateNotReady.gameObject.SetActive(false);
        imgStateReady.gameObject.SetActive(false);

        if (state == 0)
            imgStateNotReady.gameObject.SetActive(true);
        else
            imgStateReady.gameObject.SetActive(true);
    }

    public void setDisplayName(string v) => txtPlayerName.text = v;
}
