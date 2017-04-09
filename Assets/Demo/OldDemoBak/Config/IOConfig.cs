using System.Collections;
using CatLib.API.Config;
using CatLib.API.IO;

public class IOConfig : IConfig
{

    /// <summary>
    /// 类
    /// </summary>
    public object Name
    {
        get
        {
            return typeof(CatLib.IO.IO);
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
                typeof(IDisk).ToString() , new Hashtable(){ 
								{ "crypt" , typeof(IIOCrypt) } 
							}
            };
        }
    }

}
