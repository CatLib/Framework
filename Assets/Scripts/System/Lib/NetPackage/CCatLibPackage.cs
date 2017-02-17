using System;
using CatLib.Contracts.NetPackage;

namespace CatLib.NetPackage
{
    
    /// <summary>
    /// CatLib 默认数据包
    /// </summary>
    public class CCatLibPackage : IPackage
    {

        private object package;

        public CCatLibPackage(object package){

            this.package = package;

        }

        /// <summary>
        /// 数据包
        /// </summary>
        public object Package { 
            
            get{ return package; }
            
        }

        /// <summary>
        /// 数据包字节流
        /// </summary>
        public byte[] ToByte(){ 
            
            if(package is byte[]){
                
                return (byte[])package;
            
            }

            throw new ApplicationException("this model not support toByte()");
             
        }

    }
}
