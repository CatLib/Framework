using UnityEngine;

namespace CatLib.API.Resources
{
    /// <summary>对象信息</summary>
    public interface IObjectInfo
    {

        void Instantiate();

        T Get<T>() where T : Object;

    }

}