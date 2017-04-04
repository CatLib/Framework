/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
 
using CatLib.API.Resources;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CatLib.Resources
{

    public class DefaultObjectInfo : IObject
    {

        private Object obj;

        public DefaultObjectInfo(Object obj)
        {
            this.obj = obj;
        }

        public GameObject Instantiate()
        {
            if (obj != null)
            {
                if (obj is GameObject)
                {
                    GameObject prefab = obj as GameObject;
                    Object ins = Object.Instantiate(prefab);
                    ins.name = prefab.name;
                    return (GameObject)ins;
                }
            }
            return null;
        }

        public Object UnHostedGet()
        {
            return obj;
        }

        public T Get<T>(object hostedObject) where T : Object
        {
            return obj as T;
        }

        public Object Get(object hostedObject)
        {
            return obj;
        }

    }

}