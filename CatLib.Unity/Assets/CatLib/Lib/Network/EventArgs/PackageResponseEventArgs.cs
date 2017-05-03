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
using CatLib.API.Network;

namespace CatLib.Network
{
    /// <summary>
    /// 响应参数
    /// </summary>
    public class PackageResponseEventArgs : EventArgs, IPackageResponse
    {
        /// <summary>
        /// 响应的数据包
        /// </summary>
        public IPackage Response { get; protected set; }

        /// <summary>
        /// 构建一个响应参数
        /// </summary>
        /// <param name="package">数据包</param>
        public PackageResponseEventArgs(IPackage package)
        {
            Response = package;
        }
    }
}