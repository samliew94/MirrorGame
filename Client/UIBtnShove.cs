using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIBtnShove : IUIBtnGeneric
{
    private static UIBtnShove instance;
    public static UIBtnShove GetInstance() => instance;

    protected override void Awake()
    {
        base.Awake();

        instance = this;
    }

    protected override void OnBtnClicked()
    {
        PlayerShove.GetInstance().Shove();
    }
}
