
using CatLib.Contracts.Secret;

namespace CatLib.Secret{

	public class SecretProvider : ServiceProvider {

		public override void Register()
        {
            App.Singleton<Secret>().Alias<ISecret>();
        }
	}

}