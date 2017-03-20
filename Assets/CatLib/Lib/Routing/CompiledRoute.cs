
using System.Collections.Generic;

namespace CatLib.Routing
{

    /// <summary>
    /// 编译后的路由信息
    /// </summary>
    public class CompiledRoute
    {

        /// <summary>
        /// 静态文本
        /// </summary>
        public string StaticPrefix{ get; set; }

        /// <summary>
        /// 路由匹配表达式
        /// </summary>
        public string RouteRegex{ get; set; }

        /// <summary>
        /// 所有需要匹配的变量单独的正则匹配式
        /// </summary>
        public string[] Tokens{ get; set; }

        /// <summary>
        /// 路径中的变量
        /// </summary>
        public string[] PathVariables{ get; set; }

        /// <summary>
        /// 匹配host的表达式
        /// </summary>
        public string HostRegex{ get; set; }

        /// <summary>
        /// host部分的需要匹配变量单独的正则表达式
        /// </summary>
        public string[] HostTokens{ get; set; }

        /// <summary>
        /// 匹配host的的变量名
        /// </summary>
        public string[] HostVariables{ get; set; }

        /// <summary>
        /// 所有的变量列表
        /// </summary>
        public string[] Variables{ get; set; }


        public CompiledRoute(){}

    }

}