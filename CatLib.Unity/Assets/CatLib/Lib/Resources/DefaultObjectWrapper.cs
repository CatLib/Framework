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

using CatLib.API.Resources;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CatLib.Resources
{
    /// <summary>
    /// 默认包装对象
    /// </summary>
    public sealed class DefaultObjectWrapper : IObject
    {
        /// <summary>
        /// 原始对象
        /// </summary>
        private readonly Object target;

        /// <summary>
        /// 原始对象，注意这个访问将不会引用计数
        /// </summary>
        public Object Original
        {
            get { return target; }
        }

        /// <summary>
        /// 包装一个原始对象
        /// </summary>
        /// <param name="target">原始对象</param>
        public DefaultObjectWrapper(Object target)
        {
            this.target = target;
        }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <returns>GameObject</returns>
        public GameObject Instantiate()
        {
            if (target == null)
            {
                return null;
            }
            if (!(target is GameObject))
            {
                return null;
            }
            var prefab = (GameObject)target;
            Object ins = Object.Instantiate(prefab);
            ins.name = prefab.name;
            return (GameObject)ins;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T">转换的类型</typeparam>
        /// <param name="hostedObject">宿主</param>
        /// <returns>获取对象</returns>
        public T Get<T>(object hostedObject) where T : Object
        {
            return target as T;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="hostedObject">宿主</param>
        /// <returns>获取对象</returns>
        public Object Get(object hostedObject)
        {
            return target;
        }
    }
}