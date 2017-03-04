using CatLib.API.Resources;
using UnityEngine;

namespace CatLib.Resources
{

    public class DefaultObjectInfo : IObjectInfo
    {

        private Object obj;

        public DefaultObjectInfo(Object obj)
        {
            this.obj = obj;
        }

        public void Instantiate()
        {

        }

        public T Get<T>() where T : Object
        {
            return obj as T;
        }

    }

}