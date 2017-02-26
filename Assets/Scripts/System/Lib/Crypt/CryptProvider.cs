
using CatLib.API.Crypt;

namespace CatLib.Crypt{

	public class CryptProvider : ServiceProvider {

		public override void Register()
        {
            App.Singleton<Crypt>().Alias<ICrypt>();
        }
	}

}