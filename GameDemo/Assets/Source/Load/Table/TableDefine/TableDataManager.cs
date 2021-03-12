using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TableData
{
    // key 为主键(int)， Value 为 整行数据
    private Dictionary<int, object> rowDataDic = new Dictionary<int, object>();
    // key 为主键(string)， Value 为 整行数据
    private Dictionary<string, object> rowStrDataDic = new Dictionary<string, object>();

    public void AddRowData(object ID, object rowObj)
    {
        if (ID.GetType() == typeof(int))
        {
            if (!rowDataDic.ContainsKey((int)ID))
            {
                rowDataDic.Add((int)ID, rowObj);
            }
        }
        else
        {
            if (!rowStrDataDic.ContainsKey((string)ID))
            {
                rowStrDataDic.Add((string)ID, rowObj);
            }
        }
    }

    public object GetData(int keyID)
    {
        object row = null;
        if (rowDataDic.TryGetValue(keyID, out row))
        {
            return row;
        }

        return row;
    }

    public object GetData(string keyID)
    {
        object row = null;
        if (rowStrDataDic.TryGetValue(keyID, out row))
        {
            return row;
        }

        return row;
    }

    public List<object> GetAllData()
    {
        List<object> rowDataList = new List<object>();
        IEnumerator ie = rowDataDic.GetEnumerator();
        while(ie.MoveNext())
        {
            KeyValuePair<int, object> kv = (KeyValuePair<int, object>)ie.Current;
            rowDataList.Add(kv.Value);
        }

        IEnumerator ie2 = rowStrDataDic.GetEnumerator();
        while (ie2.MoveNext())
        {
            KeyValuePair<string, object> kv = (KeyValuePair<string, object>)ie2.Current;
            rowDataList.Add(kv.Value);
        }

        return rowDataList;
    }
}

public delegate void LoadTableComplete();
public class TableDataManager {

    public static readonly TableDataManager Instance = new TableDataManager();

    private readonly Dictionary<string, TableData> tableDataDic = new Dictionary<string, TableData>();

    public static LoadTableComplete loadTableComplete;
    public int loadCount = 0;
    public void AddTableData(string tableName, TableData tableData)
    {
        if (tableDataDic.ContainsKey(tableName))
        {
            tableDataDic.Remove(tableName);
        }

        tableDataDic.Add(tableName, tableData);

        ++loadCount;
        if (loadCount >= (int)TableType.None && loadTableComplete != null)
        {
            loadTableComplete();
        }
    }
	
    // 根据表类型和主键ID读取数据
    public object GetTableData(TableType tableTpye, int keyID)
    {
        string tableName = TableDefine.GetTableName(tableTpye);

        TableData tableData = null;

        if (tableDataDic.TryGetValue(tableName, out tableData))
        {
            return tableData.GetData(keyID);
        }

        return null;
    }

    public object GetTableData(TableType tableTpye, string keyID)
    {
        string tableName = TableDefine.GetTableName(tableTpye);

        TableData tableData = null;

        if (tableDataDic.TryGetValue(tableName, out tableData))
        {
            return tableData.GetData(keyID);
        }

        return null;
    }

    // 读取整个表数据
    public List<object> GetTableData(TableType tableTpye)
    {
        string tableName = TableDefine.GetTableName(tableTpye);
        TableData tableData = null;

        if (tableDataDic.TryGetValue(tableName, out tableData))
        {
            return tableData.GetAllData();
        }

        return null;
    }
}