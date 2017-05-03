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

using System.Collections.Generic;
using CatLib.API.Csv;

namespace CatLib.Csv
{
    /// <summary>
    /// Csv解析器
    /// </summary>
    public sealed class CsvParser : ICsvParser
    {
        /// <summary>
        /// Csv解析器配置
        /// </summary>
        private readonly CsvParserOptions options;

        /// <summary>
        /// 构建一个Csv解析器
        /// </summary>
        /// <param name="options">Csv解析器配置</param>
        public CsvParser(CsvParserOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// 解析Csv数据
        /// </summary>
        /// <param name="data">需要被解析的字符串</param>
        /// <returns>返回的二维数组为行和列的数据</returns>
        public string[][] Parser(string data)
        {
            var lines = ParseLine(data);
            var query = new List<string[]>();

            for (var i = 0; i < lines.Length; i++)
            {
                if (i == 0 && options.SkipHeader)
                {
                    continue;
                }
                query.Add(options.Standard.Parse(lines[i]));
            }

            return query.ToArray();
        }

        /// <summary>
        /// 格式化成行
        /// </summary>
        /// <param name="data">需要被解析的字符串</param>
        /// <returns>每行数据</returns>
        private string[] ParseLine(string data)
        {
            var lst = new List<string>();
            data = data.Replace("\r\n", System.Environment.NewLine);
            var lines = data.Split(new[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].TrimStart()[0] == options.AnnotationChar)
                {
                    continue;
                }
                lst.Add(lines[i]);
            }
            return lst.ToArray();
        }
    }
}