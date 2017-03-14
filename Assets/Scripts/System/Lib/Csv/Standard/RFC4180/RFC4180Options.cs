

public class RFC4180Options{

    /// <summary>
    /// 引用字符
    /// </summary>
    public char QuoteChar { get; set; }
    
    /// <summary>
    /// 分隔符
    /// </summary>
    public char DelimiterChar { get; set; }

    public RFC4180Options()
    {
        QuoteChar = '"';
        DelimiterChar = ',';
    }

}
