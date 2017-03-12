using System.Collections.Generic;
using CatLib.API.Csv;
using System.IO;
using System.Text;

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
            /*
            List<string> lst = new List<string>();
            StringReader sr = new StringReader(data);
            string line;
            while (true)
            {
                line = NextLine(sr);

                lst.Add(line);

            }*/



            List<string> lst = new List<string>();
            string[] lines = data.Split(new string[]{ "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            
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

        /*
        protected string NextLine(StringReader reader)
        {

            string result = string.Empty;

            int c = reader.Peek();

            if (c == '"')
            {
                result += ReadQuoted(reader);
            }
            else
            {
                result += c;
            }

        }

        private string ReadQuoted(StringReader reader)
        {
            reader.Read();

            string result = ReadTo(reader, '"');

            reader.Read();

            if (reader.Peek() != '"')
            {
                return result;
            }

            StringBuilder buffer = new StringBuilder(result);
            do
            {

                buffer.Append((char)reader.Read());
                buffer.Append(ReadTo(reader, '"'));
                reader.Read();

            } while (reader.Peek() == '"');

            return buffer.ToString();
        }

        private string ReadTo(StringReader reader, char readTo)
        {
            StringBuilder buffer = new StringBuilder();
            while (reader.Peek() != -1 && reader.Peek() != readTo)
            {
                buffer.Append((char)reader.Read());
            }
            return buffer.ToString();
        }*/

    }

}