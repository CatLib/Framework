using System.Text.RegularExpressions;
using System.Collections;

namespace CatLib.Routing
{

    /// <summary>
    /// 路由条目编译器
    /// </summary>
    public class RouteCompiler
    {

        /// <summary>
        /// 编译路由条目
        /// </summary>
        /// <returns></returns>
        public static CompiledRoute Compile(Route route)
        {
            string uri = Regex.Replace(route.Uri.OriginalString, @"\{(\w+?)\?\}", "{$1}");

            Hashtable hashtable = CompilePattern(route, uri);



            return null;

        }

        /// <summary>
        /// 编译参数
        /// </summary>
        /// <param name="route"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        protected static Hashtable CompilePattern(Route route , string uri)
        {

            string[] parameters = GetParameters(uri);
            string[] optionalParameters = GetOptionalParameters(route);


            UnityEngine.Debug.Log(uri);

            return null;
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="uri">uri</param>
        /// <returns></returns>
        protected static string[] GetParameters(string uri)
        {
            string regstr = @"\{(\w+?)\}";
            Regex reg = new Regex(regstr);
            MatchCollection mc = reg.Matches(uri);

            string[] parameters = new string[mc.Count];
            for (int i = 0; i < mc.Count; i++)
            {
                parameters[i] = mc[i].Groups[1].ToString();
            }

            return parameters;
        }

        /// <summary>
        /// 获取可选的参数
        /// </summary>
        /// <returns></returns>
        protected static string[] GetOptionalParameters(Route route)
        {

            string regstr = @"\{(\w+?)\?\}";
            Regex reg = new Regex(regstr);

            MatchCollection mc = reg.Matches(route.Uri.OriginalString);

            string[] parameters = new string[mc.Count];
            for (int i = 0; i < mc.Count; i++)
            {
                parameters[i] = mc[i].Groups[1].ToString();
            }

            return parameters;

        }


    }

}