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
        public static readonly string ON_INITING = "application.initing";

        /// <summary>
        /// 当初始化结束
        /// </summary>
        public static readonly string ON_INITED = "application.inited";

        /// <summary>
        /// 当服务提供商流程开始前
        /// </summary>
        public static readonly string ON_PROVIDER_PROCESSING = "application.provider.processing";

        /// <summary>
        /// 当服务提供商流程结束
        /// </summary>
        public static readonly string ON_PROVIDER_PROCESSED = "application.provider.processed";

        /// <summary>
        /// 当程序启动完成
        /// </summary>
        public static readonly string ON_APPLICATION_START_COMPLETE = "application.start.complete";
    }
}