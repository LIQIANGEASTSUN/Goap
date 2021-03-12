using System.Collections.Generic;
using Goap;
using UnityEngine;

public class SkillManager {
    private GoapAgent goapAgent;

    private List<Skill> skillList = new List<Skill>();

	public SkillManager(GoapAgent goapAgent)
    {
        this.goapAgent = goapAgent;
        Init();
    }

    private void Init()
    {
        NpcData npcData = goapAgent.NpcData;
        //int[] skills = new int[]{ npcData.skillID, npcData.skillID1, npcData.skillID2};

        int[] skills = new int[] { npcData.skillID, npcData.skillID1, npcData.skillID2, };

        skillList.Clear();
        for (int i = 0; i < skills.Length; ++i)
        {
            int skillID = skills[i];
            SkillData skillData = TableTool.GetTableDataRow<SkillData>(TableType.Skill, skillID);
            if (skillData == null)
            {
                continue;
            }

            Skill skill = new Skill(skillData, i);
            skillList.Add(skill);
        }
    }

    public void OnFrame()
    {
        for (int i = 0; i < skillList.Count; ++i)
        {
            skillList[i].OnFrame();
        }
    }

    public Skill GetUseableSkill()
    {
        List<Skill> useList = new List<Skill>();
        for (int i = 0; i < skillList.Count; ++i)
        {
            Skill skill = skillList[i];
            if (!skill.IsUseable())
            {
                continue;
            }

            useList.Add(skill);
        }

        Skill useSkill = null;
        if (useList.Count <= 0)
        {
            return useSkill;
        }
        else
        {
            //int value = Random.Range(0, useList.Count);
            return useList[0];
        }
    }
}