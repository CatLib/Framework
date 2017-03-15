using UnityEngine;
using CatLib.API;
using CatLib.API.Resources;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace CatLib.Resources
{

    /// <summary>
    /// 资源托管
    /// </summary>
    public class ResourcesHosted : IUpdate , IResourcesHosted
    {

        [Dependency]
        public AssetBundleLoader assetBundleLoader { get; set; }

        /// <summary>
        /// 自检间隔
        /// </summary>
        private const int SELF_CHECK_INTERVAL = 5;

        /// <summary>
        /// 多少时间卸载1个资源包
        /// </summary>
        private const float UNLOAD_INTERVAL = 1;

        /// <summary>
        /// 引用字典（查询）
        /// </summary>
        private Dictionary<string, Dictionary<string, ObjectInfo>> refDict = new Dictionary<string, Dictionary<string, ObjectInfo>>();

        /// <summary>
        /// 遍历用列表（冗余数据）
        /// </summary>
        private List<ObjectInfo> refTraversal = new List<ObjectInfo>();

        /// <summary>
        /// 释放队列
        /// </summary>
        private Queue<ObjectInfo> destroyQueue = new Queue<ObjectInfo>();

        /// <summary>
        /// 检测时钟
        /// </summary>
        private float time = SELF_CHECK_INTERVAL;

        public ResourcesHosted()
        {
            App.Instance.StartCoroutine(UnLoad());
        }

        /// <summary>
        /// 获取一个引用
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IObjectInfo Get(string path)
        {
            var assetPath = Path.GetDirectoryName(path);
            var name = Path.GetFileName(path);
            if (refDict.ContainsKey(assetPath) && refDict[assetPath].ContainsKey(name))
            {
                ObjectInfo info = refDict[assetPath][name];
                info.IsDestroy = false; 
                return info;
            }
            return null;
        }

        /// <summary>
        /// 托管内容
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="obj">生成的对象</param>
        public IObjectInfo Hosted(string path, Object obj)
        {
            if (obj == null) { return null; }

            var assetPath = Path.GetDirectoryName(path);
            var name      = Path.GetFileName(path);

            var info = new ObjectInfo(assetPath, name, obj);

            if (!refDict.ContainsKey(assetPath))
            {
                refDict.Add(assetPath, new Dictionary<string, ObjectInfo>());
            }
            refDict[assetPath].Add(name, info);
            refTraversal.Add(info);

            return info;
        }

        /// <summary>
        /// 每帧更新
        /// </summary>
        public void Update()
        {
            if((time -= App.Instance.Time.DeltaTime) <= 0)
            {
                for (int i = 0; i < refTraversal.Count; i++)
                {
                    if (!refTraversal[i].IsDestroy)
                    {
                        refTraversal[i].Check();
                        if (refTraversal[i].IsDestroy)
                        {
                            //防止处理卸载来不及不停的加入队列
                            if (!destroyQueue.Contains(refTraversal[i]))
                            {
                                destroyQueue.Enqueue(refTraversal[i]);
                            }
                        }
                    }
                }
                time += SELF_CHECK_INTERVAL;
            }
        }


        public IEnumerator UnLoad()
        {

            ObjectInfo info = null;
            Dictionary<string, ObjectInfo> tmpDict = null;
            bool needDestroy = false;

            while (true) {

                if (destroyQueue.Count <= 0)
                {
                    yield return new WaitForEndOfFrame();
                    continue;
                }
     
                info = destroyQueue.Dequeue();
                if (!info.IsDestroy)
                {
                    yield return new WaitForEndOfFrame();
                    continue;
                }

                needDestroy = true;
                if (refDict.ContainsKey(info.AssetBundle))
                {

                    tmpDict = refDict[info.AssetBundle];
                    foreach (var val in tmpDict.Values)
                    {
                        if (!val.IsDestroy) { needDestroy = false; }
                    }
                    if (needDestroy)
                    {
                        bool isSuccess = assetBundleLoader.UnloadAssetBundle(info.AssetBundle);
                        if (isSuccess)
                        {
                            foreach (var val in tmpDict.Values)
                            {
                                refTraversal.Remove(val);
                            }
                            refDict.Remove(info.AssetBundle);
                            yield return new WaitForSeconds(UNLOAD_INTERVAL);
                        }else
                        {
                            //如果释放失败则重新丢入队尾
                            destroyQueue.Enqueue(info);
                        }
                    }

                }

                yield return new WaitForEndOfFrame();

            }
        }

    }
}