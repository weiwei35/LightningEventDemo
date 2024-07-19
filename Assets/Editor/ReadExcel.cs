using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using OfficeOpenXml;
using UnityEditor;
using UnityEngine;

public class Startup
{
[MenuItem("工具/编译道具配置")]
    public static void 编译道具配置()
    {
        string path = Application.dataPath + "/Editor/ItemData.xlsx";
        string assetName = "ItemData";
        FileInfo fileInfo = new FileInfo(path);
        //创建so类
        ItemDataSO itemData = (ItemDataSO)ScriptableObject.CreateInstance(typeof(ItemDataSO));
        //打开Excel文件，using会在使用完毕后关闭文件
        using(ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            //选取表单
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["道具"];
            //遍历每一行
            for (int i = worksheet.Dimension.Start.Row+3; i <= worksheet.Dimension.End.Row; i++)
            {
                SelectItem item = new SelectItem();
                //获取每列数据的数据类型
                Type itemType = typeof(SelectItem);
                //遍历每一列
                for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                {
                    //用反射对item赋值，将数据类型附加到赋值内容中
                    FieldInfo typeInfo = itemType.GetField(worksheet.GetValue(1, j).ToString());
                    string itemValue = "";
                    if (worksheet.GetValue(i, j) != null){
                        itemValue = worksheet.GetValue(i, j).ToString();
                    }
                    typeInfo.SetValue(item,Convert.ChangeType(itemValue,typeInfo.FieldType));
                }
                //当前行赋值结束，添加到列表中
                itemData.items.Add(item);
            }
        }
        //保存SO文件
        if(File.Exists("Assets/Resources/" + assetName +".asset"))
        {
            File.Delete("Assets/Resources/" + assetName +".asset");
            if(File.Exists("Assets/Resources/" + assetName +".meta"))
                File.Delete("Assets/Resources/" + assetName +".meta");
            AssetDatabase.CreateAsset(itemData,"Assets/Resources/" + assetName +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }else{
            AssetDatabase.CreateAsset(itemData,"Assets/Resources/" + assetName +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
[MenuItem("工具/编译经验配置")]
    public static void 编译经验配置()
    {
        string path = Application.dataPath + "/Editor/ItemData.xlsx";
        string assetName2 = "ExpLevelData";
        FileInfo fileInfo = new FileInfo(path);
        //创建so类
        ExpLevelDataSO expLevelData = (ExpLevelDataSO)ScriptableObject.CreateInstance(typeof(ExpLevelDataSO));
        //打开Excel文件，using会在使用完毕后关闭文件
        using(ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            //选取表单
            ExcelWorksheet worksheet2 = excelPackage.Workbook.Worksheets["经验"];
            //遍历每一行
            for (int i = worksheet2.Dimension.Start.Row+3; i <= worksheet2.Dimension.End.Row; i++)
            {
                ExpLevel item = new ExpLevel();
                //获取每列数据的数据类型
                Type itemType1 = typeof(ExpLevel);
                //遍历每一列
                for (int j = worksheet2.Dimension.Start.Column; j <= worksheet2.Dimension.End.Column; j++)
                {
                    //用反射对item赋值，将数据类型附加到赋值内容中
                    FieldInfo typeInfo = itemType1.GetField(worksheet2.GetValue(1, j).ToString());
                    string itemValue = worksheet2.GetValue(i, j).ToString();
                    typeInfo.SetValue(item,Convert.ChangeType(itemValue,typeInfo.FieldType));
                }
                //当前行赋值结束，添加到列表中
                expLevelData.items.Add(item);
            }
        }
        //保存SO文件
        if(File.Exists("Assets/Resources/" + assetName2 +".asset"))
        {
            File.Delete("Assets/Resources/" + assetName2 +".asset");
            if(File.Exists("Assets/Resources/" + assetName2 +".meta"))
                File.Delete("Assets/Resources/" + assetName2 +".meta");
            AssetDatabase.CreateAsset(expLevelData,"Assets/Resources/" + assetName2 +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }else{
            AssetDatabase.CreateAsset(expLevelData,"Assets/Resources/" + assetName2 +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
[MenuItem("工具/编译波次配置")]
    public static void 编译波次配置()
    {
        string path = Application.dataPath + "/Editor/ItemData.xlsx";
        string assetName2 = "EnemyGroupData";
        FileInfo fileInfo = new FileInfo(path);
        //创建so类
        EnemyGroupDataSO enemyGroupData = (EnemyGroupDataSO)ScriptableObject.CreateInstance(typeof(EnemyGroupDataSO));
        //打开Excel文件，using会在使用完毕后关闭文件
        using(ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            //选取表单
            ExcelWorksheet worksheet2 = excelPackage.Workbook.Worksheets["波次"];
            //遍历每一行
            for (int i = worksheet2.Dimension.Start.Row+3; i <= worksheet2.Dimension.End.Row; i++)
            {
                EnemyLevelGroup item = new EnemyLevelGroup();
                //获取每列数据的数据类型
                Type itemType1 = typeof(EnemyLevelGroup);
                //遍历每一列
                for (int j = worksheet2.Dimension.Start.Column; j <= worksheet2.Dimension.End.Column; j++)
                {
                    //用反射对item赋值，将数据类型附加到赋值内容中
                    FieldInfo typeInfo = itemType1.GetField(worksheet2.GetValue(1, j).ToString());
                    string itemValue = worksheet2.GetValue(i, j).ToString();
                    typeInfo.SetValue(item,Convert.ChangeType(itemValue,typeInfo.FieldType));
                }
                //当前行赋值结束，添加到列表中
                enemyGroupData.enemyLevelGroups.Add(item);
            }
        }
        //保存SO文件
        if(File.Exists("Assets/Resources/" + assetName2 +".asset"))
        {
            File.Delete("Assets/Resources/" + assetName2 +".asset");
            if(File.Exists("Assets/Resources/" + assetName2 +".meta"))
                File.Delete("Assets/Resources/" + assetName2 +".meta");
            AssetDatabase.CreateAsset(enemyGroupData,"Assets/Resources/" + assetName2 +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }else{
            AssetDatabase.CreateAsset(enemyGroupData,"Assets/Resources/" + assetName2 +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
