using CatLib.Lua;
using System;
using CatLib.Container;

public class CLuaConfig : CConfig
{

    /// <summary>
    /// 类
    /// </summary>
    public override Type Class
    {
        get
        {
            return typeof(CLua);
        }
    }

    /// <summary>
    /// 配置
    /// </summary>
    protected override object[] Field
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
