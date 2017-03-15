
using CatLib.API.Config;
using CatLib.API.Hash;

namespace CatLib.Hash
{

    public class HashProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Singleton<Hash>().Alias<IHash>().Resolving((app , bind, obj)=>{

                IConfigStore config = app.Make<IConfigStore>();
                Hash hash = obj as Hash;

                hash.SetFactor(config.Get<int>(typeof(Hash) , "factor" , 6));
                hash.SetGenerateSalt(config.Get(typeof(Hash) , "salt" , null));

                return obj;

            });
        }
    }

}