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
using UnityEditor;
using System.IO;

namespace CatLib.Lua
{
    /// <summary>
    /// Lua文件编辑器强化
    /// </summary>
    public class LuaFileCreateEditor : Editor
    {

        [MenuItem("Assets/CatLib/Create/Xlua File")]
        public static void CreateXLuaFile()
        {
            CreateFile("lua", "lua.txt");
        }

        [MenuItem("Assets/CatLib/Create/Lua File")]
        public static void CreateLuaFile()
        {
            CreateFile("lua", "lua");
        }

        [MenuItem("Assets/CatLib/Create/Text File")]
        public static void CreateTextFile()
        {
            CreateFile("txt", "txt");
        }

        private static void CreateFile(string fileName, string fileEx)
        {
            //获取当前所选择的目录（相对于Assets路径）
            var selectPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            var path = UnityEngine.Application.dataPath.Replace("Assets", "") + Path.AltDirectorySeparatorChar;
            var newFileName = "new_" + fileName + "." + fileEx;
            var newFilePath = selectPath + Path.AltDirectorySeparatorChar + newFileName;
            var fullPath = path + newFilePath;

            //重名处理
            if (File.Exists(fullPath))
            {
                var newName = "new_" + fileName + "_" + Random.Range(0, 1000) + "." + fileEx;
                newFilePath = selectPath + Path.AltDirectorySeparatorChar + newName;
                fullPath = fullPath.Replace(newFileName, newName);
            }

            //如果是空白文件，编码并没有设置UTF8
            File.WriteAllText(fullPath, "-- test", System.Text.Encoding.UTF8);
            AssetDatabase.Refresh();

            //选中新创建的文件
            var asset = AssetDatabase.LoadAssetAtPath(newFilePath, typeof(Object));
            Selection.activeObject = asset;
        }
    }
}
