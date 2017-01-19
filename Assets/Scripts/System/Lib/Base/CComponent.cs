using UnityEngine;
using System.Collections;
using CatLib.Event;

namespace CatLib.Base
{

    public class CComponent : IEvent
    {

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

    }

}
