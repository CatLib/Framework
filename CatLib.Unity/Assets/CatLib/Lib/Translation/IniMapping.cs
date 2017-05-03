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

using CatLib.API.INI;

namespace CatLib.Translation
{
    /// <summary>
    /// ini映射
    /// </summary>
    public sealed class IniMapping : IFileMapping
    {
        /// <summary>
        /// 结果集
        /// </summary>
        private readonly IIniResult result;

        /// <summary>
        /// 构建一个ini映射集
        /// </summary>
        /// <param name="result">结果集</param>
        public IniMapping(IIniResult result)
        {
            this.result = result;
        }

        /// <summary>
        /// 获取映射
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="def">默认值</param>
        /// <returns>值</returns>
        public string Get(string key, string def = null)
        {
            return result.Get(string.Empty, key, def);
        }
    }
}