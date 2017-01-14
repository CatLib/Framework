/*
 * 作者：喵喵
 * QQ：917684105
 */
using UnityEngine;
using System.Collections;

namespace CatLib.Event
{
    /// <summary>事件数据</summary>
    public sealed class CEventData
    {

        private int id = 0;
        /// <summary>事件ID</summary>
        public int ID { get { return this.id; } }

        private System.Action aevent = null;
        /// <summary>事件</summary>
        public System.Action Event { get { return this.aevent; } }

        private System.Action<object> eventObject = null;
        /// <summary>事件</summary>
        public System.Action<object> EventObject { get { return this.eventObject; } }

        /// <summary>框架事件数据</summary>
        /// <param name="id">事件ID</param>
        /// <param name="events">事件调用方法</param>
        public CEventData(int id, System.Action events)
        {
            this.id = id;
            this.aevent = events;
        }

        /// <summary>框架事件数据</summary>
        /// <param name="id">事件ID</param>
        /// <param name="events">事件调用方法</param>
        public CEventData(int id, System.Action<object> events)
        {
            this.id = id;
            this.eventObject = events;
        }

        /// <summary>调用方法</summary>
        /// <param name="id">事件ID</param>
        public void CallFunction(int id)
        {
            this.CallFunction(id, null);
        }

        /// <summary>调用方法</summary>
        /// <param name="id">事件ID</param>
        /// <param name="data">数据</param>
        public void CallFunction(int id, object data)
        {
            if (id != this.id) { return; }
            if (this.aevent != null) { this.aevent(); }
            if (this.eventObject != null) { this.eventObject(data); }
        }

    }

}