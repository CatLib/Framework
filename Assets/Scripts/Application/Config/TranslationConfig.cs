using System;
using CatLib.API;
using CatLib.Translation;

public class TransConfig : Configs {

	/// <summary>
    /// 类
    /// </summary>
    public override Type Class
    {
        get
        {
            return typeof(Translator);
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
                "root" , "lang",
				"fallback" , "zh"
            };
        }
    }
}
