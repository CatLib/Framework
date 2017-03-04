
using UnityEngine;

namespace CatLib.API.Resources
{
    public interface IResourcesHosted
    {

        /// <summary>
        /// 从托管系统中获取一个托管
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <returns></returns>
        IObjectInfo Get(string path);

        /// <summary>
        /// 托管内容一个内容
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="obj">托管对象</param>
        IObjectInfo Hosted(string path, Object obj);

    }

}