/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
 
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
