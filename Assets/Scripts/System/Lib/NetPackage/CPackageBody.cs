using CatLib.Contracts.NetPackage;
using System;
using System.Runtime.InteropServices;


namespace CatLib.NetPackge
{

    /// <summary>
    /// 包体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable()]
    public class CPackageBody : IPackageBody
    {

        private byte[] content;

        public byte[] Content { get { return content; } set { content = value; } }

        public CPackageBody() { }

        public CPackageBody(byte[] content)
        {
            Content = content;
        }

    }

}