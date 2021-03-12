using UnityEngine;

public class Skill  {
    private SkillData skillData;
    private float coolDown = 0;
    private int index = 0;

    public Skill(SkillData skillData, int index)
    {
        this.skillData = skillData;
        this.index = index;
    }

    public void Reset()
    {
        coolDown = skillData.coolDown;
    }

    public bool IsUseable()
    {
        return coolDown <= 0;
    }

    public void OnFrame()
    {
        coolDown -= Time.deltaTime;
    }

    public float Range()
    {
        return skillData != null ? skillData.attackRange : 0;
    }

    public SkillData SkillData { get { return skillData; } }

    public int Index { get { return index; } }
}