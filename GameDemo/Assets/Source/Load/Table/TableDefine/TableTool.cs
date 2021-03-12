using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TableTool {
    //  读表示例
    //  根据ID 获取单行数据值
    //  SoldierClient soliderClient = TableTool.GetTableDataRow<SoldierClient>(TableType.SoldierClient, 1000);

    //  根据表类型读取所有数据值
    //  List<SoldierClient> soliderClientList = TableTool.GetTableData<SoldierClient>(TableType.SoldierClient);

    // 根据 ID 获取行数据
    public static T GetTableDataRow<T>(TableType tableType, int id) where T : new ()
    {
        return (T)TableDataManager.Instance.GetTableData( tableType, id);
    }

    public static T GetTableDataRow<T>(TableType tableType, string id) where T : new()
    {
        return (T)TableDataManager.Instance.GetTableData(tableType, id);
    }

    // 获取表所有行数据
    public static List<T> GetTableData<T>(TableType tableType) where T : new ()
    {
        List<object> tableDataObjectList = TableDataManager.Instance.GetTableData(tableType);

        List<T> tableDataList = new List<T>();
        for (int i = 0; i < tableDataObjectList.Count; ++i)
        {
            T monsterData = (T)tableDataObjectList[i];
            tableDataList.Add(monsterData);
        }

        return tableDataList;
    }
}