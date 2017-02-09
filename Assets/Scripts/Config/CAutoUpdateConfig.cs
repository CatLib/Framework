using UnityEngine;
using System.Collections;
using CatLib.UpdateSystem;
using System;
using UnityEngine.Networking;

public class CAutoUpdateConfig : CConfig{

    /// <summary>
    /// 类
    /// </summary>
	public override Type Class
    {
        get
        {
            return typeof(CAutoUpdate);
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
