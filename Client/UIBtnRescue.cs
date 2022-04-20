public class UIBtnRescue : IUIBtnGeneric
{
    private static UIBtnRescue instance;
    public static UIBtnRescue GetInstance() => instance;

    protected override void Awake()
    {
        base.Awake();

        instance = this;
    }

    protected override void OnBtnClicked()
    {
        PlayerRescue.GetInstance().Rescue();
    }
}
