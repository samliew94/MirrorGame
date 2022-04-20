using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    public virtual void show() => gameObject.SetActive(true);

    public virtual void hide() => gameObject.SetActive(false);

}