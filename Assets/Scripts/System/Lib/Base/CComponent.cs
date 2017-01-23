using UnityEngine;
using System.Collections;
using CatLib.Event;
using CatLib.Contracts.Event;
using CatLib.Container;

namespace CatLib.Base
{

    public class CComponent : IEvent
    {

        [CDependency]
        public CApplication Application { get; set; }


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
