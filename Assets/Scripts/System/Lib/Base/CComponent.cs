using CatLib.Contracts.Event;
using CatLib.Contracts.Base;
using CatLib.Base;
using CatLib.Container;

namespace CatLib.Base
{

    public class CComponent : IEvent , IGuid
    {

        public IApplication App { get { return CApp.Instance; } }


        private IEventAchieve cevent = null;
        /// <summary>
        /// 事件系统
        /// </summary>
        public IEventAchieve Event
        {
            get
            {
                if (this.cevent == null) { this.cevent = CApp.Instance.Make<IEventAchieve>(); }
                return this.cevent;
            }
        }

        private long guid;

        public long Guid
        {
            get
            {

                if (guid <= 0)
                {
                    guid = App.GetGuid();
                }
                return guid;
            }
        }
        public string TypeGuid
        {
            get
            {
                return GetType().ToString() + "-" + Guid;
            }
        }

    }

}
