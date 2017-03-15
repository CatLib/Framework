using System;
using CatLib.API.Config;
using CatLib.AutoUpdate;

public class AutoUpdateConfig : IConfig{

    /// <summary>
    /// 类
    /// </summary>
	public object Name
    {
        get
        {
            return typeof(AutoUpdate);
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
                "update.url" , "http://pvp.oss-cn-shanghai.aliyuncs.com/demo"
                //"update.api" , "api请求地址"
            };
        }
    }

}
