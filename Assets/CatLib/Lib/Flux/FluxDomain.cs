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

namespace CatLib.Flux
{

    /// <summary>
    /// 域
    /// </summary>
    public class FluxDomain
    {

        /// <summary>
        /// 存储地图
        /// </summary>
        protected IDictionary<string, IStore>  storeMap = new Dictionary<string, IStore>();

        /// <summary>
        /// 视图地图
        /// </summary>
        protected IDictionary<string, IView> viewMap = new Dictionary<string, IView>();

        /// <summary>
        /// 调度器
        /// </summary>
        protected IFluxDispatcher dispatcher;

        /// <summary>
        /// 注册一个存储块
        /// </summary>
        /// <param name="store"></param>
        public virtual void RegisterStore(IStore store)
        {



        }

    }

}