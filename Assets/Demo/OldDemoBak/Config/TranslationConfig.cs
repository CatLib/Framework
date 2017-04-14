using CatLib.API.Config;
using CatLib.Translation;

public class TransConfig : IConfig {

	/// <summary>
    /// 类
    /// </summary>
    public string Name
    {
        get
        {
            return typeof(Translator).ToString();
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
