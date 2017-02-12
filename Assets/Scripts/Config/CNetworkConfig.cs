using CatLib.Container;
using CatLib.Network;
using System;

public class CNetworkConfig : CConfig {

    /// <summary>
    /// 类
    /// </summary>
    public override Type Class
    {
        get
        {
            return typeof(CNetwork);
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
                "connector.test"    , "http://127.0.0.1/testcookie.php",
                "connector.testtcp" , "127.0.0.1:7777",
            };
        }
    }

}
