using System;
using CatLib;
using CatLib.API;

public class EnvConfig : Configs
{

    /// <summary>
    /// 类
    /// </summary>
    public override Type Class
    {
        get
        {
            return typeof(Env);
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
                "debug" , DebugLevels.Staging,
            };
        }
    }

}
