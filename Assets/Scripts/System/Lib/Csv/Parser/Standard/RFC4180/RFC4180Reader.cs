using System.Text;
using System.IO;
using System.Collections.Generic;

namespace CatLib.Csv
{

    public class RFC4180Reader
    {

        private RFC4180Options options;

        public RFC4180Reader(RFC4180Options options)
        {
            this.options = options;
        }

        public string[] Parse(StringReader reader)
        {
            bool isContinue = true;
            string content;
            var cols = new List<string>();
            while (isContinue)
            {
                content = NextToken(reader , out isContinue);

                if (content != null)
                {
                    cols.Add(content);
                }
            }
            return cols.ToArray();
        }

        private string NextToken(StringReader reader , out bool isContinue)
        {

            string result = string.Empty;

            int c = reader.Peek();

            //分隔符
            if (c == options.DelimiterChar)
            {
                reader.Read();
                isContinue = true;
                return null;
            }
            else
            {
                //如果是引用字符那么读取引用
                if (c == options.QuoteChar)
                {
                    result = ReadQuoted(reader);

                    if (IsEndOfStream(reader.Peek()))
                    {
                        isContinue = false;
                        return result;
                    }

                    if (IsDelimiter(reader.Peek()))
                    {
                        reader.Read();
                    }

                    isContinue = true;
                    return result;
                }

                if (IsEndOfStream(c))
                {
                    isContinue = false;
                    return null;
                }
                else
                {
                    result = ReadTo(reader , options.DelimiterChar);

                    if (IsEndOfStream(reader.Peek()))
                    {
                        isContinue = false;
                        return result;
                    }

                    if (IsDelimiter(reader.Peek()))
                    {
                        reader.Read();
                    }

                    isContinue = true;
                    return result;
                }


            }
        }

        /// <summary>
        /// 读取引用字符
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private string ReadQuoted(StringReader reader)
        {
            reader.Read();

            string result = ReadTo(reader , options.QuoteChar);

            reader.Read();

            if (reader.Peek() != options.QuoteChar)
            {
                return result;
            }

            StringBuilder buffer = new StringBuilder(result);
            do
            {

                buffer.Append((char)reader.Read());
                buffer.Append(ReadTo(reader, options.QuoteChar));
                reader.Read();

            } while (reader.Peek() == options.QuoteChar);

            return buffer.ToString();
        }

        private bool IsDelimiter(int c)
        {
            return c == options.DelimiterChar;
        }

        private bool IsEndOfStream(int c)
        {
            return c == -1;
        }

        private string ReadTo(StringReader reader, char readTo)
        {
            StringBuilder buffer = new StringBuilder();
            while (reader.Peek() != -1 && reader.Peek() != readTo)
            {
                buffer.Append((char)reader.Read());
            }
            return buffer.ToString();
        }

    }

}