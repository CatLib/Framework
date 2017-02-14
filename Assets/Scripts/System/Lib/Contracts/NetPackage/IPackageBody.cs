using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatLib.Contracts.NetPackage
{

    /// <summary>
    /// 包体接口
    /// </summary>
    public interface IPackageBody
    {

        byte[] Content { get; }

    }

}