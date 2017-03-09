using CatLib.API.JSON;

namespace CatLib.JSON{

	public class JSONProvider : ServiceProvider {

		public override void Register()
		{

			RegisterParse();
			App.Singleton<JSON>().Alias<IJSON>().Alias("json");

		}

		protected void RegisterParse(){

			App.Singleton<IJSONAdapter>((app , param) => {

				return new TinyJsonAdapter();

			}).Alias("json.parse");

		}


	}

}
