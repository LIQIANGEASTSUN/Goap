using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Reflection;

#pragma warning disable 0219,0414

public class LoadTableData {
    public static readonly LoadTableData Instance = new LoadTableData();

    // 加载所有数据表
    public void LoadAllTable()
    {
        List<string> tableNameList = new List<string>();

        string[] tableFiles = new string[] { };

        for (int i = 0; i < (int)TableType.None; ++i)
        {
            string tableName = TableDefine.GetTableName((TableType)i);
            tableNameList.Add(tableName);
        }

        LoadCallBackHandler LoadTableCallBack = (handlerParam) =>
        {
            TextAsset textAsset = (TextAsset)handlerParam.assetObj;
            GetTableData(handlerParam.assetName, textAsset);
        };

        for (int i = 0; i < tableNameList.Count; ++i)
        {
            // 读取数据表
            AssetPool.Instance.TableData.LoadAsset<TextAsset>(tableNameList[i], LoadTableCallBack, new System.Object[] { tableNameList[i] });
        }
    }

    // 解析表数据
    public void GetTableData( string tableName, TextAsset textAsset)
    {
        if (textAsset == null)
        {
            UnityEngine.Debug.LogWarning( tableName + " textAsset is null");
            return;
        }

        TableData tableData = GetData(tableName, textAsset);
        TableDataManager.Instance.AddTableData(tableName, tableData);
    }

	private Dictionary<string, int> errorAttributeInfo = new Dictionary<string, int> ();
    private TableData GetData(string tableName, TextAsset textAsset)
    {
        TableData tableData = new TableData();
        string[] rowData = textAsset.text.Split(new string[] { "\r\n" }, StringSplitOptions.None);  // 拆分行
        if (rowData.Length < 3)
        {
            return null;
        }
        
        List<PropertyInfo> propertyInfoList = GetPropertyInfoList(tableName, rowData[1]);
        for (int i = 3; i < rowData.Length; ++i)
        {
            // 实例化一个 T 类对象
            object row = TableDefine.GetTableType(tableName);
            System.Type t = row.GetType();
            string[] cloData = rowData[i].Split('`');  // 拆分行
            if (cloData.Length < propertyInfoList.Count || (cloData.Length > 0 && string.IsNullOrEmpty(cloData[0])))
            {
                continue;
            }
            PropertyInfo property;
            for (int j = 0; j < cloData.Length; ++j)
            {
                if (propertyInfoList.Count <= j)
                {
                    UnityEngine.Debug.LogError("属性列不对 " + tableName + "   列：" + j);
                    break;
                }

                property = propertyInfoList[j];
                try
                {
                    property.SetValue(row, Convert.ChangeType(cloData[j], property.PropertyType), null); // 给属性赋值
                }
                catch
                {
                    
                }
            }

            object keyID = -1;  // 主键
            if (propertyInfoList.Count > 0)
            {
                keyID = propertyInfoList[0].GetValue(row, null);  // 获取主键ID
            }
            tableData.AddRowData(keyID, row);
        }

        return tableData;
    }

    // 获取类所有属性
    private List<PropertyInfo> GetPropertyInfoList(string tableName, string attributesRow)
    {
        // 属性名数组
        string[] attributesNameArr = attributesRow.Split('`');

        List<PropertyInfo> propertyInfoList = new List<PropertyInfo>();
        // 实例化一个 T 类对象
        object row = TableDefine.GetTableType(tableName);
        // 获取该类类型  
        System.Type t = row.GetType();
        for (int i = 0; i < attributesNameArr.Length; ++i)  // 获取所有属性
        {
            PropertyInfo f_property = t.GetProperty(attributesNameArr[i]);
            if (f_property == null)
            {
                UnityEngine.Debug.LogError("*********发现一列对象与数据表'" + tableName + "'不匹配的属性:" + attributesNameArr[i]);
                continue;
            }
            propertyInfoList.Add(f_property);
        }

        return propertyInfoList;
    }
}