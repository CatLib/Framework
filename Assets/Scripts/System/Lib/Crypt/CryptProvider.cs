
using CatLib.API.Config;
using CatLib.API.Crypt;

namespace CatLib.Crypt{

	public class CryptProvider : ServiceProvider {

		public override void Register()
        {
            App.Singleton<Crypt>().Alias<ICrypt>().Resolving((app, bind , obj)=>{
                
                IConfigStore config = app.Make<IConfigStore>();
                Crypt crypt = obj as Crypt;

                crypt.SetKey(config.Get(typeof(Crypt) , "key" , null));

                return obj;
                
            });
        }
	}

}