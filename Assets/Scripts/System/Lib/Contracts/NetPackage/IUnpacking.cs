
namespace CatLib.Contracts.NetPackage
{

    /// <summary>
    /// 拆包器
    /// </summary>
    public interface IUnpacking
    {

        /// <summary>
        /// 将未分包的数据追加入拆包器
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        bool Append(byte[] bytes , out IPackage[] package);

        /// <summary>
        /// 清空拆包器里的数据
        /// </summary>
        void Clear();
        
    }

}