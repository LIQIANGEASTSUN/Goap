using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public enum TableType
{
    /// <summary>
    /// 文本语言表
    /// </summary>
    Language,

    /// <summary>
    /// 技能表
    /// </summary>
    Skill,

    /// <summary>
    /// Npc 表
    /// </summary>
    Npc,

    None,
}

public class TableDefine
{
    private readonly static string language        = "Language";
    private readonly static string skill           = "Skill";
    private readonly static string npc             = "Npc";

    private static Dictionary<string, object> ObjectDic = new Dictionary<string, object>();

    private static Dictionary<TableType, string> TableTypeDic = new Dictionary<TableType, string>();
    static TableDefine()
    {
        ObjectDic.Clear();
        ObjectDic[language]                         = new LanguageData();
        TableTypeDic[TableType.Language]            = language;

        ObjectDic[skill]                            = new SkillData();
        TableTypeDic[TableType.Skill]               = skill;

        ObjectDic[npc]                              = new NpcData();
        TableTypeDic[TableType.Npc]                 = npc;
    }

    public static object GetTableType(string tableName)
    {
        object obj = null;
        if (ObjectDic.TryGetValue(tableName, out obj))
        {
            Type t = obj.GetType();
            object newobj = Activator.CreateInstance(t, true);//根据类型创建实例
            return newobj;
        }

        return obj;
    }

    public static string GetTableName(TableType tableTpye)
    {
        string tableName = string.Empty;
        if (TableTypeDic.TryGetValue(tableTpye, out tableName))
        {
            return tableName;
        }

        return tableName;  
    }
}