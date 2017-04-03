
using CatLib.API.Flux;
using CatLib.Flux;

namespace CatLib.Demo.Flux
{

    public class DemoAction
    {

        private static IFluxDispatcher dispatcher;

        public static IFluxDispatcher Dispatcher
        {
            get
            {
                if (dispatcher == null)
                {
                    dispatcher = App.Instance.Make<IFluxDispatcher>();
                }
                return dispatcher;
            }
        }

        public static void AddCount()
        {
            //Dispatcher.Dispatch(App.Instance.MakeParams<INotification>(DemoStore.ADD));
            Dispatcher.Dispatch(new Notification(DemoStore.ADD));
        }

    }

}