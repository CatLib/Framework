
using CatLib.API.Flux;

namespace CatLib.Flux
{

    /// <summary>
    /// 存储
    /// </summary>
    public class Store : FluxStore
    {

        protected Store(IFluxDispatcher dispatcher) : base(dispatcher)
        {

        }

        protected override INotification Notification()
        {
            return new Notification(NotificationName);
        }

    }

}