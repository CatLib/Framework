
using CatLib.API.Config;

namespace CatLib.Config{

	public class ConfigProvider : ServiceProvider {

		public override void Register()
		{

			App.Singleton<ConfigStore>().Alias<IConfigStore>().Alias("config");

		}

	}

}