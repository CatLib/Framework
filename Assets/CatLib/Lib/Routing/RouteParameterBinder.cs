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
            BindHostParameters(route , request);
            ReplaceDefaults(route, request);
        }

        protected static void BindPathParameters(Route route , Request request)
        {
            Regex reg = new Regex(route.Compiled.RouteRegex);
            MatchToKeys(route , request , reg.Match(request.Uri));
        }

        protected static void BindHostParameters(Route route, Request request)
        {
            Regex reg = new Regex(route.Compiled.HostRegex);
            MatchToKeys(route , request , reg.Match(request.Host));
        }

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

        protected static void ReplaceDefaults(Route route , Request request){


        }
    }

}