
using System.Text.RegularExpressions;

namespace CatLib.Routing{

	public class UriValidator : IValidators {


		/// <summary>
        /// 是否匹配
        /// </summary>
        /// <param name="route">路由条目</param>
        /// <param name="request">请求</param>
        /// <returns></returns>
        public bool Matches(Route route, Request request){
			
			return (new Regex(route.Compiled.RouteRegex)).IsMatch(request.Uri);

		}

	}

}