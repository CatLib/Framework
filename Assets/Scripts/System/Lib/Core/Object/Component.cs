using CatLib.Contracts;
using CatLib.Contracts.Event;

namespace CatLib
{

    public class Component : IEvent , IGuid
    {

        public IApplication App { get { return CatLib.App.Instance; } }


        private IEventAchieve cevent = null;
        /// <summary>
        /// 事件系统
        /// </summary>
        public IEventAchieve Event
        {
            get
            {
                if (this.cevent == null) { this.cevent = CatLib.App.Instance.Make<IEventAchieve>(); }
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
