using UnityEngine;

namespace CatLib.API.Resources
{
    /// <summary>对象信息</summary>
    public interface IObjectInfo
    {

        GameObject Instantiate();

        T Get<T>(object hostedObject) where T : Object;

        Object Get(object hostedObject);

        Object UnHostedGet();

    }

}