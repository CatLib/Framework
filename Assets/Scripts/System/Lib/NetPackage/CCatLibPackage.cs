using CatLib.Contracts.NetPackage;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CatLib.NetPackge
{
    
    /// <summary>
    /// CatLib 默认数据包
    /// </summary>
    public class CCatLibPackage : IPackage
    {

        /// <summary>
        /// 数据包
        /// </summary>
        public object Package { 
            
            get{ return null; }
            
        }

        /// <summary>
        /// 数据包字节流
        /// </summary>
        public byte[] PackageByte{ 
            
            get{ return null; }
             
        }

    }
}
