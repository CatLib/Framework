using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CatLib.Support
{

    public static class CClass
    {

        /// <summary>
        /// 获取子类类型
        /// </summary>
        /// <param name="parentType"></param>
        /// <returns></returns>
        public static Type[] GetChildTypes(this Type parentType)
        {

            List<Type> lstType = new List<Type>();
            Assembly assem = Assembly.GetAssembly(parentType);

            foreach (Type tChild in assem.GetTypes())
            {
                if (tChild.BaseType == parentType)
                {
                    lstType.Add(tChild);
                }
            }

            return lstType.ToArray();

        }


    }

}