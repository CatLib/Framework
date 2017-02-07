using UnityEngine;
using System.Collections;
using CapLib.Base;
using CatLib.Container;
using XLua;

namespace CatLib.Base
{

    /// <summary>
    /// 门面基类
    /// </summary>
    public class CBaseFacade<T>
    {

        public static T Instance
        {
            get
            {
                return CApp.Instance.Make<T>();
            }
        }
    }

}