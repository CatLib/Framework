/*
 * This file is part of the CatLib package.
 *
 * (c) Ming ming <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System;
using CatLib.API;

namespace CatLib.Lua
{
    /// <summary>
    /// Lua适配器
    /// </summary>
    public interface ILuaEngineAdapter : IDestroy
    {
        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="script">脚本</param>
        /// <returns>
        /// 执行结果
        /// 如果只有一个返回值那么返回的将是结果值
        /// 反之则是一个结果数组
        /// </returns>
        object DoString(byte[] script);

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="script">脚本</param>
        /// <returns>
        /// 执行结果
        /// 如果只有一个返回值那么返回的将是结果值
        /// 反之则是一个结果数组
        /// </returns>
        object DoString(string script);

        /// <summary>
        /// 增加加载器
        /// </summary>
        /// <param name="callback">用户定义的脚本加载器</param>
        void AddLoader(Func<string,byte[]> callback);

        /// <summary>
        /// 移除加载器
        /// </summary>
        /// <param name="callback">要被移除的加载器</param>
        /// <returns>是否成功</returns>
        bool RemoveLoader(Func<string,byte[]> callback);
    }
}

