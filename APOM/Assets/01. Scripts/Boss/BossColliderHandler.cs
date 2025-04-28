using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossColliderHandler : MonoBehaviour
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
}
