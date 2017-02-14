using CatLib.Contracts.NetPackage;
using System.Collections.Generic;

namespace CatLib.NetPackge
{
    
    /// <summary>
    /// CatLib 网络数据包
    /// </summary>
    public class CCatLibUnpacking : IUnpacking
    {

        private Queue<byte> dataQueue = new Queue<byte>();

        /// <summary>
        /// 将未分包的数据追加入拆包器
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        public bool Append(byte[] bytes, out IPackage[] package)
        {
            for(int i = 0; i < bytes.Length; i++)
            {
                dataQueue.Enqueue(bytes[i]);
            }

            
            

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
