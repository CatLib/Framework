/*
 * This file is part of the CatLib package.
 *
 * (c) Ming ming <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
// ===============================================================================
// File Name           :    LuaFileCreateEditor.cs
// Class Description   :    在Unity Editor下右键创建文本及Lua文件
// Author              :    Mingming
// Create Time         :    2017-04-20 18:43:29
// ===============================================================================
// Copyright © Mingming . All rights reserved.
// ===============================================================================
public class LuaFileCreateEditor : Editor 
{

    [MenuItem("Assets/Create/CatLib/Xlua File")]
    public static void CreateXLuaFile() {
        CreateFile("lua","lua.txt");
    }

    [MenuItem("Assets/Create/CatLib/Lua File")]
    public static void CreateLuaFile() {
        CreateFile("lua","lua");
    }

    [MenuItem("Assets/Create/CatLib/Text File")]
    public static void CreateTextFile() {
        CreateFile("txt","txt");
    }

    private static void CreateFile(string fileName,string fileEx) 
    {
        //获取当前所选择的目录（相对于Assets路径）
        string selectPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        string path = UnityEngine.Application.dataPath.Replace("Assets","") + "/";
        string newFileName = "new_" + fileName + "." + fileEx;
        string newFilePath = selectPath + "/" + newFileName;
        string fullPath = path + newFilePath;

        //重名处理
        if (File.Exists(fullPath))
        {
            string newName = "new_" + fileName + "_" + Random.Range(0, 1000) + "." + fileEx;
            newFilePath = selectPath + "/" + newName;
            fullPath = fullPath.Replace(newFileName, newName);
        }
        //如果是空白文件，编码并没有设置UTF8
        File.WriteAllText(fullPath, "-- test", System.Text.Encoding.UTF8);
        AssetDatabase.Refresh();
        //选中新创建的文件
        Object asset = AssetDatabase.LoadAssetAtPath(newFilePath, typeof(Object));
        Selection.activeObject = asset;
    }
}
