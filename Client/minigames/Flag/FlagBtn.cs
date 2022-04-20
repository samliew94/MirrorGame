using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FlagBtn : IUIBtnGeneric
{
    private FlagArrow flagArrow;
    private float _cd;
    private float cd { get => GetCd(); set => SetCd(value); }

    private readonly float refCd = .5f;
    
    public void Start()
    {
        flagArrow = FindObjectOfType<FlagArrow>();
        cd = refCd;
    }

    public void Update()
    {
        if (cd > 0)
            cd -= Time.deltaTime;
    }

    protected override void OnBtnClicked()
    {
        if (cd > 0 || !flagArrow.GetIsHitGoal())
            return;

        cd = .5f;
        PlayerFlag.GetInstance().EndMiniGame(true);
        ClientLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Win plant flag");
    }

    private float GetCd() => _cd;
    private void SetCd(float v)
    {
        _cd = v;

        SetIsInteractable(v <= 0 ? true : false);
    }
}



