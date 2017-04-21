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

namespace CatLib.Translation
{
    /// <summary>
    /// 文件映射
    /// </summary>
    public interface IFileMapping
    {
        /// <summary>
        /// 获取映射
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="def">默认值</param>
        /// <returns>值</returns>
        string Get(string key, string def = null);
    }
}