using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace CatLib.NetPackge
{

    /// <summary>
    /// 包头
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable()]
    public class CPackageHead
    {

        /// <summary>
        /// 保留标记
        /// </summary>
        public ushort Flag;

        /// <summary>
        /// 包体长度
        /// </summary>
        public ushort BodyLength;

        /// <summary>
        /// 校验码
        /// </summary>
        public ushort CheckCode;

    }

}