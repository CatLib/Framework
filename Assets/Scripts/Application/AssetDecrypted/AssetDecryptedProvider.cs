using CatLib;
using CatLib.API.ResourcesSystem;


/// <summary>
/// 资源服务提供商
/// </summary>
public class AssetDecryptedProvider : ServiceProvider
{

    public override void Register()
    {
        App.Singleton<AssetDecrypted>().Alias<IDecrypted>();
    }

}