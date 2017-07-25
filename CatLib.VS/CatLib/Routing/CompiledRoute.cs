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

using System.Text;
using System.Text.RegularExpressions;

namespace CatLib.Routing
{
    /// <summary>
    /// 编译后的路由信息
    /// </summary>
    internal sealed class CompiledRoute
    {
        /// <summary>
        /// 静态文本
        /// </summary>
        public string StaticPrefix { get; set; }

        /// <summary>
        /// 路由匹配表达式
        /// </summary>
        public Regex RouteRegex { get; set; }

        /// <summary>
        /// 所有需要匹配的变量单独的正则匹配式
        /// </summary>
        public string[][] Tokens { get; set; }

        /// <summary>
        /// 路径中的变量
        /// </summary>
        public string[] PathVariables { get; set; }

        /// <summary>
        /// 匹配host的表达式
        /// </summary>
        public Regex HostRegex { get; set; }

        /// <summary>
        /// host部分的需要匹配变量单独的正则表达式
        /// </summary>
        public string[][] HostTokens { get; set; }

        /// <summary>
        /// 匹配host的的变量名
        /// </summary>
        public string[] HostVariables { get; set; }

        /// <summary>
        /// 所有的变量列表
        /// </summary>
        public string[] Variables { get; set; }

        /// <summary>
        /// 转为字符串
        /// </summary>
        /// <returns>编译后的字符串表示信息</returns>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine("[RouteRegex]: " + RouteRegex);
            builder.AppendLine();

            builder.AppendLine("[StaticPrefix]: " + StaticPrefix);
            builder.AppendLine();

            builder.AppendLine("[Tokens]:");
            for (var i = 0; i < Tokens.Length; i++)
            {
                if (Tokens[i][0] == "text")
                {
                    builder.AppendLine("    [type]: " + Tokens[i][0] + " , [regex]: " + Tokens[i][1]);
                }
                else
                {
                    builder.AppendLine("    [type]: " + Tokens[i][0] + " , [preceding]: " + Tokens[i][1] + " , [regex]: " + Tokens[i][2] + " , [var]: " + Tokens[i][3]);
                }
            }

            builder.AppendLine();
            builder.AppendLine("[PathVariables]: ");
            for (var i = 0; i < PathVariables.Length; i++)
            {
                builder.AppendLine("    " + PathVariables[i]);
            }

            builder.AppendLine();
            builder.AppendLine("[HostRegex]: " + HostRegex);

            builder.AppendLine();
            builder.AppendLine("[HostTokens]: ");
            for (var i = 0; i < HostTokens.Length; i++)
            {
                if (HostTokens[i][0] == "text")
                {
                    builder.AppendLine("    [type]: " + HostTokens[i][0] + " , [regex]: " + HostTokens[i][1]);
                }
                else
                {
                    builder.AppendLine("    [type]: " + HostTokens[i][0] + " , [preceding]: " + HostTokens[i][1] + " , [regex]: " + HostTokens[i][2] + " , [var]: " + HostTokens[i][3]);
                }
            }

            builder.AppendLine();
            builder.AppendLine("[HostVariables]: ");
            for (var i = 0; i < HostVariables.Length; i++)
            {
                builder.AppendLine("    " + HostVariables[i]);
            }

            builder.AppendLine();
            builder.AppendLine("[Variables]: ");
            for (var i = 0; i < Variables.Length; i++)
            {
                builder.AppendLine("    " + Variables[i]);
            }

            return builder.ToString();
        }
    }
}