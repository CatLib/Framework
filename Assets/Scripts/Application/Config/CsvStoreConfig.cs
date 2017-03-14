

using CatLib.API.Config;
using CatLib.CsvStore;

public class CsvStoreConfig : IConfig
{

    /// <summary>
    /// 类
    /// </summary>
    public object Name
    {
        get
        {
            return typeof(CsvStore);
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
