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
            string[] lines = data.Split(new string[]{ "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            List<string> lst = new List<string>();
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