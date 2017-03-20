
using System.Collections.Generic;
using System.Text;

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
        public string[][] Tokens{ get; set; }

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
        public string[][] HostTokens{ get; set; }

        /// <summary>
        /// 匹配host的的变量名
        /// </summary>
        public string[] HostVariables{ get; set; }

        /// <summary>
        /// 所有的变量列表
        /// </summary>
        public string[] Variables{ get; set; }


        public CompiledRoute(){}

        public override string ToString(){

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("[RouteRegex]: " + RouteRegex);
            builder.AppendLine();

            builder.AppendLine("[StaticPrefix]: " + StaticPrefix);
            builder.AppendLine();
            

            builder.AppendLine("[Tokens]:");
            for(int i = 0 ; i < Tokens.Length ; i++){

                if(Tokens[i][0] == "text"){

                    builder.AppendLine("    [type]: " + Tokens[i][0] + " , [regex]: " + Tokens[i][1]);
                }else{
                    builder.AppendLine("    [type]: " + Tokens[i][0] + " , [preceding]: " + Tokens[i][1]+ " , [regex]: " + Tokens[i][2] + " , [var]: " + Tokens[i][3]);
                }

            }

            builder.AppendLine();
            builder.AppendLine("[PathVariables]: ");
            for(int i = 0 ; i < PathVariables.Length ; i++){

                builder.AppendLine("    " + PathVariables[i]);

            }

            builder.AppendLine();
            builder.AppendLine("[HostRegex]: " + HostRegex);

            builder.AppendLine();
            builder.AppendLine("[HostTokens]: ");
            for(int i = 0 ; i < HostTokens.Length ; i++){

                if(HostTokens[i][0] == "text"){
                    builder.AppendLine("    [type]: " + HostTokens[i][0] + " , [regex]: " + HostTokens[i][1]);
                }else{
                    builder.AppendLine("    [type]: " + HostTokens[i][0] + " , [preceding]: " + HostTokens[i][1]+ " , [regex]: " + HostTokens[i][2] + " , [var]: " + HostTokens[i][3]);
                }

            }

            builder.AppendLine();
            builder.AppendLine("[HostVariables]: ");
            for(int i = 0 ; i < HostVariables.Length ; i++){

                builder.AppendLine("    " + HostVariables[i]);

            }

            builder.AppendLine();
            builder.AppendLine("[Variables]: ");
            for(int i = 0 ; i < Variables.Length ; i++){

                builder.AppendLine("    " + Variables[i]);

            }


            return builder.ToString();

        }

    }

}