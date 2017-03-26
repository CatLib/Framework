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
using CatLib.API.Routing;
using CatLib.API.Container;
using CatLib.API;
using System.Reflection;
using System.Collections.Generic;

namespace CatLib.Routing
{

    /// <summary>
    /// 属性路由编译器
    /// </summary>
    public class AttrRouteCompiler
    {

        /// <summary>
        /// 路由器
        /// </summary>
        private Router router;

        /// <summary>
        /// 扫描目标
        /// </summary>
        private Type target = typeof(IRouted);

        /// <summary>
        /// 被路由的属性
        /// </summary>
        private Type routed = typeof(Routed);

        /// <summary>
        /// 属性路由编译器
        /// </summary>
        /// <param name="router"></param>
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
                foreach (var type in assembly.GetTypes())
                {
                    ComplieRouted(type);
                }
            }
        }

        /// <summary>
        /// 编译属性路由
        /// </summary>
        /// <param name="type"></param>
        protected void ComplieRouted(Type type)
        {
            if (!(target.IsAssignableFrom(type) && type != target)) { return; }
            object[] obj = type.GetCustomAttributes(this.routed, true);

            Routed routed = null;
            if (obj.Length > 0)
            {
                routed = obj[0] as Routed;
            }

            ComplieController(type, routed);

        }

        /// <summary>
        /// 编译控制器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="routed"></param>
        protected void ComplieController(Type type , Routed baseRouted)
        {

            //类的属性标记中的基础路径
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

            List<IRoute> routeList = new List<IRoute>();
            IRoute route;
            foreach (MethodInfo method in methods)
            {
                if((route = ComplieFunction(type , method, baseRouted)) != null)
                {
                    routeList.Add(route);
                }
            }

            if (baseRouted == null) { return; }

            Dictionary<string, string> controllerWhere = ComplieDirection(baseRouted.Where);
            Dictionary<string, string> controllerDefaults = ComplieDirection(baseRouted.Defaults);
            
            for(int i = 0; i < routeList.Count; i++)
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
        /// <param name="method"></param>
        /// <param name="baseRouted"></param>
        protected IRoute ComplieFunction(Type type , MethodInfo method, Routed baseRouted)
        {

            object[] routeds = method.GetCustomAttributes(this.routed, false);
            if (routeds.Length <= 0) { return null; }

            Routed routed = routeds[0] as Routed;
            if (routed == null) { return null; }

            //如果是包含scheme完整的uri那么则忽略来自控制器提供的uri
            //这里的所有的开头都不允许出现‘/’
            string path = routed.Path.TrimStart('/');
            if (baseRouted != null && !HasScheme(routed.Path))
            {
                //如果开发者提供了控制器的路由配置，那么将会合并控制器路由的全局部分
                int index = baseRouted.Path.LastIndexOf("://");
                if (index != -1 && (index + 3) == baseRouted.Path.Length)
                {
                    path = baseRouted.Path + path;
                }else
                {
                    path = baseRouted.Path.TrimEnd('/') + "/" + path;
                }
            }

            IRoute route = router.Reg(path, type , method.Name);

            //编译标记中的属性路由中的配置到路由条目中
            ComplieOptions(route, routed);

            return route;

        }

        /// <summary>
        /// 编译配置信息
        /// </summary>
        /// <param name="route"></param>
        /// <param name="routed"></param>
        protected void ComplieOptions(IRoute route , Routed routed)
        {
            ComplieOptionsGroup(route, routed);
            ComplieOptionsWhere(route, ComplieDirection(routed.Where));
            ComplieOptionsDefaults(route, ComplieDirection(routed.Defaults));
        }

        /// <summary>
        /// 增加组信息
        /// </summary>
        /// <param name="route"></param>
        /// <param name="routed"></param>
        protected void ComplieOptionsGroup(IRoute route, Routed routed)
        {
            if (!string.IsNullOrEmpty(routed.Group))
            {
                route.Group(routed.Group);
            }
        }

        /// <summary>
        /// 增加where信息
        /// </summary>
        /// <param name="route"></param>
        /// <param name="routed"></param>
        protected void ComplieOptionsWhere(IRoute route, Dictionary<string , string> routed)
        {
            foreach(var kv in routed)
            {
                route.Where(kv.Key, kv.Value);
            }
        }

        /// <summary>
        /// 增加defaults信息
        /// </summary>
        /// <param name="route"></param>
        /// <param name="routed"></param>
        protected void ComplieOptionsDefaults(IRoute route, Dictionary<string, string> routed)
        {
            foreach (var kv in routed)
            {
                route.Defaults(kv.Key, kv.Value);
            }
        }

        /// <summary>
        /// 编译指向语法
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        protected Dictionary<string , string> ComplieDirection(string val)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(val)) { return data; }
            string[] segment = val.Split(',');
            if (segment.Length <= 0) { return data; }

            string[] fragment;
            string[] split = new string[] { "=>" };
            for (int i = 0; i < segment.Length; i++)
            {
                fragment = segment[i].Split(split, StringSplitOptions.RemoveEmptyEntries);
                if(fragment.Length != 2)
                {
                    throw new RuntimeException("routed options exception , can not resolve:" + val);
                }
                data.Remove(fragment[0]);
                data.Add(fragment[0], fragment[1]);
            }

            return data;
        }

        /// <summary>
        /// 是否包含scheme
        /// </summary>
        /// <param name="path"></param>
        protected bool HasScheme(string path)
        {
            return path.IndexOf(@"://") >= 0;
        }

    }

}