using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UICollectFlagBtn : IUIBtnGeneric
{
    private static UICollectFlagBtn instance;
    public static UICollectFlagBtn GetInstance() => instance;
    public void Start() => instance = this;

    protected override void OnBtnClicked() => PlayerFlag.GetInstance().RecollectFlag();
}
