
namespace CatLib.API.JSON{

	public interface IJSON {
		
		T Decode<T>(string json);

		string Encode(object item);

	}

}