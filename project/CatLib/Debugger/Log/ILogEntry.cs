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

using CatLib.API.Debugger;
using System;

namespace CatLib.Debugger.Log
{
    /// <summary>
    /// 日志条目
    /// </summary>
    public interface ILogEntry
    {
        /// <summary>
        /// 条目id
        /// </summary>
        long Id { get; }

        /// <summary>
        /// 日志等级
        /// </summary>
        LogLevels Level { get; }

        /// <summary>
        /// 日志内容
        /// </summary>
        string Message { get; }

        /// <summary>
        /// 命名空间
        /// </summary>
        string Namespace { get; }

        /// <summary>
        /// 记录时间
        /// </summary>
        long Time { get; }

        /// <summary>
        /// 获取调用堆栈
        /// </summary>
        /// <param name="assemblyMatch">程序集是否符合输出条件</param>
        /// <returns>调用堆栈</returns>
        string[] GetStackTrace(Predicate<string> assemblyMatch = null);

        /// <summary>
        /// 是否可以被忽略
        /// </summary>
        /// <param name="type">处理器类型</param>
        /// <returns>是否可以忽略这个处理器</returns>
        bool IsIgnore(Type type);
    }
}
