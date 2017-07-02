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
    /// 调试器
    /// </summary>
    public interface IDebugger : ILogger
    {
        /// <summary>
        /// 定义命名空间对应的分类
        /// </summary>
        /// <param name="namespaces">该命名空间下的输出的调试语句将会被归属当前定义的组</param>
        /// <param name="categroyName">分类名(用于在调试控制器显示)</param>
        void DefinedCategory(string namespaces, string categroyName);

        /// <summary>
        /// 定义监控实行方案
        /// </summary>
        /// <param name="moitorName">监控名</param>
        /// <param name="handler">执行句柄</param>
        /// <param name="sort">排序</param>
        void DefinedMonitor(string moitorName, IMonitor handler , int sort = int.MaxValue);

        /// <summary>
        /// 增加日志记录器
        /// </summary>
        /// <param name="logger">记录器</param>
        void AddLogHandler(ILogHandler logger);

        /// <summary>
        /// 监控一个内容
        /// </summary>
        /// <param name="monitorName">监控名</param>
        /// <param name="value">监控值</param>
        void Monitor(string monitorName, object value);
    }
}
