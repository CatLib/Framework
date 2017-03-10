using CatLib;
using CatLib.API;
using CatLib.API.Config;

public class EnvConfig : IConfig
{

    /// <summary>
    /// 类
    /// </summary>
    public object Service
    {
        get
        {
            return typeof(Env);
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
