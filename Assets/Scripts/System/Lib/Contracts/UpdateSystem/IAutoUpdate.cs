using UnityEngine;
using System.Collections;
using XLua;

namespace CatLib.Contracts.UpdateSystem
{

    [LuaCallCSharp]
    /// <summary>
    /// 自动更新接口
    /// </summary>
    public interface IAutoUpdate
    {

        bool UpdateAsset();

        bool UpdateAsset(string resUrl);

        int NeedUpdateNum{ get; }

        int UpdateNum{ get;}

    }
}
