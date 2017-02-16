
namespace CatLib.Contracts.NetPackage
{

    /// <summary>
    /// 拆封包器
    /// </summary>
    public interface IPacking
    {

        /// <summary>
        /// 将未分包的数据追加入拆封包器
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>一个数据包的偏移量</returns>
        int Input(byte[] bytes);

        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        IPackage Decode(byte[] bytes);

        /// <summary>
        /// 封包
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        byte[] Encode(IPackage bytes);
        
    }

}