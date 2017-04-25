using CatLib;
using CatLib.API;
using CatLib.API.Config;

public class EnvConfig : IConfig
{

    /// <summary>
    /// 类
    /// </summary>
    public string Name
    {
        get
        {
            return typeof(Env).ToString();
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
                "debug" , DebugLevels.Dev,
            };
        }
    }

}
