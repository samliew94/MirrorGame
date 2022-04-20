using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FlagRoot : MonoBehaviour
{
    private static FlagRoot instance;
    public static FlagRoot GetInstance() => instance;

    private FlagArrow flagArrow;
    private FlagGoal flagGoal;

    public void Awake()
    {
        instance = this;

        flagArrow = GameObject.FindObjectOfType<FlagArrow>();
        flagGoal = GameObject.FindObjectOfType<FlagGoal>();
    }

    public async void Setup(int difficulty, bool isGoalBottom)
    {
        while (!flagArrow.GetIsReady() || !flagGoal.GetIsReady())
        {
            await Task.Delay(100);
            continue;
        }

        var goalPosTop = 5;
        var goalPosBottom = -3f;

        flagGoal.transform.position = new Vector2(0, isGoalBottom ? goalPosBottom : goalPosTop);

        var speed = 9 + difficulty; // default

        if (speed > 20)
            speed = 20;
        
        flagArrow.SetSpeed(speed);
    }
}


