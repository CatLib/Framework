

namespace CatLib.JSON{

	public interface IJSONAdapter{

		T Decode<T>(string json);

		string Encode(object item);

	}

}
