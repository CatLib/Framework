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

using CatLib.API.Routing;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CatLib.Routing
{
    /// <summary>
    /// 路由条目编译器
    /// </summary>
    internal sealed class RouteCompiler
    {
        /// <summary>
        /// 分隔符
        /// </summary>
        private const string Separators = @"/,;.:-_~+*=@|";

        /// <summary>
        /// 变量最大长度
        /// </summary>
        private const int VariableMaximumLength = 32;

        /// <summary>
        /// 编译路由条目
        /// </summary>
        /// <returns>编译后的路由条目</returns>
        public static CompiledRoute Compile(Route route)
        {
            Hashtable result;
            string[][] hostTokens = { };
            string[] hostVariables, variables;
            hostVariables = new string[] { };
            string host, hostRegex;
            hostRegex = string.Empty;

            if ((host = route.Uri.Host) != string.Empty)
            {
                result = CompilePattern(route, host, true);
                hostVariables = result["variables"] as string[];

                hostTokens = result["tokens"] as string[][];
                hostRegex = result["regex"].ToString();
            }

            var uri = Regex.Replace(route.Uri.FullPath, @"\{(\w+?)\?\}", "{$1}");
            result = CompilePattern(route, uri, false);

            var staticPrefix = result["staticPrefix"].ToString();
            var pathVariables = result["variables"] as string[];

            var tmp = new List<string>(hostVariables);
            for (var i = 0; i < pathVariables.Length; i++)
            {
                if (!tmp.Contains(pathVariables[i]))
                {
                    tmp.Add(pathVariables[i]);
                }
            }

            variables = tmp.ToArray();

            var tokens = result["tokens"] as string[][];
            var regex = result["regex"].ToString();

            if (string.IsNullOrEmpty(regex) ||
                    string.IsNullOrEmpty(hostRegex))
            {
                throw new RuntimeException("Compiler route faild , uri:" + route.Uri.FullPath);
            }

            return new CompiledRoute
            {
                StaticPrefix = staticPrefix,
                RouteRegex = new Regex(regex),
                Tokens = tokens,
                PathVariables = pathVariables,
                HostRegex = new Regex(hostRegex),
                HostTokens = hostTokens,
                HostVariables = hostVariables,
                Variables = variables
            };
        }

        /// <summary>
        /// 编译参数
        /// </summary>
        /// <param name="route">路由条目</param>
        /// <param name="uri">uri</param>
        /// <param name="isHost">是否是host</param>
        /// <returns>编译数据</returns>
        private static Hashtable CompilePattern(Route route, string uri, bool isHost)
        {
            int[] parametersIndex = null;

            var defaultSeparator = isHost ? "." : "/";

            //可选参数
            var optionalParameters = new List<string>(MatchParameters(route.Uri.FullPath, @"\{(\w+?)\?\}", ref parametersIndex));

            //所有参数
            var parameters = MatchParameters(uri, @"\{(\w+?)\}", ref parametersIndex);

            //已经被使用的变量名
            var variables = new List<string>();

            var tokens = new List<string[]>();

            var pos = 0;
            string varName, precedingText, precedingChar, where, followingPattern, nextSeparator;
            bool isSeparator;

            for (var i = 0; i < parameters.Length; i++)
            {
                varName = parameters[i];

                // 获取当前匹配的变量之前的静态文本
                precedingText = uri.Substring(pos, parametersIndex[i] - pos);
                pos = parametersIndex[i] + parameters[i].Length + 2;

                precedingChar = precedingText.Length <= 0 ? string.Empty : precedingText.Substring(precedingText.Length - 1);

                isSeparator = string.Empty != precedingChar && Separators.Contains(precedingChar);

                if (IsMatch(@"^\d", varName))
                {
                    throw new DomainException(string.Format("Variable name [{0}] cannot start with a digit in route pattern [{1}]. please use a different name.", varName, uri));
                }

                if (variables.Contains(varName))
                {
                    throw new DomainException(string.Format("Route pattern [{0}] cannot reference variable name [{1}] more than once.", varName, uri));
                }

                if (varName.Length > VariableMaximumLength)
                {
                    throw new DomainException(string.Format("Variable name [{0}] cannot be longer than [{1}] characters in route pattern [{2}]. please use a shorter name.", varName, VariableMaximumLength, uri));
                }

                if (isSeparator && precedingText != precedingChar)
                {
                    tokens.Add(new[] { "text", precedingText.Substring(0, precedingText.Length - precedingChar.Length) });
                }
                else if (!isSeparator && precedingText.Length > 0)
                {
                    tokens.Add(new[] { "text", precedingText });
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
                        Regex.Escape(defaultSeparator),
                        defaultSeparator != nextSeparator && nextSeparator != string.Empty ? Regex.Escape(nextSeparator) : string.Empty
                    );
                }

                tokens.Add(new[] { "variable", isSeparator ? precedingChar : string.Empty, where, varName });
                variables.Add(varName);
            }

            //如果不是全部内容则追加入后续内容
            if (pos < uri.Length)
            {
                tokens.Add(new[] { "text", uri.Substring(pos) });
            }

            var firstOptional = int.MaxValue;
            if (!isHost)
            {
                string[] token;
                for (var i = tokens.Count - 1; i >= 0; --i)
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
            var regexp = string.Empty;
            for (int i = 0, nbToken = tokens.Count; i < nbToken; ++i)
            {
                regexp += ComputeRegexp(tokens, i, firstOptional);
            }
            regexp = '^' + regexp + "$";

            var hash = new Hashtable
                        {
                            { "staticPrefix" ,  "text" == tokens[0][0] ? tokens[0][1] : string.Empty },
                            { "regex" , regexp },
                            { "variables" , variables.ToArray() }
                        };

            tokens.Reverse();
            hash.Add("tokens", tokens.ToArray());

            return hash;
        }

        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <param name="tokens">处理块</param>
        /// <param name="index">第几个下标</param>
        /// <param name="firstOptional">第一个可选项的下标</param>
        /// <returns></returns>
        private static string ComputeRegexp(IList<string[]> tokens, int index, int firstOptional)
        {
            var token = tokens[index];

            if (token[0] == "text")
            {
                //传统文本匹配格式
                return Regex.Escape(token[1]);
            }

            if (index == 0 && firstOptional == 0)
            {
                return string.Format("{0}(?<{1}>{2})?", Regex.Escape(token[1]), token[3], token[2]);
            }

            var regexp = string.Format("{0}(?<{1}>{2})", Regex.Escape(token[1]), token[3], token[2]);
            if (index < firstOptional)
            {
                return regexp;
            }

            regexp = "(?:" + regexp;
            var nbTokens = tokens.Count;
            if (nbTokens - 1 == index)
            {
                regexp += Str.Repeat(")?", nbTokens - firstOptional - (0 == firstOptional ? 1 : 0));
            }

            return regexp;
        }

        /// <summary>
        /// 搜索下一个分隔符
        /// </summary>
        /// <param name="uri">uri</param>
        /// <returns>下一个分隔符</returns>
        private static string FindNextSeparator(string uri)
        {
            if (uri == string.Empty)
            {
                return string.Empty;
            }
            // 先删除所有占位符，这样才能找到真正的静态内容
            if (string.Empty == (uri = Regex.Replace(uri, @"\{\w+\}+?", string.Empty)))
            {
                return string.Empty;
            }
            return Separators.Contains(uri[0].ToString()) ? uri[0].ToString() : string.Empty;
        }

        /// <summary>
        /// 是否匹配
        /// </summary>
        /// <param name="val">输入值</param>
        /// <param name="regstr">正则表达式</param>
        /// <returns></returns>
        private static bool IsMatch(string regstr, string val)
        {
            var reg = new Regex(regstr);
            return reg.IsMatch(val);
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="regstr">正则表达式</param>
        /// <param name="parameIndex">参数下标</param>
        /// <returns>匹配到的参数</returns>
        private static string[] MatchParameters(string uri, string regstr, ref int[] parameIndex)
        {
            var reg = new Regex(regstr);
            var mc = reg.Matches(uri);

            var parameters = new string[mc.Count];
            parameIndex = new int[mc.Count];
            for (var i = 0; i < mc.Count; i++)
            {
                parameIndex[i] = mc[i].Index;
                parameters[i] = mc[i].Groups[1].ToString();
            }

            return parameters;
        }
    }
}