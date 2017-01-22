using UnityEngine;
using System.Collections;

namespace CatLib.Contracts.UpdateSystem
{

    /// <summary>
    /// 自动更新接口
    /// </summary>
    public interface IAutoUpdate
    {

        bool UpdateAsset();

        bool UpdateAsset(string resUrl);

    }
}
