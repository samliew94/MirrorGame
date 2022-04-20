using UnityEngine;

public class PlantFlagExitBtn : IUIBtnGeneric
{
    private float _cd;
    private float cd { get => GetCd(); set => SetCd(value); }

    private readonly float refCd = .5f;
    
    public void Start()
    {
        cd = refCd;
    }

    public void Update()
    {
        if (cd > 0)
            cd -= Time.deltaTime;
    }

    protected override void OnBtnClicked()
    {
        if (cd > 0)
            return;

        cd = .5f;
        PlayerFlag.GetInstance().EndMiniGame(false);
    }

    private float GetCd() => _cd;
    private void SetCd(float v)
    {
        _cd = v;

        SetIsInteractable(v <= 0 ? true : false);
    }
}



