using System.Collections.Generic;
using System;

namespace CatLib.Container
{

    /// <summary>
    /// 编译策略链
    /// </summary>
    public class BuildStrategyChain<TBuildStages>
    {

        /// <summary>
        /// 编译策略
        /// </summary>
        private readonly List<IBuilderStrategy>[] stages;

        /// <summary>
        /// 锁定对象
        /// </summary>
        private readonly object lockObject = new object();

        /// <summary>
        /// 构建一个编译策略链
        /// </summary>
        public BuildStrategyChain()
        {
            stages = new List<IBuilderStrategy>[NumberOfEnumValues()];

            for (int i = 0; i < stages.Length; ++i)
            {
                stages[i] = new List<IBuilderStrategy>();
            }
        }

        /// <summary>
        /// 增加一个编译策略
        /// </summary>
        /// <param name="strategy"></param>
        /// <param name="stage"></param>
        public void Add(IBuilderStrategy strategy, TBuildStages stage)
        {
            lock (lockObject)
            {
                stages[Convert.ToInt32(stage)].Add(strategy);
            }
        }

        /// <summary>
        /// 增加一个编译策略
        /// </summary>
        /// <typeparam name="TStrategy"></typeparam>
        /// <param name="stage"></param>
        public void Add<TStrategy>(TBuildStages stage) where TStrategy : IBuilderStrategy, new()
        {
            Add(new TStrategy(), stage);
        }

        /// <summary>
        /// 清空编译策略
        /// </summary>
        public void Clear()
        {
            lock (lockObject)
            {
                foreach (List<IBuilderStrategy> stage in stages)
                {
                    stage.Clear();
                }
            }
        }

        /// <summary>
        /// 获取步骤数量
        /// </summary>
        /// <returns></returns>
        private static int NumberOfEnumValues()
        {
            return Enum.GetValues(typeof(TBuildStages)).Length;
        }

    }

}
