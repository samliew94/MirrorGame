using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyButtonColors : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private Button[] buttons;
    private readonly Dictionary<int, Button> buttonMap = new Dictionary<int, Button>();

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();

        int colorId = 0;
        foreach (var button in buttons)
        {
            buttonMap.Add(colorId, button);
            button.onClick.AddListener(() => PlayerLobbyColor.getInstance()?.CmdChangeColor(colorId));
            colorId++;
        }
    }

    public void onChangedColor(List<int> takenColors)
    {
        foreach (var kvp in buttonMap)
        {
            kvp.Value.interactable = true;

            if(takenColors.Contains(kvp.Key))
                kvp.Value.interactable = false;
        }
    }
}
