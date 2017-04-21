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

using UnityEngine;
using CatLib.API.Flux;

namespace CatLib.Flux
{
    /// <summary>
    /// 视图
    /// </summary>
    public abstract class View : MonoBehaviour
    {
        /// <summary>
        /// 关注的容器列表
        /// </summary>
        /// <returns>关注的容器列表</returns>
        protected virtual IStore[] Observer
        {
            get
            {
                return new IStore[] { };
            }
        }

        /// <summary>
        /// 当起始时调用
        /// </summary>
        public void Start()
        {
            for (var i = 0; i < Observer.Length; i++)
            {
                Observer[i].AddListener(OnChange);
            }
        }

        /// <summary>
        /// 当释放时调用
        /// </summary>
        public void OnDestroy()
        {
            for (var i = 0; i < Observer.Length; i++)
            {
                Observer[i].RemoveListener(OnChange);
            }
        }

        /// <summary>
        /// 当存储发生变更时
        /// </summary>
        /// <param name="action">行为</param>
        protected virtual void OnChange(IAction action) { }
    }
}
