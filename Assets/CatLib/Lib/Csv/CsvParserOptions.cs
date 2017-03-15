

namespace CatLib.Csv
{

    /// <summary>
    /// Csv选项
    /// </summary>
    public class CsvParserOptions
    {

        /// <summary>
        /// 是否跳过头
        /// </summary>
        public bool SkipHeader { get; set; }

        /// <summary>
        /// 行注释字符
        /// </summary>
        public char AnnotationChar { get; set; }

        /// <summary>
        /// 使用的解释标准
        /// </summary>
        public IStandard Standard { get; set; }

        public CsvParserOptions(IStandard standard)
        {
            Standard = standard;
            SkipHeader = false;
            AnnotationChar = ';';
        }

    }

}