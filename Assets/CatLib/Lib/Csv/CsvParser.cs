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

    public class CsvParser : ICsvParser
    {

        private CsvParserOptions options;

        public CsvParser(CsvParserOptions options)
        {

            this.options = options;

        }

        /// <summary>
        /// 格式化csv
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string[][] Parser(string data)
        {
            string[] lines = ParseLine(data);
            List<string[]> query = new List<string[]>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (i == 0 && options.SkipHeader) { continue; }
                query.Add(options.Standard.Parse(lines[i]));
            }

            return query.ToArray();
        }

        /// <summary>
        /// 格式化成行
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected string[] ParseLine(string data)
        {
            List<string> lst = new List<string>();
            data = data.Replace("\r\n" , System.Environment.NewLine);
            string[] lines = data.Split(new string[]{ System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries);
            
            for(int i = 0; i < lines.Length; i++)
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