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

using System.Collections;

namespace CatLib.API.IO
{
    /// <summary>
    /// 驱动器
    /// </summary>
    public interface IDisk
    {
        /// <summary>
        /// 获取磁盘中的一个文件
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <param name="pathType">路径类型</param>
        /// <returns>文件</returns>
        IFile File(string path, PathTypes type = PathTypes.Relative);

        /// <summary>
        /// 获取磁盘中的文件夹
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <param name="type">路径类型</param>
        /// <returns>文件夹</returns>
        IDirectory Directory(string path, PathTypes type = PathTypes.Relative);

        /// <summary>
        /// 获取磁盘中的文件夹
        /// </summary>
        /// <returns>文件夹</returns>
        IDirectory Directory();

        /// <summary>
        /// 获取默认根路径
        /// </summary>
        IDirectory Root { get; }

        /// <summary>
        /// 磁盘是否是被加密的
        /// </summary>
        bool IsCrypt { get; }

        /// <summary>
        /// 设定磁盘配置
        /// </summary>
        /// <param name="config">配置信息</param>
        void SetConfig(Hashtable config);
    }
}