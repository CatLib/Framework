
namespace CatLib.API.Network
{

    /// <summary>
    /// 拆封包器
    /// </summary>
    public interface IPacking
    {

        /// <summary>
        /// 拆包
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        byte[][] Decode(byte[] bytes);

        /// <summary>
        /// 封包
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        byte[] Encode(byte[] bytes);

        /// <summary>
        /// 清空缓存数据
        /// </summary>
        void Clear();
        
    }

}