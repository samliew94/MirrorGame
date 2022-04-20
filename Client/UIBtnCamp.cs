public class UIBtnCamp : IUIBtnGeneric
{
    private static UIBtnCamp instance;
    public static UIBtnCamp GetInstance() => instance;

    protected override void Awake()
    {
        base.Awake();

        instance = this;
    }

    protected override void OnBtnClicked()
    {
    }
}

