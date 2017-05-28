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

using CatLib.API;
using CatLib.API.Time;
using CatLib.API.Timer;
using CatLib.Stl;

namespace CatLib.Timer
{
    /// <summary>
    /// 计时器管理器
    /// </summary>
    public sealed class TimerManager : IUpdate
    {
        /// <summary>
        /// 时间管理器
        /// </summary>
        private readonly ITimeManager timeManager;

        /// <summary>
        /// 运行列表
        /// </summary>
        private readonly SortSet<TimerGroup, int> executeList;

        /// <summary>
        /// 构建一个计时器管理器
        /// </summary>
        /// <param name="timeManager">时间管理器</param>
        public TimerManager([Inject(Required = true)]ITimeManager timeManager)
        {
            this.timeManager = timeManager;
            executeList = new SortSet<TimerGroup, int>();
        }

        /// <summary>
        /// 创建一个计时器
        /// </summary>
        /// <returns></returns>
        public ITimer Create()
        {
            return new Timer(this, timeManager.Default);
        }

        /// <summary>
        /// 每帧更新
        /// </summary>
        public void Update()
        {
            
        }
    }
}
