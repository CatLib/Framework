using CatLib.API.Config;
using CatLib.Translation;

public class TransConfig : IConfig {

	/// <summary>
    /// 类
    /// </summary>
    public object Name
    {
        get
        {
            return typeof(Translator);
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
                "root" , "lang",
				"fallback" , "zh"
            };
        }
    }
}
