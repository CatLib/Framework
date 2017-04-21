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

namespace CatLib.AutoUpdate
{
    /// <summary>
    /// 文件启动更新前事件
    /// </summary>
    public sealed class UpdateFileStartEventArgs : EventArgs
    {
        /// <summary>
        /// 需要更新的文件列表
        /// </summary>
        public string[] UpdateList { get; private set; }

        /// <summary>
        /// 创建一个文件启动更新事件
        /// </summary>
        /// <param name="list"></param>
        public UpdateFileStartEventArgs(string[] list)
        {
            UpdateList = list;
        }
    }
}