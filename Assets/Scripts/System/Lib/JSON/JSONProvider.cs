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
