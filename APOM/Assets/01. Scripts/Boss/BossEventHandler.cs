using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEventHandler : MonoBehaviour
{
    private BossAI bossAI;

    private bool isSkill1Active = false;
    private void Awake()
    {
        bossAI = GetComponentInParent<BossAI>();
    }
    public void OnHit()
    {
        bossAI.ColliderOnEnable(0.3f);
    }

    public void UseSkill1Area()
    {
        bossAI.ShowSkill1Area();
    }
    
    public void UseSkill1Eff()
    {
        bossAI.OnSkill1Eff(1.5f);
    }
    public void TurnSkill1()
    {
        bossAI.StartRotationCorrection();
    }

    public void EndSkill1()
    {
        if (!isSkill1Active)
        {
            isSkill1Active = true;
            return;
        }

        if (isSkill1Active)
        {
            EndSkillByServer();
        }
    }

    public void UseSkill2Area()
    {
        bossAI.ShowSkill2Area();
    }

    public void UseSkill2Eff()
    {
        bossAI.OnSkill2Eff(2.5f);
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

    public void UseSkill3Eff()
    {
        bossAI.UseSkil3Eff();
    }

    public void EndSkillByServer()
    {
        bossAI.isSkillActive = false;

        // 대기 중이던 상태가 있다면 실행
        if (bossAI.pendingState.HasValue)
        {
            var nextState = bossAI.pendingState.Value;
            bossAI.pendingState = null;

            CS_MONSTER_AI packet = new CS_MONSTER_AI();
            packet.AiID = bossAI.boss.bossID;
            packet.BossState = (uint)nextState;
            packet.CurSpeed = 0f;
            Managers.Network.Send(packet);

            Debug.Log("스킬 종료 후 보류 상태 적용: " + nextState);
        }
    }
}
