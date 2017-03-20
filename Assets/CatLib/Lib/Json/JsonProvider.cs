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
 
using CatLib.API.Json;

namespace CatLib.Json{

	public class JSONProvider : ServiceProvider {

		public override void Register()
		{

			RegisterParse();
			App.Singleton<Json>().Alias<IJson>().Alias("json");

		}

		protected void RegisterParse(){

			App.Singleton<IJsonAdapter>((app , param) => {

				return new TinyJsonAdapter();

			}).Alias("json.parse");

		}


	}

}
