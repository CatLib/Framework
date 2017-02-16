using CatLib.Contracts.NetPackage;
using System.Collections.Generic;

namespace CatLib.NetPackge
{
    
    /// <summary>
    /// CatLib 网络拆包器
    /// </summary>
    public class CCatLibPacking : IPacking
    {
        /// <summary>
        /// 将未分包的数据追加入拆封包器
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>一个数据包的偏移量</returns>
        public int Input(byte[] bytes){

            return 0;

        }

        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public IPackage Decode(byte[] bytes){

            return null;

        }

        /// <summary>
        /// 封包
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public byte[] Encode(IPackage bytes){

            return null;

        }

    }
}
