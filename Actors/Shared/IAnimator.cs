using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class IAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        if (!animator)
            throw new Exception($"animator instance field is not set! Set them in inspector!");
    }

    public void Show(bool isShow)
    {
        if (!animator.enabled)
            animator.enabled = true;

        animator.SetBool("isShow", isShow);
    }

    public void Death(bool isDeath)
    {
        if (!animator.enabled)
            animator.enabled = true;

        animator.SetBool("isDeath", isDeath);
    }
}
