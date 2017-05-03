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

namespace CatLib.API.Lua
{
    /// <summary>
    /// Lua接口
    /// </summary>
    public interface ILua : IDestroy
    {
        /// <summary>
        /// 获取Lua适配器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetLuaEngine<T>();

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
        /// <param name="callback">加载器回调</param>
        void AddLoader(Func<string, byte[]> callback);

        /// <summary>
        /// 移除加载器
        /// </summary>
        /// <param name="callback">加载器回调</param>
        /// <returns>是否成功</returns>
        bool RemoveLoader(Func<string, byte[]> callback);
    }
}