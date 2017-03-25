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
using System.Text.RegularExpressions;

namespace CatLib.Routing
{

    /// <summary>
    /// 路由参数绑定
    /// </summary>
    public class RouteParameterBinder
    {

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static void Parameters(Route route , Request request)
        {   
            BindPathParameters(route , request);
            BindQueryParameters(route, request);
            BindHostParameters(route , request);
            ReplaceDefaults(route, request);
        }

        /// <summary>
        /// 匹配路径
        /// </summary>
        /// <param name="route"></param>
        /// <param name="request"></param>
        protected static void BindPathParameters(Route route , Request request)
        {
            Regex reg = new Regex(route.Compiled.RouteRegex);
            MatchToKeys(route , request , reg.Match(request.SchemeHostPath));
        }

        /// <summary>
        /// 匹配Host
        /// </summary>
        /// <param name="route"></param>
        /// <param name="request"></param>
        protected static void BindHostParameters(Route route, Request request)
        {
            Regex reg = new Regex(route.Compiled.HostRegex);
            MatchToKeys(route , request , reg.Match(request.Host));
        }

        /// <summary>
        /// 匹配绑定参数
        /// </summary>
        /// <param name="route"></param>
        /// <param name="request"></param>
        protected static void BindQueryParameters(Route route , Request request)
        {
            if (request.Uri.Query.Length <= 0) { return; }
            string query = request.Uri.Query;
            query = query.Substring(1, query.Length - 1);

            string[] paramKV = query.Split('&');
            string[] kv;
            foreach(string parameter in paramKV)
            {
                kv = parameter.Split('=');
                if(kv.Length == 2)
                {
                    request.AddParameters(kv[0], kv[1]);
                }
            }

        }

        /// <summary>
        /// 匹配路径中的key
        /// </summary>
        /// <param name="route"></param>
        /// <param name="request"></param>
        /// <param name="matches"></param>
        protected static void MatchToKeys(Route route , Request request , Match matches)
        {   
            string[] parameterNames = route.Compiled.Variables;
            if(parameterNames.Length <= 0){ return; }

            string val;
            for(int i = 0 ; i < route.Compiled.Variables.Length ; i++){

                val = matches.Groups[route.Compiled.Variables[i]].Value;
                if(!string.IsNullOrEmpty(val)){
                    request.AddParameters(route.Compiled.Variables[i] , val);
                }

            }
        }

        /// <summary>
        /// 将没有传入的参数替换为默认参数
        /// </summary>
        /// <param name="route"></param>
        /// <param name="request"></param>
        protected static void ReplaceDefaults(Route route , Request request){

            string varName , defaults;
            for(int i = 0 ; i < route.Compiled.Variables.Length ; i++){
                
                varName = route.Compiled.Variables[i];
                if(request.Get(varName) == null){

                    defaults = route.GetDefaults(varName);
                    if(!string.IsNullOrEmpty(defaults)){

                        request.AddParameters(varName , defaults);

                    }

                }
                

            }

        }
    }

}