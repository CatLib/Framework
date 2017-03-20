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
    /// 协议解析器
    /// </summary>
	public interface IProtocol{


		/// <summary>
        /// 协议反序列化
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        IPackage Decode(byte[] bytes);

		/// <summary>
        /// 协议序列化
        /// </summary>
        /// <param name="package">协议包</param>
        /// <returns></returns>
        byte[] Encode(IPackage package);


	}

}