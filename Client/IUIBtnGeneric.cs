using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public abstract class IUIBtnGeneric : MonoBehaviour
{
    private bool isHidden;

    protected Button btn;
    private Image imgCd;
    protected virtual void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => OnBtnClicked());

        imgCd = GetComponent<Image>();
    }
    public virtual void Show()
    {
        isHidden = false;
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        isHidden = true;
        gameObject.SetActive(false);
    }

    public bool GetIsHidden() => isHidden;
    public bool GetIsInteractable() => btn.interactable;
    public void SetIsInteractable(bool value)
    {
        btn.interactable = value;
    }

    protected abstract void OnBtnClicked();

    public void SetCd(float current, float total)
    {
        if (!imgCd)
            return;

        imgCd.fillAmount = 1 - (current / total);
    }
}