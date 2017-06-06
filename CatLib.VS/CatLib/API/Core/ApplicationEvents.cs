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

namespace CatLib.API
{
    /// <summary>
    /// 应用程序事件
    /// </summary>
    public sealed class ApplicationEvents
    {
        /// <summary>
        /// 当初始化时
        /// </summary>
        public static readonly string OnIniting = "application.initing";

        /// <summary>
        /// 当初始化结束
        /// </summary>
        public static readonly string OnInited = "application.inited";

        /// <summary>
        /// 当程序启动完成
        /// </summary>
        public static readonly string OnStartComplete = "application.complete";
    }
}