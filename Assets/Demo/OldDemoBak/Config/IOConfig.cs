using System.Collections;
using CatLib.API.Config;
using CatLib.API.IO;

public class IOConfig : IConfig
{

    /// <summary>
    /// 类
    /// </summary>
    public string Name
    {
        get
        {
            return typeof(CatLib.IO.IO).ToString();
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
