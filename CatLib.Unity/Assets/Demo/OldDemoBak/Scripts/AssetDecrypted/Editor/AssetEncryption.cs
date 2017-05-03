
using CatLib.API;
using CatLib.API.Resources;
using CatLib.API.AssetBuilder;

namespace CatLib.Resources
{

    /// <summary>
    /// 对打包的资源加入加密策略
    /// </summary>
    public class AssetEncryption : IBuildStrategy
    {


        [Inject]
        public IEnv Env { get; set; }

        public BuildProcess Process { get { return BuildProcess.Encryption; } }

        public void Build(IBuildContext context)
        {

            UnityEngine.Debug.Log(Env.AssetPath);
            var ioCrypted = new IOCrypted();
            foreach (string releaseFile in context.ReleaseFiles)
            {
                //todo: 进行加密
                ioCrypted.Encrypted("", new byte[] { });
                UnityEngine.Debug.Log(releaseFile);
            }

            context.EncryptionFiles = context.ReleaseFiles;
        }



    }

}