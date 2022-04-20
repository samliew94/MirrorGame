using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    private static UIPlayer instance;
    public static UIPlayer GetInstance() => instance;
    private readonly List<Button> buttons = new List<Button>();

    private Button btnPlantFlag;
    private Button btnCollectFlag;
    public void Start() => instance = this;

    public void Awake()
    {
        buttons.AddRange(GetComponentsInChildren<Button>());

        btnPlantFlag = buttons.Find(b => b.transform.name == "btnFlag");
        btnCollectFlag = buttons.Find(b => b.transform.name == "btnCollectFlag");
        ShowBtnCollectFlag(false);
    }

    public void ShowBtnPlantFlag(bool isShow) => btnPlantFlag.gameObject.SetActive(isShow);
    public void ShowBtnCollectFlag(bool isShow)
    {
        btnCollectFlag.interactable = isShow;
        btnCollectFlag.gameObject.SetActive(isShow);
    }

    public void ShowButtons(bool isShow) => buttons?.ForEach(b=>b?.gameObject?.SetActive(isShow));
}
