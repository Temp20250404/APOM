using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEventHandler : MonoBehaviour
{
    private BossAI bossAI;

    private void Awake()
    {
        bossAI = GetComponentInParent<BossAI>();
    }
    public void OnHit()
    {
        bossAI.ColliderOnEnable(0.3f);
    }

    public void UseSkill1()
    {
        Vector3 up = new Vector3(0, 2, 0);
        bossAI.ShowSkill1Area(transform.position + up, 2f, 1f); // 스킬 범위 표시
    }
    public void Skill2Eff()
    {
        bossAI.OnSkill2Eff(3.0f);
    }

    public void UseSkill3()
    {
        bossAI.UseSkill3(this.transform);
    }

    public void EndSkill3()
    {
        bossAI.EndSkill3(this.transform);
    }

    public void EndSkill3Anim()
    {
        bossAI.EndSkillAnim();
    }

    public void EndSkillByServer()
    {
        bossAI.isSkillActive = false;
    }
}
