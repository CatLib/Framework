using CatLib.API.Config;
using CatLib.Lua;

public class LuaConfig : IConfig
{

    /// <summary>
    /// 类
    /// </summary>
    public string Name
    {
        get
        {
            return typeof(XLuaEngine).ToString();
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
                //热补丁文件路径
                "lua.hotfix" , new string[] { "scripts/hotfix" }
            };
        }
    }

}
