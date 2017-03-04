using System;
using CatLib.API;
using CatLib.AutoUpdate;

public class AutoUpdateConfig : Configs{

    /// <summary>
    /// 类
    /// </summary>
	public override Type Class
    {
        get
        {
            return typeof(AutoUpdate);
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
                "update.url" , "http://pvp.oss-cn-shanghai.aliyuncs.com/demo"
                //"update.api" , "api请求地址"
            };
        }
    }

}
