using UnityEngine;
using UnityEngine.UI;

public class UILobbyPlayer : MonoBehaviour
{
    [SerializeField] private Image imgColorId;
    [SerializeField] private Text txtDisplayName;

    private int _colorId;
    public int colorId
    {
        get => _colorId;
        set
        {
            _colorId = value;

            imgColorId.color = Color.white; // lookup from ColorUtil
        }
    }

    private string _displayName;
    public string displayName
    {
        get => _displayName;
        set
        {
            _displayName = value;

            txtDisplayName.text = value; 
        }
    }
       
}