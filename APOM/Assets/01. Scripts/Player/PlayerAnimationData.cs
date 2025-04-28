using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    [SerializeField] private string groundParameterName = "@@Ground";

    [SerializeField] private string defaultParameterName = "@Default";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string moveParameterName = "Move";

    [SerializeField] private string attackParameterName = "@Attack";
    [SerializeField] private string normalAttackParameterName = "NormalAttack";
    [SerializeField] private string dodgeParameterName = "Dodge";
    [SerializeField] private string skill1ParameterName = "Skill1";
    [SerializeField] private string skill2ParameterName = "Skill2";
    [SerializeField] private string skill3ParameterName = "Skill3";
    [SerializeField] private string skill4ParameterName = "Skill4";
    [SerializeField] private string skill5ParameterName = "Skill5";

    public int groundParameterHash { get; private set; }

    public int defaultParameterHash { get; private set; }
    public int idleParameterHash { get; private set; }
    public int moveParameterHash { get; private set; }

    public int attackParameterHash { get; private set; }
    public int normalAttackParameterHash { get; private set; }
    public int dodgeParameterHash { get; private set; }
    public int skill1ParameterHash { get; private set; }
    public int skill2ParameterHash { get; private set; }
    public int skill3ParameterHash { get; private set; }
    public int skill4ParameterHash { get; private set; }
    public int skill5ParameterHash { get; private set; }

    public void Initialize()
    {
        groundParameterHash = Animator.StringToHash(groundParameterName);

        defaultParameterHash = Animator.StringToHash(defaultParameterName);
        idleParameterHash = Animator.StringToHash(idleParameterName);
        moveParameterHash = Animator.StringToHash(moveParameterName);

        attackParameterHash = Animator.StringToHash(attackParameterName);
        normalAttackParameterHash = Animator.StringToHash(normalAttackParameterName);
        dodgeParameterHash = Animator.StringToHash(dodgeParameterName);
        skill1ParameterHash = Animator.StringToHash(skill1ParameterName);
        skill2ParameterHash = Animator.StringToHash(skill2ParameterName);
        skill3ParameterHash = Animator.StringToHash(skill3ParameterName);
        skill4ParameterHash = Animator.StringToHash(skill4ParameterName);
        skill5ParameterHash = Animator.StringToHash(skill5ParameterName);
    }
}
