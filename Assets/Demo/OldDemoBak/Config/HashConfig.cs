using CatLib.Hash;
using CatLib.API.Config;

public class HashConfig : IConfig
{

    /// <summary>
    /// 类
    /// </summary>
    public string Name
    {
        get
        {
            return typeof(Hash).ToString();
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
                //2选1配置
                //"salt" , "$2a$10$VSE4DZuf5gJHQZFHSycoCe",
                //"factor" , 2
            };
        }
    }

}
