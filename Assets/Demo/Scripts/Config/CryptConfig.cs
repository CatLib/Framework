using CatLib.Crypt;
using CatLib.API.Config;

public class CryptConfig : IConfig
{

    /// <summary>
    /// 类
    /// </summary>
    public object Name
    {
        get
        {
            return typeof(Crypt);
        }
    }

    /// <summary>
    /// 配置
    /// </summary>
    public object[] Config
    {
        get
        {
            return new object[]
            {
                "key" , "12345678901234567890123456789012",
            };
        }
    }

}
