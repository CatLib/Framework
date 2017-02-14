using CatLib.Contracts.NetPackage;

namespace CatLib.NetPackge
{
    
    /// <summary>
    /// CatLib 网络数据包
    /// </summary>
    public class CCatLibUnpacking : IUnpacking
    {
        /// <summary>
        /// 将未分包的数据追加入拆包器
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        public bool Append(byte[] bytes, out IPackage[] package)
        {
            UnityEngine.Debug.Log(System.Text.Encoding.Default.GetString(bytes));
            package = null;
            return false;
        }

        /// <summary>
        /// 清空拆包器里的数据
        /// </summary>
        public void Clear()
        {

        }
    }
}
