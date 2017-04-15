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
 
namespace CatLib.API.Network
{
    /// <summary>
    /// 数据包
    /// </summary>
    public interface IPackage
    {

        /// <summary>
        /// 数据包
        /// </summary>
        object Package { get; }

        /// <summary>
        /// 数据包字节流
        /// </summary>
        byte[] ToByte();
    
    }
}
