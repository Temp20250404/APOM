using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    [SerializeField] private string defaultParameterName = "@Default";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string moveParameterName = "Move";

    public int defaultParameterHash { get; private set; }
    public int idleParameterHash { get; private set; }
    public int moveParameterHash { get; private set; }

    public void Initialize()
    {
        defaultParameterHash = Animator.StringToHash(defaultParameterName);
        idleParameterHash = Animator.StringToHash(idleParameterName);
        moveParameterHash = Animator.StringToHash(moveParameterName);
    }
}
