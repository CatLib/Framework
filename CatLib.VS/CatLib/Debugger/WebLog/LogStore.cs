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

using System;
using CatLib.API.Debugger;
using CatLib.Debugger.Log;
using CatLib.Debugger.WebLog.LogHandler;
using CatLib.Stl;
using System.Collections.Generic;

namespace CatLib.Debugger.WebLog
{
    /// <summary>
    /// Web调试服务
    /// </summary>
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
        private readonly SortSet<ILogEntry, long> logEntrys;

        /// <summary>
        /// 最大储存的日志记录数
        /// </summary>
        private readonly int maxLogEntrys = 1024;

        /// <summary>
        /// 当前唯一标识符
        /// </summary>
        private readonly string guid;

        /// <summary>
        /// 当前唯一标识符
        /// </summary>
        public string Guid
        {
            get { return guid; }
        }

        /// <summary>
        /// 构造一个Web调试服务
        /// </summary>
        public LogStore([Inject(Required = true)]Logger logger)
        {
            logger.AddLogHandler(new WebLogHandler(this));
            categroy = new Dictionary<string, string>();
            logEntrys = new SortSet<ILogEntry, long>();
            logEntrys.ReverseIterator();
            guid = System.Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 记录一个日志
        /// </summary>
        /// <param name="entry">日志条目</param>
        internal void Log(ILogEntry entry)
        {
            while (logEntrys.Count >= maxLogEntrys)
            {
                logEntrys.Shift();
            }
            logEntrys.Add(entry, entry.Id);
        }

        /// <summary>
        /// 定义命名空间对应的分类
        /// </summary>
        /// <param name="namespaces">该命名空间下的输出的调试语句将会被归属当前定义的组</param>
        /// <param name="categroyName">分类名(用于在调试控制器显示)</param>
        public void DefinedCategory(string namespaces, string categroyName)
        {
            categroy[namespaces] = categroyName;
        }

        /// <summary>
        /// 获取在LastId之后的日志条目实体
        /// </summary>
        /// <param name="lastId">最后的Id</param>
        /// <returns>日志条目</returns>
        public IList<ILogEntry> GetAllEntrysAfterLastId(long lastId)
        {
            return logEntrys.GetElementRangeByScore(lastId + 1, long.MaxValue);
        }
    }
}
