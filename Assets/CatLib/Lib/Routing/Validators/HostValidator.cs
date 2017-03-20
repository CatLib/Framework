
using System.Text.RegularExpressions;

namespace CatLib.Routing{

	public class HostValidator : IValidators {

		/// <summary>
        /// 是否匹配
        /// </summary>
        /// <param name="route">路由条目</param>
        /// <param name="request">请求</param>
        /// <returns></returns>
        public bool Matches(Route route, Request request){

			if (string.IsNullOrEmpty(route.Compiled.HostRegex)) {
            	return true;
        	}

			return (new Regex(route.Compiled.HostRegex)).IsMatch(request.Host);

		}

	}

}
