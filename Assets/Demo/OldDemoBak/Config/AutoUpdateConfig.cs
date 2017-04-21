using System;
using CatLib.API.Config;
using CatLib.AutoUpdate;

public class AutoUpdateConfig : IConfig{

    /// <summary>
    /// 类
    /// </summary>
	public string Name
    {
        get
        {
            return typeof(AutoUpdate).ToString();
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
