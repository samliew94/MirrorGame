public class UIBtnFlare : IUIBtnGeneric
{
    private static UIBtnFlare instance;
    public static UIBtnFlare GetInstance() => instance;

    protected override void Awake()
    {
        base.Awake();

        instance = this;
    }

    protected override void OnBtnClicked()
    {
        PlayerFlare.GetInstance().ShootFlare();
    }
}

