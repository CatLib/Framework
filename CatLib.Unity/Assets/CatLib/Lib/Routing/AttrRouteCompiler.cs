﻿/*
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
using CatLib.API.Routing;
using CatLib.API;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace CatLib.Routing
{
    /// <summary>
    /// 属性路由编译器
    /// </summary>
    internal sealed class AttrRouteCompiler
    {
        /// <summary>
        /// 路由器
        /// </summary>
        private readonly Router router;

        /// <summary>
        /// 被路由的特性标记
        /// </summary>
        private readonly Type routed = typeof(RoutedAttribute);

        /// <summary>
        /// 编译记录
        /// </summary>
        private readonly Dictionary<string, bool> buildRecord = new Dictionary<string, bool>();

        /// <summary>
        /// 控制器编译记录
        /// </summary>
        private readonly Dictionary<string, bool> controllerFuncBuildRecord = new Dictionary<string, bool>();

        /// <summary>
        /// 属性路由编译器
        /// </summary>
        /// <param name="router">路由器</param>
        public AttrRouteCompiler(Router router)
        {
            this.router = router;
        }

        /// <summary>
        /// 编译属性路由
        /// </summary>
        public void Complie()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (IsStripping(assembly))
                {
                    continue;
                }
                foreach (var type in assembly.GetTypes())
                {
                    ComplieRouted(type);
                }
            }

            buildRecord.Clear();
        }

        /// <summary>
        /// 编译属性路由
        /// </summary>
        /// <param name="type">编译类型</param>
        private void ComplieRouted(Type type)
        {
            if (!type.IsDefined(this.routed, false))
            {
                return;
            }
            var obj = type.GetCustomAttributes(this.routed, false);
            if (obj.Length <= 0)
            {
                return;
            }

            controllerFuncBuildRecord.Clear();

            RoutedAttribute routed;
            for (var i = 0; i < obj.Length; i++)
            {
                routed = obj[i] as RoutedAttribute;

                if (routed == null)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(routed.Path))
                {
                    routed.Path = router.GetDefaultScheme() + "://" + ClassOrFunctionNameToRouteName(type.Name);
                }

                ComplieController(type, routed);
            }
        }

        /// <summary>
        /// 编译控制器
        /// </summary>
        /// <param name="type">控制器类型</param>
        /// <param name="baseRouted">控制器路由标记</param>
        private void ComplieController(Type type, RoutedAttribute baseRouted)
        {
            //类的属性标记中的基础路径
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

            var routeList = new List<IRoute>();
            IRoute[] routes;
            foreach (var method in methods)
            {
                if ((routes = ComplieFunction(type, method, baseRouted)) != null)
                {
                    routeList.AddRange(routes);
                }
            }

            var controllerWhere = ComplieDirection(baseRouted.Where);
            var controllerDefaults = ComplieDirection(baseRouted.Defaults);

            IRoute route;
            for (var i = 0; i < routeList.Count; i++)
            {
                route = routeList[i];
                ComplieOptionsGroup(route, baseRouted);
                ComplieOptionsWhere(route, controllerWhere);
                ComplieOptionsDefaults(route, controllerDefaults);
            }
        }

        /// <summary>
        /// 编译函数
        /// </summary>
        /// <param name="controllerType">控制器类型</param>
        /// <param name="method">方法信息</param>
        /// <param name="baseRouted">控制器路由标记</param>
        private IRoute[] ComplieFunction(Type controllerType, MethodInfo method, RoutedAttribute baseRouted)
        {
            var routeds = method.GetCustomAttributes(this.routed, false);
            if (routeds.Length <= 0)
            {
                return null;
            }

            var ret = new List<IRoute>();
            RoutedAttribute routed;
            for (var i = 0; i < routeds.Length; i++)
            {
                routed = routeds[i] as RoutedAttribute;

                //如果没有给定方法路由名则默认提供
                if (string.IsNullOrEmpty(routed.Path))
                {
                    routed.Path = ClassOrFunctionNameToRouteName(method.Name);
                }

                //如果是包含scheme完整的uri那么则忽略来自控制器提供的uri
                //这里的所有的开头都不允许出现‘/’
                var path = routed.Path.TrimStart('/');
                if (baseRouted != null && !HasScheme(routed.Path))
                {
                    //如果开发者提供了控制器的路由配置，那么将会合并控制器路由的全局部分
                    var index = baseRouted.Path.LastIndexOf("://");
                    if (index != -1 && (index + 3) == baseRouted.Path.Length)
                    {
                        path = baseRouted.Path + path;
                    }
                    else
                    {
                        path = baseRouted.Path.TrimEnd('/') + "/" + path;
                    }
                }

                // 检查控制器内是否重复编译
                if (controllerFuncBuildRecord.ContainsKey(path + "$" + method.Name))
                {
                    continue;
                }
                controllerFuncBuildRecord.Add(path + "$" + method.Name, true);

                // 检查全局是否重复编译
                CheckRepeat(path, controllerType, method);

                var route = router.Reg(path, controllerType, method.Name);

                //编译标记中的属性路由中的配置到路由条目中
                ComplieOptions(route, routed);

                ret.Add(route);
            }

            return ret.ToArray();
        }

        /// <summary>
        /// 检查是否重复编译
        /// </summary>
        /// <param name="path">编译路径</param>
        /// <param name="controllerType">控制器类型</param>
        /// <param name="method">编译方法</param>
        private void CheckRepeat(string path, Type controllerType, MethodInfo method)
        {
            if (buildRecord.ContainsKey(path))
            {
                throw new CatLibException("build attr route has be repeat , class: " + controllerType.FullName + " , method: " + method.Name);
            }

            buildRecord.Add(path, true);
        }

        /// <summary>
        /// 编译配置信息
        /// </summary>
        /// <param name="route">路由条目</param>
        /// <param name="routed">路由特性</param>
        private void ComplieOptions(IRoute route, RoutedAttribute routed)
        {
            ComplieOptionsGroup(route, routed);
            ComplieOptionsWhere(route, ComplieDirection(routed.Where));
            ComplieOptionsDefaults(route, ComplieDirection(routed.Defaults));
        }

        /// <summary>
        /// 增加组信息
        /// </summary>
        /// <param name="route">路由条目</param>
        /// <param name="routed">路由特性</param>
        private void ComplieOptionsGroup(IRoute route, RoutedAttribute routed)
        {
            if (!string.IsNullOrEmpty(routed.Group))
            {
                route.Group(routed.Group);
            }
        }

        /// <summary>
        /// 增加where信息
        /// </summary>
        /// <param name="route">路由条目</param>
        /// <param name="routed">路由特性</param>
        private void ComplieOptionsWhere(IRoute route, Dictionary<string, string> routed)
        {
            foreach (var kv in routed)
            {
                route.Where(kv.Key, kv.Value, false);
            }
        }

        /// <summary>
        /// 增加defaults信息
        /// </summary>
        /// <param name="route">路由条目</param>
        /// <param name="routed">路由特性</param>
        private void ComplieOptionsDefaults(IRoute route, Dictionary<string, string> routed)
        {
            foreach (var kv in routed)
            {
                route.Defaults(kv.Key, kv.Value, false);
            }
        }

        /// <summary>
        /// 编译指向语法
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <returns>解析的指向语法</returns>
        private Dictionary<string, string> ComplieDirection(string input)
        {
            var data = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(input))
            {
                return data;
            }
            var segment = input.Split(',');

            if (segment.Length <= 0)
            {
                return data;
            }

            string[] fragment;
            var split = new[] { "=>" };
            for (var i = 0; i < segment.Length; i++)
            {
                fragment = segment[i].Split(split, StringSplitOptions.RemoveEmptyEntries);
                if (fragment.Length != 2)
                {
                    throw new RuntimeException("routed options exception , can not resolve:" + input);
                }
                data.Remove(fragment[0]);
                data.Add(fragment[0], fragment[1]);
            }

            return data;
        }

        /// <summary>
        /// 程序集是否是被剥离的
        /// </summary>
        /// <param name="assembly">资源集</param>
        /// <returns>是否过滤</returns>
        private bool IsStripping(Assembly assembly)
        {
            string[] notStripping = { "Assembly-CSharp", "Assembly-CSharp-Editor-firstpass", "Assembly-CSharp-Editor" };
            for (var i = 0; i < notStripping.Length; i++)
            {
                if (assembly.GetName().Name == notStripping[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 类名或者方法名转为路由名
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>路由名</returns>
        private string ClassOrFunctionNameToRouteName(string name)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < name.Length; i++)
            {
                if (name[i] > 'A' && name[i] < 'Z')
                {
                    if (i > 0 && !(name[i - 1] > 'A' && name[i - 1] < 'Z'))
                    {
                        builder.Append("-");
                    }
                }
                builder.Append(name[i].ToString().ToLower());
            }
            return builder.ToString();
        }

        /// <summary>
        /// 是否包含scheme
        /// </summary>
        /// <param name="uri">输入值</param>
        private bool HasScheme(string uri)
        {
            return uri.IndexOf(@"://") >= 0;
        }
    }
}