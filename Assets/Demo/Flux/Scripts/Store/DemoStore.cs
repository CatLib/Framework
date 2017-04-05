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

using CatLib.Flux;
using CatLib.API.Flux;

namespace CatLib.Demo.Flux
{

    public class DemoStore : Store
    {

        public const string ADD = "add";

        #region instance

        private static DemoStore instance;

        public static DemoStore Get(IFluxDispatcher dispatcher)
        {
            if (instance == null)
            {
                UnityEngine.Debug.Log("create");
                instance = new DemoStore(dispatcher);
            }
            return instance;
        }

        #endregion

        private int count;

        /// <summary>
        /// 测试用的存储
        /// </summary>
        /// <param name="dispatcher"></param>
        public DemoStore(IFluxDispatcher dispatcher)
            : base(dispatcher)
        {
            count = 0;
        }

        /// <summary>
        /// 当存储接受到调度时
        /// </summary>
        /// <param name="action"></param>
        protected override void OnDispatch(IAction action)
        {
            switch (action.Action)
            {
                case ADD:
                    Add();
                    Change(); // 表明这个存储已经发生变化
                    break;
            }
        }

        /// <summary>
        /// 获取计数
        /// </summary>
        /// <returns></returns>
        public int GetCount() { return count; }

        /// <summary>
        /// 计数加1
        /// </summary>
        protected void Add() { count++; }

    }

}