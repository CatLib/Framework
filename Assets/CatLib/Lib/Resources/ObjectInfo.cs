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
using System.Collections.Generic;
using Object = UnityEngine.Object;
using UnityEngine;
using CatLib.API.Resources;

namespace CatLib.Resources
{
    /// <summary>
    /// 对象信息
    /// </summary>
    public sealed class ObjectInfo : IObject
    {
        /// <summary>
        /// 原始对象，注意这个访问将不会引用计数
        /// </summary>
        private Object Object { get; set; }

        /// <summary>
        /// 非托管获取(除非你对引用计数系统非常了解否则不要使用这个函数)
        /// </summary>
        /// <returns>原始对象</returns>
        public Object Original
        {
            get { return Object; }
        }

        /// <summary>
        /// AssetBundle路径
        /// </summary>
        public string AssetBundle { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否处于释放中
        /// </summary>
        private bool isDestroy;

        /// <summary>
        /// 是否处于释放中
        /// </summary>
        public bool IsDestroy
        {
            get { return isDestroy; }
            set
            {
                if (isDestroy && !value)
                {
                    protectedNum = 1;
                }
                isDestroy = value;
            }
        }

        /// <summary>
        /// 保护计数
        /// </summary>
        private int protectedNum = 1;

        /// <summary>
        /// 弱引用
        /// </summary>
        private readonly List<WeakReference> references = new List<WeakReference>();

        /// <summary>
        /// 对象信息
        /// </summary>
        /// <param name="name">资源名</param>
        /// <param name="obj">资源原始对象</param>
        /// <param name="assetBundleName">资源路径</param>
        public ObjectInfo(string assetBundleName, string name, Object obj)
        {
            AssetBundle = assetBundleName;
            Name = name;
            Object = obj;
        }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <returns>GameObject</returns>
        public GameObject Instantiate()
        {
            if (Object == null)
            {
                return null;
            }
            if (!(Object is GameObject))
            {
                return null;
            }
            var prefab = (GameObject)Object;
            Object obj = Object.Instantiate(prefab);
            obj.name = prefab.name;
            Hosted(obj);
            return (GameObject)obj;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T">转换的类型</typeparam>
        /// <param name="hostedObject">宿主</param>
        /// <returns>获取对象</returns>
        public T Get<T>(object hostedObject) where T : Object
        {
            Hosted(hostedObject);
            return Object as T;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="hostedObject">宿主</param>
        /// <returns>获取对象</returns>
        public Object Get(object hostedObject)
        {
            Hosted(hostedObject);
            return Object;
        }

        /// <summary>
        /// 托管一个对象
        /// </summary>
        /// <param name="hostedObject">宿主</param>
        private void Hosted(object hostedObject)
        {
            if (hostedObject == null)
            {
                throw new Exception("please set the hosted object!");
            }

            for (var i = 0; i < references.Count; ++i)
            {
                if (hostedObject.Equals(references[i].Target))
                {
                    return;
                }
            }
            var wr = new WeakReference(hostedObject);
            references.Add(wr);
            IsDestroy = false;
        }

        /// <summary>
        /// 检查资源
        /// </summary>
        public void Check()
        {
            if (IsDestroy)
            {
                return;
            }
            if (protectedNum > 0)
            {
                --protectedNum;
                return;
            }
            object o;
            for (var i = references.Count - 1; i >= 0; --i)
            {
                o = references[i].Target;
                if (o == null)
                {
                    references.RemoveAt(i);
                }
            }
            if (references.Count > 0)
            {
                return;
            }
            IsDestroy = true;
            protectedNum = 1;
        }
    }
}