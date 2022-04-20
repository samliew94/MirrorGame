using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILobbyColorsWindow : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    public void show() => panel.SetActive(true);
    public void hide() => panel.SetActive(false);

    public void toggle()
    {
        if (panel.activeSelf)
            hide();
        else
            show();
    }
}
