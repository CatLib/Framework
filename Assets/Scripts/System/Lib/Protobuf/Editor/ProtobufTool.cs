using System.Diagnostics;
using UnityEditor;
using CatLib.API;

namespace CatLib.Protobuf
{

    public static class ProtobufTool
    {

        /// <summary>
        /// 编译工具路径
        /// </summary>
        public static string GenToolPath = UnityEngine.Application.dataPath + "/Scripts/System/Lib/Protobuf/Editor/Gen/";

        public static string Arguments = "-i:{in} -o:{out}";

        [MenuItem("CatLib/Protobuf Builder/Build", false, 5)]
        public static void BuildProtobuf()
        {
            IEnv env = App.Instance.Make<IEnv>();

            string protoPath = EditorUtility.OpenFilePanel("Choose .Proto File", UnityEngine.Application.dataPath, "proto");

            if (string.IsNullOrEmpty(protoPath)) { return; }

            string saveTo = EditorUtility.SaveFilePanel("Save Proto File", protoPath, System.IO.Path.GetFileNameWithoutExtension(protoPath), "cs");

            if (string.IsNullOrEmpty(saveTo)) { return; }

            string call = null;
            if(env.Platform == UnityEngine.RuntimePlatform.WindowsEditor)
            {
                call = "protogen.exe";
            }

            if (string.IsNullOrEmpty(call)) { UnityEngine.Debug.Log("not support this platform to build"); return; }

            ProcessStartInfo start = new ProcessStartInfo(GenToolPath + call);

            start.Arguments = Arguments.Replace("{in}", protoPath).Replace("{out}", saveTo);
            start.CreateNoWindow = false;
            start.ErrorDialog = true;
            start.UseShellExecute = true;

            Process p = Process.Start(start);

        }


    }
}