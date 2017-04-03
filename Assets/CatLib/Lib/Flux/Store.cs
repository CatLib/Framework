
namespace CatLib.Flux
{

    /// <summary>
    /// 存储
    /// </summary>
    public class Store : FluxStore
    {

        protected Store() : base(DefaultName, null)
        {

        }

        protected Store(string storeName) : base(storeName, null)
        {

        }

        protected Store(string storeName , object data) : base(storeName , data)
        {

        }

        protected override INotification Notification()
        {
            return new Notification(NotificationName, this);
        }

    }

}