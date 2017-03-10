using System;

namespace CatLib.API.Config{

	public interface IConfigStore{

		T Get<T>(string service, string field , T def = default(T));

		T Get<T>(Type service , string field , T def = default(T));

		object Get(string service , string field , object def);

		object Get(Type service , string field , object def);

		string Get(string service , string field , string def);

		string Get(Type service , string field , string def);

	}

}