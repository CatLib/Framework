
using CatLib.API.ResourcesSystem;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CatLib.ResourcesSystem
{

    public class GenTableStrategy : IBuildStrategy
    {

        public BuildProcess Process { get { return BuildProcess.GenTable; } }

        public void Build(IBuildContext context)
        {

            context.EncryptionFiles = context.ReleaseFiles;

            BuildAssetFile(context);

        

            /*
            string[] assetBundles = AssetDatabase.GetAllAssetBundleNames();
            string[] assetFiles;
            string head = "Assets" + IO.IO.PATH_SPLITTER + Env.ResourcesBuildPath;
            string relativeFile;
            string extension;
            string variant;
            List<string> assetFileList = new List<string>();
            for (int i = 0; i < assetBundles.Length; i++)
            {
                assetFiles = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundles[i]);
                for(int n = 0;n < assetFiles.Length; n++)
                {
                    relativeFile = assetFiles[n].Substring(head.Length);
                    extension = System.IO.Path.GetExtension(relativeFile);

                    relativeFile = relativeFile.Substring(0, relativeFile.Length - extension.Length);
                    variant = System.IO.Path.GetExtension(relativeFile);

                    if(variant != string.Empty)
                    {
                        relativeFile = relativeFile.Substring(0, relativeFile.Length - variant.Length);
                    }

                    if (!assetFileList.Contains(relativeFile + extension))
                    {
                        assetFileList.Add(relativeFile + extension);
                    }

                }
            }*/



        }

        protected void BuildAssetFile(IBuildContext context)
        {

            AssetFileStore store = new AssetFileStore();
            AssetFile file = new AssetFile();
            if (context.EncryptionFiles != null)
            {
                for (int i = 0; i < context.EncryptionFiles.Length; i++)
                {
                    file.Append(context.EncryptionFiles[i], true);
                }
            }

            store.Save(context.ReleasePath, file);
            List<string> releasePath = new List<string>(context.ReleaseFiles);
            releasePath.Add(AssetFileStore.FILE_NAME);
            context.ReleaseFiles = releasePath.ToArray();

        }



    }

}