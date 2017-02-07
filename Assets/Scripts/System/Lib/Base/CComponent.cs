using CatLib.Event;
using CatLib.Contracts.Event;
using CatLib.Contracts.Base;
using CapLib.Base;
using XLua;

namespace CatLib.Base
{

    public class CComponent : IEvent , IGuid
    {

        public IApplication Application { get { return CApp.Instance; } }


        private CEvent cevent = null;
        /// <summary>
        /// 事件系统
        /// </summary>
        public CEvent Event
        {
            get
            {
                if (this.cevent == null) { this.cevent = new CEvent(); }
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
                    guid = Application.GetGuid();
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
