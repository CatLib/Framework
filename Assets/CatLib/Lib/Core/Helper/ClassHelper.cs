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
 
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CatLib
{

    public static class ClassHelper
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

        public static Type[] GetChildTypesWithInterface(this Type interfaceType){

            List<Type> lstType = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var t in type.GetInterfaces())
                    {
                        if (t == interfaceType)
                        {
                            lstType.Add(type);
                        }
                    }
                }
            }
            return lstType.ToArray();
            
        }

    }

}