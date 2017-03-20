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
 
using TinyJson;

namespace CatLib.Json{
	public class TinyJsonAdapter : IJsonAdapter {

		public T Decode<T>(string json){

			return JSONParser.FromJson<T>(json);

		}

		public string Encode(object item){

			return JSONWriter.ToJson(item);

		}

	}

}