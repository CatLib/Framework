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
    public class ObjectInfo : IObject
    {

        /// <summary>原对象</summary>
        protected Object Object {  get; set; }

        /// <summary>
        /// AssetBundle信息
        /// </summary>
        public string AssetBundle { get; set; }

        /// <summary>
        /// 对象名字
        /// </summary>
        public string Name { get; set; }

        private bool isDestroying = false;

        /// <summary>
        /// 是否正在被释放的
        /// </summary>
        public bool IsDestroying
        {
            get { return isDestroying; }
            set
            {
                if(isDestroying && !value)
                {
                    protectedNum = 1;
                }
                isDestroying = value;
            }
        }

        /// <summary>
        /// 保护计数
        /// </summary>
        private int protectedNum = 1;

        /// <summary>
        /// 引用计数
        /// </summary>
        private int refCount = 0;

        /// <summary>
        /// 引用计数
        /// </summary>
        private List<WeakReference> references = new List<WeakReference>();

        /// <summary>
        /// 对象信息
        /// </summary>
        /// <param name="obj"></param>
        public ObjectInfo(string assetBundleName ,string name , Object obj)
        {
            AssetBundle = assetBundleName;
            Name = name;
            Object = obj;
            refCount = 0;
        }

        /// <summary>
        /// 实例化
        /// </summary>
        public GameObject Instantiate()
        {
            if (Object != null)
            {
                if (Object is GameObject)
                {
                    GameObject prefab = Object as GameObject;
                    Object obj = Object.Instantiate(prefab);
                    obj.name = prefab.name;
                    Hosted(obj);
                    return (GameObject)obj;
                }
            }
            return null;
        }

        /// <summary>
        /// 非托管获取(除非你对引用计数系统非常了解否则不要使用这个函数)
        /// </summary>
        /// <returns></returns>
        public Object UnHostedGet()
        {
            return Object;
        }

        /// <summary>
        /// 托管对象后换取Object
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="hostedObject"></param>
        /// <returns></returns>
        public T Get<T>(object hostedObject) where T : Object
        {
            Hosted(hostedObject);
            return Object as T;
        }

        /// <summary>
        /// 托管对象后换取Object
        /// </summary>
        /// <param name="hostedObject"></param>
        /// <returns></returns>
        public Object Get(object hostedObject)
        {
            Hosted(hostedObject);
            return Object;
        }

        /// <summary>
        /// 引用计数强制加1
        /// </summary>
        public void Retain()
        {
            refCount++;
        }

        /// <summary>
        /// 引用计数强制减1
        /// </summary>
        public void Release()
        {
            refCount--;
            refCount = Math.Min(refCount, 0);
        }

        /// <summary>
        /// 托管一个对象
        /// </summary>
        /// <param name="hostedObject"></param>
        protected void Hosted(object hostedObject)
        {
            if (hostedObject == null)
            {
                throw new Exception("please set the hosted object!");
            }

            for (int i = 0; i < references.Count; ++i)
            {
                if (hostedObject.Equals(references[i].Target))
                {
                    return;
                }
            }
            WeakReference wr = new WeakReference(hostedObject);
            references.Add(wr);
            refCount++;
            IsDestroying = false;
        }

        public void Check()
        {
            if (IsDestroying) { return; }

            if(protectedNum > 0)
            {
                --protectedNum;
                return;
            }
            object o;
            for (int i = references.Count - 1; i >= 0; --i)
            {
                o = references[i].Target;
                if (o == null)
                {
                    references.RemoveAt(i);
                    refCount--;
                }
            }
            if (refCount <= 0)
            {
                IsDestroying = true;
                protectedNum = 1;
            }
        }

    }

}