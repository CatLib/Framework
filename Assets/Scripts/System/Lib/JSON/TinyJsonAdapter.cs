
using TinyJson;

namespace CatLib.JSON{
	public class TinyJsonAdapter : IJSONAdapter {

		public T Decode<T>(string json){

			return JSONParser.FromJson<T>(json);

		}

		public string Encode(object item){

			return JSONWriter.ToJson(item);

		}

	}

}