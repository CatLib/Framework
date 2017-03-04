
using CatLib.API.Resources;
using UnityEngine;

namespace CatLib.Resources
{

    /// <summary>
    /// 对象信息
    /// </summary>
    public class ObjectInfo : IObjectInfo
    {

        /// <summary>原对象</summary>
        protected UnityEngine.Object Object {  get; set; }

        public string AssetBundle { get; set; }

        public string Name { get; set; }

        private bool isDestroy = false;
        public bool IsDestroy
        {
            get { return isDestroy; }
            set
            {
                if(isDestroy && !value)
                {
                    protectedNum = 3;
                }
                isDestroy = value;
            }
        }

        private int protectedNum = 3;

        /// <summary>
        /// 对象信息
        /// </summary>
        /// <param name="obj"></param>
        public ObjectInfo(string assetBundleName ,string name , Object obj)
        {
            AssetBundle = assetBundleName;
            Name = name;
            Object = obj;
        }

        /// <summary>
        /// 实例化
        /// </summary>
        public void Instantiate()
        {

        }

        public T Get<T>() where T : Object
        {
            return Object as T;
        }

        /// <summary>
        /// 托管一个对象 
        /// </summary>
        /// <param name="hostedObject"></param>
        public void Hosted(UnityEngine.Object hostedObject)
        {

        }

        public void Check()
        {
            if(protectedNum > 0)
            {
                --protectedNum;
                return;
            }
            //统计弱引用 todo
            IsDestroy = true;
        }

    }

}