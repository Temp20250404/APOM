using APOM_Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : Skill_Data
{
    private Dictionary<int, Skill_Data> skillDataDictionary;
    private List<Skill_Data> skillDataList;

    public void Init()
    {
        skillDataDictionary = Skill_Data.GetDictionary();
        skillDataList = Skill_Data.GetList();
    }

    public Dictionary<int, Skill_Data> GetDictionary()
    {
        return skillDataDictionary;
    }

    public List<Skill_Data> GetList()
    {
        return skillDataList;
    }
}
