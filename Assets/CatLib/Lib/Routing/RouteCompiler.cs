using System.Text.RegularExpressions;
using System.Collections;
using CatLib.API.Routing;
using System.Collections.Generic;

namespace CatLib.Routing
{

    /// <summary>
    /// 路由条目编译器
    /// </summary>
    public class RouteCompiler
    {

        private const string SEPARATORS = @"/,;.:-_~+*=@|";

        private const int VARIABLE_MAXIMUM_LENGTH = 32;

        /// <summary>
        /// 编译路由条目
        /// </summary>
        /// <returns></returns>
        public static CompiledRoute Compile(Route route)
        {
            string uri = Regex.Replace(route.Uri.OriginalString, @"\{(\w+?)\?\}", "{$1}");

            Hashtable hashtable = CompilePattern(route, uri , false);

            string staticPrefix = hashtable["staticPrefix"].ToString();
            string[] pathVariables = hashtable["variables"] as string[];

            

            return null;

        }

        /// <summary>
        /// 编译参数
        /// </summary>
        /// <param name="route"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        protected static Hashtable CompilePattern(Route route, string uri, bool isHost)
        {

            int[] parametersIndex = null;

            string defaultSeparator = isHost ? "." : "/";

            //可选参数
            List<string> optionalParameters = new List<string>(MatchParameters(route.Uri.OriginalString, @"\{(\w+?)\?\}", ref parametersIndex));

            //所有参数
            string[] parameters = MatchParameters(uri, @"\{(\w+?)\}", ref parametersIndex);

            //已经被使用的变量名
            List<string> variables = new List<string>();

            List<string[]> tokens = new List<string[]>();

            UnityEngine.Debug.Log(uri);

            int pos = 0;
            string varName, precedingText, precedingChar, where, followingPattern, nextSeparator;
            bool isSeparator;
            for (int i = 0; i < parameters.Length; i++)
            {

                varName = parameters[i];

                // 获取当前匹配的变量之前的静态文本
                precedingText = uri.Substring(pos, parametersIndex[i] - pos);
                pos = parametersIndex[i] + parameters[i].Length + 2;

                if (precedingText.Length <= 0)
                {
                    precedingChar = string.Empty;
                }
                else
                {
                    precedingChar = precedingText.Substring(precedingText.Length - 1);
                }

                isSeparator = string.Empty != precedingChar && SEPARATORS.Contains(precedingChar);

                if (IsMatch(@"^\d", varName))
                {
                    throw new DomainException(string.Format("variable name {0} cannot start with a digit in route pattern {1}. please use a different name.", varName, uri));
                }

                if (variables.Contains(varName))
                {
                    throw new DomainException(string.Format("route pattern {0} cannot reference variable name {1} more than once.", varName, uri));
                }

                if (varName.Length > VARIABLE_MAXIMUM_LENGTH)
                {
                    throw new DomainException(string.Format("variable name {0} cannot be longer than {1} characters in route pattern {2}. please use a shorter name.", varName, VARIABLE_MAXIMUM_LENGTH, uri));
                }

                if (isSeparator && precedingText != precedingChar)
                {
                    tokens.Add(new string[] { "text", precedingText.Substring(0, precedingText.Length - precedingChar.Length) });
                }
                else if (!isSeparator && precedingText.Length > 0)
                {
                    tokens.Add(new string[] { "text", precedingText });
                }

                //获取where的约束条件
                where = route.GetWhere(varName);

                if (where == null)
                {
                    //获取之后的内容
                    followingPattern = uri.Substring(pos);

                    //下一个分隔符
                    nextSeparator = FindNextSeparator(followingPattern);

                    where = string.Format(
                        "[^{0}{1}]+",
                        RegexQuote(defaultSeparator),
                        defaultSeparator != nextSeparator && nextSeparator != string.Empty ? RegexQuote(nextSeparator) : string.Empty
                    );

                }

                tokens.Add(new string[] { "variable", isSeparator ? precedingChar : string.Empty, where, varName });
                variables.Add(varName);

            }

            //如果不是全部内容则追加入后续内容
            if (pos < uri.Length)
            {
                tokens.Add(new string[] { "text", uri.Substring(pos) });
            }

            int firstOptional = int.MaxValue;
            if (!isHost)
            {
                string[] token;
                for (int i = tokens.Count - 1; i >= 0; --i)
                {
                    token = tokens[i];
                    if ("variable" == token[0] && optionalParameters.Contains(token[3]))
                    {
                        firstOptional = i;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            //计算并生成最终的表达式
            string regexp = string.Empty;
            for (int i = 0, nbToken = tokens.Count; i < nbToken; ++i) {
                regexp += ComputeRegexp(tokens, i, firstOptional);
            }
            regexp = '^' + regexp + "$";

            var hash = new Hashtable()
                        {
                            { "staticPrefix" ,  "text" == tokens[0][0] ? tokens[0][1] : string.Empty },
                            { "regexp" , regexp },
                            { "variables" , variables.ToArray() }
                        };

            tokens.Reverse();
            hash.Add("tokens", tokens);

            return hash;

        }

        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="index"></param>
        /// <param name="firstOptional"></param>
        /// <returns></returns>
        protected static string ComputeRegexp(List<string[]> tokens , int index , int firstOptional)
        {

            string[] token = tokens[index];

            if (token[0] == "text")
            {
                //传统文本匹配格式
                return RegexQuote(token[1]);
            }
            else
            {
                //变量匹配格式

                if (index == 0 && firstOptional == 0) {
                    // 如果唯一一个变量token那么必须加入分隔符
                    return string.Format("{0}(?<{1}>{2})?", RegexQuote(token[1]), token[3], token[2]);
                }

                string regexp = string.Format("{0}(?<{1}>{2})", RegexQuote(token[1]), token[3], token[2]);
                if (index >= firstOptional)
                {
                    regexp = "(?:" + regexp;
                    int nbTokens = tokens.Count;
                    if (nbTokens - 1 == index)
                    {
                        regexp += StrRepeat(")?", nbTokens - firstOptional - (0 == firstOptional ? 1 : 0));
                    }
                }

                return regexp;

            }

        }

        /// <summary>
        /// 重复字符串
        /// </summary>
        /// <param name="val"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        protected static string StrRepeat(string val , int num)
        {
            string tmp = string.Empty;
            for(int i = 0; i < num; i++)
            {
                tmp += val;
            }
            return tmp;
        }

        /// <summary>
        /// 转义正则表达式字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected static string RegexQuote(string str)
        {
            string[] quote = new string[] { ".", @"\" , "+" , "*" , "?","[", "^", "]", "$", "(", ")", "{", "}", "=", "!", "<", ">", "|", ":", "-" };

            foreach(string q in quote)
            {
                str = str.Replace(q, "\\" + q);
            }
            return str;
        }

        /// <summary>
        /// 搜索下一个分隔符
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        protected static string FindNextSeparator(string uri)
        {
            if (uri == string.Empty) {
                return string.Empty;
            }
            // 先删除所有占位符，这样才能找到真正的静态内容
            if (string.Empty == (uri = Regex.Replace(uri, @"\{\w+\}+?", string.Empty))) {
                return string.Empty;
            }
            return false != SEPARATORS.Contains(uri[0].ToString()) ? uri[0].ToString() : string.Empty;
        }


        /// <summary>
        /// 是否匹配
        /// </summary>
        /// <param name="val"></param>
        /// <param name="regstr"></param>
        /// <returns></returns>
        protected static bool IsMatch(string val , string regstr)
        {
            Regex reg = new Regex(regstr);
            return reg.IsMatch(val);
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="regstr">正则表达式</param>
        /// <returns></returns>
        protected static string[] MatchParameters(string uri , string regstr , ref int[] parameIndex)
        {
            Regex reg = new Regex(regstr);
            MatchCollection mc = reg.Matches(uri);

            string[] parameters = new string[mc.Count];
            parameIndex = new int[mc.Count];
            for (int i = 0; i < mc.Count; i++)
            {
                parameIndex[i] = mc[i].Index;
                parameters[i] = mc[i].Groups[1].ToString();
            }

            return parameters;
        }


    }

}