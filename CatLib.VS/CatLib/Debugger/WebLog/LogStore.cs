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

using CatLib.Debugger.Log;
using CatLib.Debugger.WebLog.LogHandler;
using System;
using System.Collections.Generic;

namespace CatLib.Debugger.WebLog
{
    /// <summary>
    /// Web调试服务
    /// </summary>
    public sealed class LogStore
    {
        /// <summary>
        /// 对应不同web客户端的已经读取到的最后的日志id
        /// </summary>
        private readonly Dictionary<string, long> clientIds;

        /// <summary>
        /// 日志记录
        /// </summary>
        private readonly SortSet<ILogEntry, long> logEntrys;

        /// <summary>
        /// 最大储存的日志记录数
        /// </summary>
        private int maxLogEntrys = 1024;

        /// <summary>
        /// 构造一个Web调试服务
        /// </summary>
        public LogStore([Inject(Required = true)]Logger logger)
        {
            Guard.Requires<ArgumentNullException>(logger != null);
            logger.AddLogHandler(new WebLogHandler(this));
            clientIds = new Dictionary<string, long>();
            logEntrys = new SortSet<ILogEntry, long>();
            logEntrys.ReverseIterator();
        }

        /// <summary>
        /// 设定最大存储
        /// </summary>
        /// <param name="num"></param>
        public void SetMaxStore(int num)
        {
            Guard.Requires<ArgumentOutOfRangeException>(num >= 1);
            maxLogEntrys = num;
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
        /// 根据客户端id获取未被加载过的日志数据
        /// </summary>
        /// <param name="clientId">客户端id</param>
        /// <returns>未被加载过的日志数据</returns>
        public IList<ILogEntry> GetUnloadEntrysByClientId(string clientId)
        {
            long lastId;
            clientIds.TryGetValue(clientId, out lastId);

            var results = logEntrys.GetElementRangeByScore(lastId + 1, long.MaxValue);
            if (results.Length > 0)
            {
                clientIds[clientId] = results[results.Length - 1].Id;
            }
            return results;
        }
    }
}
