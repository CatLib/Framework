using CatLib;
using CatLib.API.IO;


/// <summary>
/// 文件解密接口
/// </summary>
public class IOCryptProvider : ServiceProvider
{

    public override void Register()
    {
        //App.Singleton<IOCrypted>().Alias<IIOCrypt>();
    }

}