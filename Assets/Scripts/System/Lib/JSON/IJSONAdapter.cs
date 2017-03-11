

namespace CatLib.Json{

	public interface IJsonAdapter{

		T Decode<T>(string json);

		string Encode(object item);

	}

}
