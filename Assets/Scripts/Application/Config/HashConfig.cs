using CatLib;
using CatLib.Hash;
using System;

public class HashConfig : Configs
{

    /// <summary>
    /// 类
    /// </summary>
    public override Type Class
    {
        get
        {
            return typeof(Hash);
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
                //2选1配置
                //"salt" , "$2a$10$VSE4DZuf5gJHQZFHSycoCe",
                //"factor" , 2
            };
        }
    }

}
