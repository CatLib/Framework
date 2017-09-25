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

namespace CatLib.API.Debugger
{
    /// <summary>
    /// 日志等级
    /// 细节见：https://tools.ietf.org/html/rfc5424
    /// </summary>
    public enum LogLevels
    {
        /// <summary>
        /// 紧急(系统不可用)
        /// </summary>
        Emergency = 0,

        /// <summary>
        /// 警报(必须立即采取行动)
        /// </summary>
        Alert = 1,

        /// <summary>
        /// 关键（关键日志）
        /// </summary>
        Critical = 2,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 3,

        /// <summary>
        /// 警告
        /// </summary>
        Warning = 4,

        /// <summary>
        /// 通知
        /// </summary>
        Notice = 5,

        /// <summary>
        /// 信息
        /// </summary>
        Info = 6,

        /// <summary>
        /// 调试级消息
        /// </summary>
        Debug = 7
    }
}
