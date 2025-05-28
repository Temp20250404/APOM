using APOM_Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    private const int SkillCount = 7;
    public APOM_Data.Skill_Data[] equippedSkills = new APOM_Data.Skill_Data[SkillCount];
    public bool[] canUseSkill = new bool[SkillCount];

    private void Awake()
    {
        for (int i = 0; i < SkillCount; i++)
        {
            canUseSkill[i] = true;
        }
    }

    public void SetSkillDatas(uint _jobIndex)
    {
        switch (_jobIndex)
        {
            case 2: // Archer
                //Managers.Data.skillData.GetDictionary().TryGetValue(5001, out Skill_Data _Data0);
                //equippedSkills[0] = _Data0;
                equippedSkills[0] = Managers.Data.skillData.GetDictionary()[5001];
                equippedSkills[1] = Managers.Data.skillData.GetDictionary()[5002];
                equippedSkills[2] = Managers.Data.skillData.GetDictionary()[5003];
                equippedSkills[3] = Managers.Data.skillData.GetDictionary()[5004];
                equippedSkills[4] = Managers.Data.skillData.GetDictionary()[5005];
                equippedSkills[5] = Managers.Data.skillData.GetDictionary()[5006];
                equippedSkills[6] = Managers.Data.skillData.GetDictionary()[5007];
                break;
        }
    }

    public void StartCooldown(int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= SkillCount) return;

        if (canUseSkill[skillIndex])
        {
            canUseSkill[skillIndex] = false;
            StartCoroutine(SkillCooldownCoroutine(skillIndex, equippedSkills[skillIndex].cooldown));
        }
    }

    private IEnumerator SkillCooldownCoroutine(int index, float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canUseSkill[index] = true;
    }
}
