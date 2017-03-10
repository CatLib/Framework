using System;

namespace CatLib.API.Config{

	public interface IConfigStore{

		T Get<T>(string name, string field , T def = default(T));

		T Get<T>(Type name , string field , T def = default(T));

		object Get(string name , string field , object def);

		object Get(Type name , string field , object def);

		string Get(string name , string field , string def);

		string Get(Type name , string field , string def);

	}

}