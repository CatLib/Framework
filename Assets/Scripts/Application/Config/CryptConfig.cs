using CatLib.API;
using CatLib.Crypt;
using System;

public class CryptConfig : Configs
{

    /// <summary>
    /// 类
    /// </summary>
    public override Type Class
    {
        get
        {
            return typeof(Crypt);
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
                "key" , "12345678901234567890123456789012",
            };
        }
    }

}
