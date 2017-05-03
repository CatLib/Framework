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
using System.Reflection;
using CatLib.API;

namespace CatLib
{
    /// <summary>
    /// CatLib实例
    /// </summary>
    public sealed class App
    {
        /// <summary>
        /// CatLib实例
        /// </summary>
        private static IApplication instance;

        /// <summary>
        /// CatLib实例
        /// </summary>
        public static IApplication Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
#if UNITY_EDITOR
                if (!UnityEngine.Application.isPlaying)
                {
                    instance = new Application().Bootstrap(Bootstrap.BootStrap);

                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        if (IsStripping(assembly))
                        {
                            continue;
                        }
                        foreach (var type in assembly.GetTypes())
                        {
                            instance.Bind(type.ToString(), type, false);
                        }
                    }
                    return instance;
                }
#endif
                throw new NullReferenceException("application not instance");
            }
            internal set
            {
                if (instance == null)
                {
                    instance = value;
                }
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// 程序集是否是被剥离的
        /// </summary>
        /// <param name="assembly">资源集</param>
        /// <returns>是否过滤</returns>
        private static bool IsStripping(Assembly assembly)
        {
            string[] notStripping = { "Assembly-CSharp-Editor" };
            foreach (var notStrippingAssembly in notStripping)
            {
                if (assembly.GetName().Name == notStrippingAssembly)
                {
                    return false;
                }
            }
            return true;
        }
#endif
    }
}