
namespace CatLib.Flux
{

    /// <summary>
    /// 存储
    /// </summary>
    public class Store : FluxStore
    {

        protected Store(string storeName) : base(storeName)
        {

        }

        protected override INotification Notification()
        {
            return new Notification(NotificationName, this);
        }

    }

}