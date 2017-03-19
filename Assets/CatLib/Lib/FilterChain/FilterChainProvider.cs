
using CatLib.API.FilterChain;

namespace CatLib.FilterChain
{
    public class FilterChainProvider : ServiceProvider
    {

        public override void Register()
        {

            App.Singleton<FilterChain>().Alias<IFilterChain>();

        }
    }

}