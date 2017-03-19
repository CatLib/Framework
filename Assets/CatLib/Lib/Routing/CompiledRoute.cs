
using System.Collections.Generic;

namespace CatLib.Routing
{

    /// <summary>
    /// 编译后的路由信息
    /// </summary>
    public class CompiledRoute
    {

        /// <summary>
        /// uri
        /// </summary>
        private string uri;

        /// <summary>
        /// 可选字段
        /// </summary>
        private IEnumerable<string> optionals;

        /// <summary>
        /// 条件选择
        /// </summary>
        private IEnumerable<KeyValuePair<string, string>> wheres;

        /// <summary>
        /// 是否被编译过
        /// </summary>
        private bool isCompiled;

        /// <summary>
        /// 创建一个编译后的路由信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="optionals"></param>
        /// <param name="wheres"></param>
        public CompiledRoute(string uri, IEnumerable<string> optionals, IEnumerable<KeyValuePair<string, string>> wheres)
        {
            this.uri = uri;
            this.optionals = optionals;
            this.wheres = wheres;
            isCompiled = false;
        }

        /// <summary>
        /// 构建路由信息
        /// </summary>
        public CompiledRoute Compile()
        {


            //catlib://index-controller/{id}/{num}/{num?}
            // 假设where num: [0-9]*
            //catlib://index-controller/

            //compilePattern(uri, compilePattern



            return this;
        }


    }

}