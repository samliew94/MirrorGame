using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public sealed class AssetsLoader
{
    public static string loadJson(string jsonFileName)
    {
        // Example
        // var textFile = Resources.Load<TextAsset>("Text/textFile01");
        var textFile = Resources.Load<TextAsset>(jsonFileName);

        return textFile.text;
    }
}