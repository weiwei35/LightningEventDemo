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
[MenuItem("工具/编译配置")]
    public static void 编译配置()
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
                    string itemValue = worksheet.GetValue(i, j).ToString();
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
}
