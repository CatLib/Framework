

using CatLib.API.Config;
using CatLib.CsvStore;

public class CsvStoreConfig : IConfig
{

    /// <summary>
    /// 类
    /// </summary>
    public string Name
    {
        get
        {
            return typeof(CsvStore).ToString();
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
                "root" , "csv",
            };
        }
    }

}
