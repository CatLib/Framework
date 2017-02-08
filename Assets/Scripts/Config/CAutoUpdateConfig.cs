using UnityEngine;
using System.Collections;
using CatLib.UpdateSystem;
using System;

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
                "update.url" , new string[] { "http://pvp.oss-cn-shanghai.aliyuncs.com" }
            };
        }
    }

}
