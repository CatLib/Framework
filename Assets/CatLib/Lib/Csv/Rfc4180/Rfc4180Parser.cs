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

using System.IO;

namespace CatLib.Csv
{
    /// <summary>
    /// RFC4180解析器
    /// </summary>
    internal sealed class RFC4180Parser : IStandard
    {
        /// <summary>
        /// 数据渲染器
        /// </summary>
        private readonly Rfc4180Reader render;

        /// <summary>
        /// 构建一个解析器
        /// </summary>
        /// <param name="options">解析配置</param>
        public RFC4180Parser(Rfc4180Options options)
        {
            render = new Rfc4180Reader(options);
        }

        /// <summary>
        /// 解析每一行数据
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public string[] Parse(string line)
        {
            using (var sr = new StringReader(line))
            {
                return render.Parse(sr);
            }
        }
    }
}