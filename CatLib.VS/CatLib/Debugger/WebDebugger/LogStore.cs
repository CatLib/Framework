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

using System.Collections.Generic;
using CatLib.API.Debugger;
using CatLib.API.Routing;

namespace CatLib.Debugger.WebDebugger
{
    /// <summary>
    /// Web调试服务
    /// </summary>
    [Routed]
    public sealed class LogStore : ILogWebCategory
    {
        /// <summary>
        /// 分组信息
        /// </summary>
        private readonly IDictionary<string, string> categroy;

        /// <summary>
        /// 分组信息
        /// </summary>
        internal IDictionary<string, string> Categroy
        {
            get { return categroy; }
        }

        /// <summary>
        /// 日志记录
        /// </summary>
        private readonly Queue<ILogEntry> logEntrys;

        /// <summary>
        /// 最大储存的日志记录数
        /// </summary>
        private readonly int maxLogEntrys = 1024;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// 构造一个Web调试服务
        /// </summary>
        public LogStore()
        {
            categroy = new Dictionary<string, string>();
            logEntrys = new Queue<ILogEntry>(maxLogEntrys);
        }

        /// <summary>
        /// 记录一个日志
        /// </summary>
        /// <param name="entry">日志条目</param>
        public void Log(ILogEntry entry)
        {
            lock (syncRoot)
            {
                while (logEntrys.Count >= maxLogEntrys)
                {
                    logEntrys.Dequeue();
                }
                logEntrys.Enqueue(entry);
            }
        }

        /// <summary>
        /// 定义命名空间对应的分类
        /// </summary>
        /// <param name="namespaces">该命名空间下的输出的调试语句将会被归属当前定义的组</param>
        /// <param name="categroyName">分类名(用于在调试控制器显示)</param>
        public void DefinedCategory(string namespaces, string categroyName)
        {
            lock (syncRoot)
            {
                categroy[namespaces] = categroyName;
            }
        }

        /// <summary>
        /// 获取在LastId之后的日志条目实体
        /// </summary>
        /// <param name="lastId">最后的Id</param>
        /// <returns>日志条目</returns>
        public IList<ILogEntry> GetAllEntrysAfterLastId(long lastId)
        {
            lock (syncRoot)
            {
                var result = new List<ILogEntry>();
                foreach (var entry in logEntrys)
                {
                    if (entry.Id < lastId)
                    {
                        break;
                    }
                    result.Add(entry);
                }
                return result;
            }
        }
    }
}
