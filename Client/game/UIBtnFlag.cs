using System;
using System.Linq;
using UnityEngine.UI;

public class UIBtnFlag : IUIBtnGeneric
{
    private static UIBtnFlag instance;
    public static UIBtnFlag GetInstance() => instance;
    private Text txtRemainingFlags;
    public void Start()
    {
        instance = this;

        txtRemainingFlags = GetComponentsInChildren<Text>().FirstOrDefault(x => x.transform.name == "txtRemainingFlags");
    }

    protected override void OnBtnClicked() => PlayerFlag.GetInstance().PlantFlag();
    public void SetRemainingFlags(int v)
    {
        if (txtRemainingFlags == null)
            return;

        txtRemainingFlags.text = v.ToString();
    }
}
