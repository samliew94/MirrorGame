using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UICampData : MonoBehaviour
{
    private static UICampData instance;
    public static UICampData GetInstance() => instance;

    private Transform canvas;

    public void Awake()
    {
        instance = this;

        canvas = GetComponentsInChildren<Transform>().FirstOrDefault(x => x.transform.name == "canvas");
    }

    public void ShowCampTools(string json)
    {
        // do things with the json, then set to active
        canvas.gameObject.SetActive(true);
    }

    public void Hide()
    {
        canvas.gameObject.SetActive(false);
    }

}
