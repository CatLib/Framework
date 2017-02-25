
using CatLib.API.Hash;

namespace CatLib.Hash
{

    public class HashProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Singleton<Hash>().Alias<IHash>();
        }
    }

}