public class UIBtnBrace : IUIBtnGeneric
{
    private static UIBtnBrace instance;
    public static UIBtnBrace GetInstance() => instance;

    protected override void Awake()
    {
        base.Awake();

        instance = this;
    }

    protected override void OnBtnClicked()
    {
        PlayerBrace.GetInstance().Brace();
    }
}
