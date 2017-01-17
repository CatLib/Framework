/*
 * 作者：喵喵
 * QQ：917684105
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CatLib.Base;

namespace CatLib.Event
{
    /// <summary>事件机制</summary>
    public class CEvent
    {
 
        #region 基础事件机制

        /// <summary>事件</summary>
        private List<CEventData> _events = new List<CEventData>();

        /// <summary>调用事件</summary>
        /// <param name="id">事件ID</param>
        public void Trigger(int id)
        {
            this.Trigger(id, null);
        }

        /// <summary>调用事件</summary>
        /// <param name="id">事件ID</param>
        public void Trigger(System.Enum id)
        {
            this.Trigger(id.ToInt(), null);
        }

        /// <summary>调用事件</summary>
        /// <param name="id">事件ID</param>
        /// <param name="dData">参数</param>
        public void Trigger(System.Enum id, object dData)
        {
            this.Trigger(id.ToInt(), dData);
        }

        /// <summary>调用事件</summary>
        /// <param name="id">事件ID</param>
        /// <param name="dData">参数</param>
        public void Trigger(int id, object dData)
        {
            foreach (CEventData data in this._events.ToArray())
            {
                data.CallFunction(id, dData);
            }
        }

        /// <summary>注册事件</summary>
        /// <param name="id">事件ID</param>
        /// <param name="func">方法</param>
        public void On(int id, System.Action func)
        {
            this._events.Add(new CEventData(id, func));
        }

        /// <summary>注册事件</summary>
        /// <param name="id">事件ID</param>
        /// <param name="func">方法</param>
        public void On(System.Enum id, System.Action func)
        {
            this._events.Add(new CEventData(id.ToInt(), func));
        }

        /// <summary>注册事件</summary>
        /// <param name="id">事件ID</param>
        /// <param name="func">方法</param>
        public void On(int id, System.Action<object> func)
        {
            this._events.Add(new CEventData(id, func));
        }

        /// <summary>注册事件</summary>
        /// <param name="id">事件ID</param>
        /// <param name="func">方法</param>
        public void On(System.Enum id, System.Action<object> func)
        {
            this._events.Add(new CEventData(id.ToInt(), func));
        }

        /// <summary>移除事件</summary>
        /// <param name="id">事件ID</param>
        /// <param name="func">事件</param>
        public void Off(System.Enum id, System.Action func)
        {
            this.Off(id.ToInt(), func);
        }

        /// <summary>移除事件</summary>
        /// <param name="id">事件ID</param>
        /// <param name="func">事件</param>
        public void Off(int id, System.Action func)
        {
            if (func == null) { return; }
            foreach (CEventData data in this._events.ToArray())
            {
                if (data.Event == func && data.ID == id)
                {
                    this._events.Remove(data);
                }
            }
        }

        /// <summary>移除事件</summary>
        /// <param name="id">事件ID</param>
        public void Off(int id)
        {
            foreach (CEventData data in this._events.ToArray())
            {
                if (data.ID == id)
                {
                    this._events.Remove(data);
                }
            }
        }

        /// <summary>移除事件</summary>
        /// <param name="id">事件ID</param>
        /// <param name="func">事件</param>
        public void Off(System.Enum id, System.Action<object> func)
        {
            this.Off(id.ToInt(), func);
        }

        /// <summary>移除事件</summary>
        /// <param name="id">事件ID</param>
        /// <param name="func">事件</param>
        public void Off(int id, System.Action<object> func)
        {
            if (func == null) { return; }
            foreach (CEventData data in this._events.ToArray())
            {
                if (data.EventObject == func && data.ID == id)
                {
                    this._events.Remove(data);
                }
            }
        }

        /// <summary>移除全部注册到当前组件的事件</summary>
        public void Off()
        {
            this._events.Clear();
        }

        #endregion

        #region 事件注册释放自动机制

        /// <summary>框架注册委托数据</summary>
        private class RegEventData
        {
            /// <summary>注册的方法</summary>
            private System.Action _action = null;
            /// <summary>注册的方法</summary>
            public System.Action Action { get { return this._action; } private set { this._action = value; } }

            /// <summary>注册的方法</summary>
            private System.Action<object> _actionObject = null;
            /// <summary>注册的方法</summary>
            public System.Action<object> ActionObject { get { return this._actionObject; } private set { this._actionObject = value; } }

            /// <summary>注册的动作ID</summary>
            private int _actionID = 0;
            public int ActionID { get { return this._actionID; } private set { this._actionID = value; } }

            /// <summary>注册的接口</summary>
            private IEvent _event = null;
            /// <summary>注册的框架</summary>
            public IEvent Event { get { return this._event; } private set { this._event = value; } }

            /// <summary>框架注册委托数据</summary>
            /// <param name="cls">事件接口</param>
            /// <param name="actionID">动作ID</param>
            /// <param name="action">委托方法</param>
            public RegEventData(IEvent cls, int actionID, System.Action action)
            {
                this.Event = cls;
                this.ActionID = actionID;
                this.Action = action;
            }

            /// <summary>框架注册委托数据</summary>
            /// <param name="cls">事件接口</param>
            /// <param name="actionID">动作ID</param>
            /// <param name="action">委托方法</param>
            public RegEventData(IEvent cls, int actionID, System.Action<object> action)
            {
                this.Event = cls;
                this.ActionID = actionID;
                this.ActionObject = action;
            }
        }

        /// <summary>注册事件数据</summary>
        private List<RegEventData> _regEventData = new List<RegEventData>();

        /// <summary>注册事件</summary>
        /// <param name="cls">需要注册的接口</param>
        /// <param name="actionID">枚举</param>
        /// <param name="action">事件函数</param>
        public void On(IEvent cls, System.Enum actionID, System.Action action)
        {
            this.On(cls, actionID.ToInt(), action);
        }

        /// <summary>注册事件</summary>
        /// <param name="cls">需要注册的接口</param>
        /// <param name="actionID">动作ID</param>
        /// <param name="action">事件函数</param>
        public void On(IEvent cls, int actionID, System.Action action)
        {
            RegEventData data = new RegEventData(cls, actionID, action);
            data.Event.Event.On(data.ActionID, data.Action);
            this._regEventData.Add(data);
        }

        /// <summary>注册事件</summary>
        /// <param name="cls">需要注册的接口</param>
        /// <param name="actionID">枚举</param>
        /// <param name="action">事件函数</param>
        public void On(IEvent cls, System.Enum actionID, System.Action<object> action)
        {
            this.On(cls, actionID.ToInt(), action);
        }

        /// <summary>注册事件</summary>
        /// <param name="cls">需要注册的接口</param>
        /// <param name="actionID">动作ID</param>
        /// <param name="action">事件函数</param>
        public void On(IEvent cls, int actionID, System.Action<object> action)
        {
            RegEventData data = new RegEventData(cls, actionID, action);
            data.Event.Event.On(data.ActionID, data.ActionObject);
            this._regEventData.Add(data);
        }

        /// <summary>移除事件</summary>
        /// <param name="cls">需要移除注册的接口</param>
        /// <param name="actionID">枚举</param>
        /// <param name="action">事件函数</param>
        public void Off(IEvent cls, System.Enum actionID, System.Action action)
        {
            this.Off(cls, actionID.ToInt(), action);
        }

        /// <summary>移除事件</summary>
        /// <param name="cls">需要移除注册的接口</param>
        /// <param name="actionID">动作ID</param>
        /// <param name="action">事件函数</param>
        public void Off(IEvent cls, int actionID, System.Action action)
        {
            foreach (RegEventData data in this._regEventData.ToArray())
            {
                if (data.Event == cls && data.ActionID == actionID && data.Action == action)
                {
                    data.Event.Event.Off(data.ActionID, data.Action);
                }
            }
        }

        /// <summary>移除事件</summary>
        /// <param name="cls">需要移除注册的接口</param>
        /// <param name="actionID">枚举</param>
        /// <param name="action">事件函数</param>
        public void Off(IEvent cls, System.Enum actionID, System.Action<object> action)
        {
            this.Off(cls, actionID.ToInt(), action);
        }

        /// <summary>移除事件</summary>
        /// <param name="cls">需要移除注册的接口</param>
        /// <param name="actionID">动作ID</param>
        /// <param name="action">事件函数</param>
        public void Off(IEvent cls, int actionID, System.Action<object> action)
        {
            foreach (RegEventData data in this._regEventData.ToArray())
            {
                if (data.Event == cls && data.ActionID == actionID && data.ActionObject == action)
                {
                    data.Event.Event.Off(data.ActionID, data.ActionObject);
                }
            }
        }

        /// <summary>清除我注册到别人的事件</summary>
        public void ClearEvent()
        {
            foreach (RegEventData data in this._regEventData) //不要ToArray()保证在遍历时不允许有其他的更改
            {
                data.Event.Event.Off(data.ActionID, data.Action);
                data.Event.Event.Off(data.ActionID, data.ActionObject);
            }
            this._regEventData.Clear();
        }

        #endregion

    }

}