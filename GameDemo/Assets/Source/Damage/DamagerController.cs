using Goap;

public class DamagerController {

	
    public static void Damage(GoapAgent attacker, GoapAgent target, int skillID)
    {
        if (!target.IsAlive())
        {
            return;
        }

        SkillData skillData = TableTool.GetTableDataRow<SkillData>(TableType.Skill, skillID);
        if (skillData == null)
        {
            return;
        }

        target.Damage(skillData.damage);
    }
}
