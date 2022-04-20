using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

public class FlagGoal : MonoBehaviour
{
    private bool isReady;
    private void Start() => isReady = true;

    public bool GetIsReady() => isReady;
}


