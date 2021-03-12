using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LANGUAGE_TYPE
{
    /// <summary>
    /// 中文
    /// </summary>
    CN    = 2,
    /// <summary>
    /// 英文
    /// </summary>
    EN    = 3,
}

public class LanguageString
{
    public static LANGUAGE_TYPE languageType = LANGUAGE_TYPE.CN;  // 语言类型 0 汉语， 1 英语

    public static string String(string textID)
    {
        LanguageData textData = TableTool.GetTableDataRow<LanguageData>(TableType.Language, int.Parse(textID));
        if (textData == null)
        {
            return textID;
        }

        if (languageType == LANGUAGE_TYPE.CN)
        {
            return textData.CN;
        }
        else if (languageType == LANGUAGE_TYPE.EN)
        {
            return textData.EN;
        }

        return textData.CN;
    }
}