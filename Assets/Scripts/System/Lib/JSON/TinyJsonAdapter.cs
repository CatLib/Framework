
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