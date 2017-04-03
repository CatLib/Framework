
using CatLib.API.Flux;
using CatLib.Flux;
using UnityEngine.UI;

namespace CatLib.Demo.Flux
{

    public class DemoView : View
    {

        public Button button;
        public Text buttonText;

        private IFluxDispatcher dispatcher;
        private DemoStore store;

        public void Awake()
        {
            dispatcher = App.Instance.Make<IFluxDispatcher>();
            store = DemoStore.Get(dispatcher);

            button.onClick.AddListener(()=>
            {
                dispatcher.Dispatch(App.Instance.MakeParams<INotification>(DemoStore.ADD));
            });
        }

        /// <summary>
        /// 关注的容器
        /// </summary>
        protected override IStore[] Observer
        {
            get
            {
                return new IStore[] {

                        store

                    };
            }
        }

        protected override void OnChange(INotification notification)
        {
            UnityEngine.Debug.Log(notification.Action + " , count is: " + store.GetCount());
            buttonText.text = "Click To Add (" + store.GetCount() + ")";
        }

    }

}