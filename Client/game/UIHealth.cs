using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    private Text txtLabel;
    private Text txtValue;

    private static UIHealth instance;
    public static UIHealth GetInstance() => instance;
    public void Start()
    {
        instance = this;

        txtLabel = GetComponentsInChildren<Text>().FirstOrDefault(x => x.transform.name == "label");
        txtValue = GetComponentsInChildren<Text>().FirstOrDefault(x => x.transform.name == "value");

        if (txtLabel == null)
            PlayerHealth.GetInstance().ThrowNullError(this, MethodBase.GetCurrentMethod().Name, nameof(txtLabel));

        if (txtValue == null)
            PlayerHealth.GetInstance().ThrowNullError(this, MethodBase.GetCurrentMethod().Name, nameof(txtValue));
    }

    public void SetLabel(object value) => txtLabel.text = value.ToString();
    public void SetValue(object value) => txtValue.text = value.ToString();

}