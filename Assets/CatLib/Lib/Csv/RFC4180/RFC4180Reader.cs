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

        /// <summary>
        /// 解析内容
        /// </summary>
        public string[] Parse(StringReader reader)
        {
            bool isContinue = true;
            string content;
            var cols = new List<string>();
            while (isContinue)
            {
                content = NextBlock(reader , out isContinue);

                if (content != null)
                {
                    cols.Add(content);
                }
            }
            return cols.ToArray();
        }

        /// <summary>
        /// 下一个区块
        /// </summary>
        /// <param name="reader">字符串</param>
        /// <param name="isContinue">是否继续循环</param>
        /// <returns></returns>
        private string NextBlock(StringReader reader , out bool isContinue)
        {

            string result = string.Empty;

            int c = reader.Peek();

            if (c == options.DelimiterChar)
            {
                //如果没有内容则返回空
                reader.Read();
                isContinue = true;
                return string.Empty;
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
            //去除第1个引用标记
            reader.Read();

            //读取到第2个引用标记的内容
            string result = ReadTo(reader , options.QuoteChar);

            //去除第2个引用标记
            reader.Read();

            if (reader.Peek() != options.QuoteChar)
            {
                //不是引用标记则直接返回
                return result;
            }

            //如果连续引用则说明引用转义
            StringBuilder buffer = new StringBuilder(result);
            do
            {
                //一直循环直到没有引用转义的内容
                buffer.Append((char)reader.Read());
                buffer.Append(ReadTo(reader, options.QuoteChar));
                reader.Read();

            } while (reader.Peek() == options.QuoteChar);

            return buffer.ToString();
        }

        /// <summary>
        /// 是否是分隔符
        /// </summary>
        /// <param name="c">字符</param>
        /// <returns></returns>
        private bool IsDelimiter(int c)
        {
            return c == options.DelimiterChar;
        }

        /// <summary>
        /// 是否是结束位置
        /// </summary>
        /// <param name="c">字符</param>
        /// <returns></returns>
        private bool IsEndOfStream(int c)
        {
            return c == -1;
        }

        /// <summary>
        /// 读取到指定字符位置
        /// </summary>
        /// <param name="reader">字符流</param>
        /// <param name="readTo">字符</param>
        /// <returns></returns>
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