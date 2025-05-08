using Game;
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
        bossAI.ShowSkill1Area(bossAI.transform.position + Vector3.up * 2f, 2f, 1f);
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

        // 대기 중이던 상태가 있다면 실행
        if (bossAI.pendingState.HasValue)
        {
            var nextState = bossAI.pendingState.Value;
            bossAI.pendingState = null;

            CS_BOSS_PHASE packet = new CS_BOSS_PHASE();
            packet.BossID = bossAI.boss.bossID;
            packet.BossState = (uint)nextState;
            packet.CurSpeed = 0f;
            Managers.Network.Send(packet);

            Debug.Log("스킬 종료 후 보류 상태 적용: " + nextState);
        }
    }
}
