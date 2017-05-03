/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System.Diagnostics;
using CatLib.API;
using UnityEditor;

namespace CatLib.Protobuf
{

    public static class ProtobufTool
    {

        /// <summary>
        /// 编译工具路径
        /// </summary>
        public static string GenToolPath = UnityEngine.Application.dataPath + "/CatLib/Lib/Protobuf/Editor/Gen/";

        public static string Arguments = "-i:{in} -o:{out}";

        [MenuItem("CatLib/Protobuf Builder/Build", false, 5)]
        public static void BuildProtobuf()
        {

            string savekey = "_" + typeof(ProtobufTool).ToString() + ".BuildProtobuf";

            IEnv env = App.Instance.Make<IEnv>();

            string protoPath = EditorUtility.OpenFilePanel("Choose .Proto File", UnityEngine.PlayerPrefs.GetString(savekey, UnityEngine.Application.dataPath), "proto");

            if (string.IsNullOrEmpty(protoPath)) { return; }

            string saveTo = EditorUtility.SaveFilePanel("Save Proto File", protoPath, System.IO.Path.GetFileNameWithoutExtension(protoPath), "cs");

            if (string.IsNullOrEmpty(saveTo)) { return; }

            UnityEngine.PlayerPrefs.SetString(savekey, System.IO.Path.GetDirectoryName(protoPath));

            string call = null;
            if(env.Platform == UnityEngine.RuntimePlatform.WindowsEditor)
            {
                call = GenToolPath + "protogen.exe";
            }else if(env.Platform == UnityEngine.RuntimePlatform.OSXEditor){
                //call = "mono " + GenToolPath + "protogen.exe";
            }

            if (string.IsNullOrEmpty(call)) { UnityEngine.Debug.Log("not support this platform to build"); return; }

            ProcessStartInfo start = new ProcessStartInfo(call);
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;
            start.Arguments = Arguments.Replace("{in}", protoPath).Replace("{out}", saveTo);
            start.CreateNoWindow = false;
            start.ErrorDialog = true;
            start.UseShellExecute = true;

            Process p = Process.Start(start);
            p.WaitForExit();
            UnityEngine.Debug.Log("protobuf build complete.");
            AssetDatabase.Refresh();

        }


    }
}