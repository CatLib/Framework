
using CatLib.API.ResourcesSystem;

namespace CatLib.ResourcesSystem
{

    public class GenTableStrategy : IBuildStrategy
    {

        public BuildProcess Process { get { return BuildProcess.GenTable; } }

        public void Build(IBuildContext context)
        {

            context.EncryptionFiles = context.ReleaseFiles;

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

    }

}