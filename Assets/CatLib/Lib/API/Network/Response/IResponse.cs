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
    /// 接口
    /// </summary>
    public interface IResponse
    {

        /// <summary>
        /// 响应字节流
        /// </summary>
        byte[] Response { get; }

    }

}