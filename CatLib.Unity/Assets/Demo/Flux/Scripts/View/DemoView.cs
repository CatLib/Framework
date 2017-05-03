/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

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

        /// <summary>
        /// 关注的存储
        /// </summary>
        protected override IStore[] Observer
        {
            get
            {
                return new IStore[] { store };
            }
        }

        public void Awake()
        {
            dispatcher = App.Instance.Make<IFluxDispatcher>();
            store = DemoStore.Get(dispatcher);

            button.onClick.AddListener(()=>
            {
                DemoAction.AddCount();
            });
            Refresh();
        }

        protected override void OnChange(IAction action)
        {
            UnityEngine.Debug.Log(action.Action + " , count is: " + store.GetCount());
            buttonText.text = "Click To Add (" + store.GetCount() + ")";
        }

        protected void Refresh()
        {
            buttonText.text = "Click To Add (" + store.GetCount() + ")";
        }
    }

}