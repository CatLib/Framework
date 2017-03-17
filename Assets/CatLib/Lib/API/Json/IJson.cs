
namespace CatLib.API.Json{

	public interface IJson {
		
		T Decode<T>(string json);

		string Encode(object item);

	}

}