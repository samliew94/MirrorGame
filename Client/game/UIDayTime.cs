using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIDayTime : MonoBehaviour
{
    private static UIDayTime instance;
    public static UIDayTime GetInstance() => instance;

    private Image imgDayOverlay;
    private Text txtTime;

    private void Awake()
    {
        instance = this;

        txtTime = GetComponentsInChildren<Text>().FirstOrDefault(x => x.transform.name == "time");
        //imgDayOverlay = GetComponentsInChildren<Image>().FirstOrDefault(x => x.transform.name == "imgDayOverlay");

        SetTime(6);
    }

    public void SetTime(int time)
    {
        var prefix = "am";

        int t;

        if (time > 12 && time < 24)
        {
            t = time - 12;
            prefix = "pm";
        }
        else
            t = time;

        txtTime.text = $"{t} {prefix}";
    }

    public void SetDay(bool isDay)
    {
        return;

        // use animator to overlay
        if (isDay)
        {
            imgDayOverlay.color = Color.clear;
        }
        else
        {
            imgDayOverlay.color = Color.blue;
        }
    }

}