using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSetBool : MonoBehaviour
{
    public string boolName;
    private Animator animator;

    // Use this for initialization
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Sets the boolean to be active or inactive
    /// </summary>
    /// <param name="active"></param>
    public void SetAnimBool(bool active)
    {
        animator.SetBool(boolName, active);
    }

    public void SetBoolActive()
    {
        animator.SetBool(boolName, true);
    }

    public void SetBoolInactive()
    {
        animator.SetBool(boolName, false);
    }
}