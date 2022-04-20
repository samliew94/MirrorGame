using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyButtonChangeColor : MonoBehaviour
{
    [SerializeField] private Button btnChangeColor;
    [SerializeField] private UILobbyColorsWindow uiLobbyColorsWindow;


    private void Awake()
    {
        btnChangeColor.onClick.AddListener(() => uiLobbyColorsWindow.toggle());
    }

}
