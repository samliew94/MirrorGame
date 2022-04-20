using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class UITransition : UIBase
{
    private static UITransition instance;
    public static UITransition GetInstance() => instance;

    private Animator animator;

    public void Start()
    {
        instance = this;

        animator = GetComponent<Animator>();

        DontDestroyOnLoad(this);

    }

    public bool GetPlayLoadingAnimation() => animator.GetBool("isLoading");
    public void PlayLoadingAnimation(bool v) => animator.SetBool("isLoading", v);
    public bool GetPlayFadeAnimation() => animator.GetBool("fade");
    public void PlayBlankBlackScreenAnimation(bool v) => animator.SetBool("blankBlack", v);
    public bool GetLoseAnimation() => animator.GetBool("lose");
    public void PlayLoseAnimation(bool v) => animator.SetBool("lose", v);
    public void StopAllBoolAnimations()
    {
        ClientLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Stopping all transitioning animations...");
        foreach (var parameter in animator.parameters)
            if (parameter.type == AnimatorControllerParameterType.Bool)
                if (animator.GetBool(parameter.name))
                    animator.SetBool(parameter.name, false);
    }
}
