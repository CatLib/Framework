
using System;
using System.Collections;
using CatLib;
using CatLib.API.IO;

public class IOConfig : Configs
{

    /// <summary>
    /// 类
    /// </summary>
    public override Type Class
    {
        get
        {
            return typeof(CatLib.IO.IO);
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
                typeof(IDisk).ToString() , new Hashtable(){ 
								{ "crypt" , typeof(IIOCrypt) } 
							}
            };
        }
    }

}
