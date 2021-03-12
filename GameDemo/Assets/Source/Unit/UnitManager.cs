using System.Collections.Generic;
using Goap;

public static class UnitManager {

    private static List<GoapAgent> monsterList = new List<GoapAgent>();

    public static void AddMonster(GoapAgent goapAgent)
    {
        monsterList.Add(goapAgent);
    }

    public static void RemoveMonster(GoapAgent goapAgent)
    {
        monsterList.Remove(goapAgent);
    }

    public static List<GoapAgent> MonsterList { get { return monsterList; } }

}