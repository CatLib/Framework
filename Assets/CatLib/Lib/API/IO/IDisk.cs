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
    /// 文件驱动
    /// </summary>
    public interface IDisk
    {

        /// <summary>
        /// 获取一个文件操作对象
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="type">路径类型 默认为相对路径</param>
        /// <returns></returns>
        IFile File(string path, PathTypes type = PathTypes.Relative);

        /// <summary>
        /// 获取一个目录操作对象
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <param name="type">路径类型 默认为相对路径</param>
        /// <returns></returns>
        IDirectory Directory(string path, PathTypes type = PathTypes.Relative);

        /// <summary>
        /// 文件驱动根目录
        /// </summary>
        IDirectory Root { get; }

        /// <summary>
        /// 是否加密
        /// </summary>
        bool IsCrypt { get; }

        /// <summary>
        /// 配置文件驱动
        /// </summary>
        /// <param name="config">配置表</param>
        void SetConfig(Hashtable config);

    }
}