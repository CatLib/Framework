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

namespace CatLib.API.Stl
{
    /// <summary>
    /// 有序集
    /// </summary>
    /// <typeparam name="TElement">元素</typeparam>
    /// <typeparam name="TScore">分数</typeparam>
    public interface ISortSet<TElement, TScore> : IEnumerable<TElement>
    {
        /// <summary>
        /// 有序集的基数
        /// </summary>
        long Count { get; }

        /// <summary>
        /// 同步锁
        /// </summary>
        object SyncRoot { get; }

        /// <summary>
        /// 反向迭代
        /// </summary>
        /// <returns>元素</returns>
        IEnumerator<TElement> GetReversEnumerator();

        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="element">元素</param>
        /// <param name="score">分数</param>
        void Add(TElement element, TScore score);

        /// <summary>
        /// 是否包含某个元素
        /// </summary>
        /// <param name="element">元素</param>
        bool Contains(TElement element);

        /// <summary>
        /// 返回有序集的分数
        /// </summary>
        /// <param name="element">元素</param>
        /// <returns>分数，如果元素不存在则返回<c>default(TScore)</c></returns>
        TScore GetScore(TElement element);

        /// <summary>
        /// 获取分数范围内的元素个数
        /// </summary>
        /// <param name="start">起始值(包含)</param>
        /// <param name="end">结束值(包含)</param>
        /// <returns>分数值在<paramref name="start"/>(包含)和<paramref name="end"/>(包含)之间的元素数量</returns>
        long GetRangeCount(TScore start, TScore end);

        /// <summary>
        /// 从有序集中删除元素，如果元素不存在返回false
        /// </summary>
        /// <param name="element">元素</param>
        /// <returns>是否成功</returns>
        bool Remove(TElement element);

        /// <summary>
        /// 根据排名区间移除区间内的元素
        /// </summary>
        /// <param name="startRank">开始的排名(包含),排名以0为底</param>
        /// <param name="stopRank">结束的排名(包含),排名以0为底</param>
        /// <returns>被删除的元素个数</returns>
        long RemoveRangeByRank(long startRank, long stopRank);

        /// <summary>
        /// 根据分数区间移除区间内的元素
        /// </summary>
        /// <param name="startScore">开始的分数（包含）</param>
        /// <param name="stopScore">结束的分数（包含）</param>
        /// <returns>被删除的元素个数</returns>
        long RemoveRangeByScore(TScore startScore, TScore stopScore);

        /// <summary>
        /// 获取排名 , 有序集成员按照Score从小到大排序
        /// </summary>
        /// <param name="element">元素</param>
        /// <returns>排名排名以0为底，为-1则表示没有找到元素</returns>
        long GetRank(TElement element);

        /// <summary>
        /// 获取排名，有序集成员按照Score从大到小排序
        /// </summary>
        /// <param name="element"></param>
        /// <returns>排名排名以0为底 , 为-1则表示没有找到元素</returns>
        long GetRevRank(TElement element);

        /// <summary>
        /// 根据排名区间获取区间内的所有元素
        /// </summary>
        /// <param name="startRank">开始的排名(包含),排名以0为底</param>
        /// <param name="stopRank">结束的排名(包含),排名以0为底</param>
        /// <returns>元素列表</returns>
        TElement[] GetElementRangeByRank(long startRank, long stopRank);

        /// <summary>
        /// 根据分数区间获取区间内的所有元素
        /// </summary>
        /// <param name="startScore">开始的分数（包含）</param>
        /// <param name="stopScore">结束的分数（包含）</param>
        /// <returns>元素列表</returns>
        TElement[] GetElementRangeByScore(TScore startScore, TScore stopScore);

        /// <summary>
        /// 根据排名获取元素 (有序集成员按照Score从小到大排序)
        /// </summary>
        /// <param name="rank">排名,排名以0为底</param>
        /// <returns>元素</returns>
        TElement GetElementByRank(long rank);

        /// <summary>
        /// 根据排名获取元素 (有序集成员按照Score从大到小排序)
        /// </summary>
        /// <param name="rank">排名,排名以0为底</param>
        /// <returns>元素</returns>
        TElement GetElementByRevRank(long rank);
    }
}

