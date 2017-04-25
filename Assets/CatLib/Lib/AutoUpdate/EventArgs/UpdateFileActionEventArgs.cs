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
#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#elif UNITY_5_2 || UNITY_5_3 
using UnityEngine.Experimental.Networking;
#endif

namespace CatLib.AutoUpdate
{
    /// <summary>
    /// 自动更新文件更新事件
    /// </summary>
    public sealed class UpdateFileActionEventArgs : EventArgs
    {
        /// <summary>
        /// 文件请求对象
        /// </summary>
        public UnityWebRequest Request { get; protected set; }

        /// <summary>
        /// 文件更新路径
        /// </summary>
        public string FilePath { get; protected set; }

        /// <summary>
        /// 创建一个自动更新文件更新事件
        /// </summary>
        /// <param name="filePath">文件更新路径</param>
        /// <param name="request">文件请求对象</param>
        public UpdateFileActionEventArgs(string filePath, UnityWebRequest request)
        {
            Request = request;
            FilePath = filePath;
        }
    }
}