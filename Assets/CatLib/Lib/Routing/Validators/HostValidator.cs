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
