using APOM_Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JobBaseStatsData : JobBaseStats_Data
{
    private Dictionary<int, JobBaseStats_Data> jobBaseStatsDataDictionary;
    private List<JobBaseStats_Data> jobBaseStatsDataList;

    public void Init()
    {
        jobBaseStatsDataDictionary = JobBaseStats_Data.GetDictionary();
        jobBaseStatsDataList = JobBaseStats_Data.GetList();
    }

    public Dictionary<int, JobBaseStats_Data> GetDictionary()
    {
        return jobBaseStatsDataDictionary;
    }

    public List<JobBaseStats_Data> GetList()
    {
        return jobBaseStatsDataList;
    }
}
