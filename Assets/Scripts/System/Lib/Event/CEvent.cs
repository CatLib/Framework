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
        public void CallEvent(int id)
        {
            this.CallEvent(id, null);
        }

        /// <summary>调用事件</summary>
        /// <param name="id">事件ID</param>
        public void CallEvent(System.Enum id)
        {
            this.CallEvent(id.ToInt(), null);
        }

        /// <summary>调用事件</summary>
        /// <param name="id">事件ID</param>
        /// <param name="dData">参数</param>
        public void CallEvent(System.Enum id, object dData)
        {
            this.CallEvent(id.ToInt(), dData);
        }

        /// <summary>调用事件</summary>
        /// <param name="id">事件ID</param>
        /// <param name="dData">参数</param>
        public void CallEvent(int id, object dData)
        {
            foreach (CEventData data in this._events.ToArray())
            {
                data.CallFunction(id, dData);
            }
        }

        /// <summary>注册事件</summary>
        /// <param name="id">事件ID</param>
        /// <param name="func">方法</param>
        public void RegEvent(int id, System.Action func)
        {
            this._events.Add(new CEventData(id, func));
        }

        /// <summary>注册事件</summary>
        /// <param name="id">事件ID</param>
        /// <param name="func">方法</param>
        public void RegEvent(System.Enum id, System.Action func)
        {
            this._events.Add(new CEventData(id.ToInt(), func));
        }

        /// <summary>注册事件</summary>
        /// <param name="id">事件ID</param>
        /// <param name="func">方法</param>
        public void RegEvent(int id, System.Action<object> func)
        {
            this._events.Add(new CEventData(id, func));
        }

        /// <summary>注册事件</summary>
        /// <param name="id">事件ID</param>
        /// <param name="func">方法</param>
        public void RegEvent(System.Enum id, System.Action<object> func)
        {
            this._events.Add(new CEventData(id.ToInt(), func));
        }

        /// <summary>移除事件</summary>
        /// <param name="id">事件ID</param>
        /// <param name="func">事件</param>
        public void RemoveEvent(System.Enum id, System.Action func)
        {
            this.RemoveEvent(id.ToInt(), func);
        }

        /// <summary>移除事件</summary>
        /// <param name="id">事件ID</param>
        /// <param name="func">事件</param>
        public void RemoveEvent(int id, System.Action func)
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
        public void RemoveEvent(int id)
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
        public void RemoveEvent(System.Enum id, System.Action<object> func)
        {
            this.RemoveEvent(id.ToInt(), func);
        }

        /// <summary>移除事件</summary>
        /// <param name="id">事件ID</param>
        /// <param name="func">事件</param>
        public void RemoveEvent(int id, System.Action<object> func)
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

        /// <summary>获取事件响应数量</summary>
        /// <param name="id">事件ID</param>
        public int GetEventNum(System.Enum id)
        {
            return this.GetEventNum(id.ToInt());
        }

        /// <summary>获取事件响应数量</summary>
        /// <param name="id">事件ID</param>
        public int GetEventNum(int id)
        {
            int num = 0;
            foreach (CEventData data in this._events.ToArray())
            {
                if (data.ID == id && (data.Event != null || data.EventObject != null))
                {
                    num++;
                }
            }
            return num;
        }

        /// <summary>移除全部注册到当前组件的事件</summary>
        public void RemoveAllEvent()
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
        public void RegEvent(IEvent cls, System.Enum actionID, System.Action action)
        {
            this.RegEvent(cls, actionID.ToInt(), action);
        }

        /// <summary>注册事件</summary>
        /// <param name="cls">需要注册的接口</param>
        /// <param name="actionID">动作ID</param>
        /// <param name="action">事件函数</param>
        public void RegEvent(IEvent cls, int actionID, System.Action action)
        {
            RegEventData data = new RegEventData(cls, actionID, action);
            data.Event.Event.RegEvent(data.ActionID, data.Action);
            this._regEventData.Add(data);
        }

        /// <summary>注册事件</summary>
        /// <param name="cls">需要注册的接口</param>
        /// <param name="actionID">枚举</param>
        /// <param name="action">事件函数</param>
        public void RegEvent(IEvent cls, System.Enum actionID, System.Action<object> action)
        {
            this.RegEvent(cls, actionID.ToInt(), action);
        }

        /// <summary>注册事件</summary>
        /// <param name="cls">需要注册的接口</param>
        /// <param name="actionID">动作ID</param>
        /// <param name="action">事件函数</param>
        public void RegEvent(IEvent cls, int actionID, System.Action<object> action)
        {
            RegEventData data = new RegEventData(cls, actionID, action);
            data.Event.Event.RegEvent(data.ActionID, data.ActionObject);
            this._regEventData.Add(data);
        }

        /// <summary>移除事件</summary>
        /// <param name="cls">需要移除注册的接口</param>
        /// <param name="actionID">枚举</param>
        /// <param name="action">事件函数</param>
        public void RemoveEvent(IEvent cls, System.Enum actionID, System.Action action)
        {
            this.RemoveEvent(cls, actionID.ToInt(), action);
        }

        /// <summary>移除事件</summary>
        /// <param name="cls">需要移除注册的接口</param>
        /// <param name="actionID">动作ID</param>
        /// <param name="action">事件函数</param>
        public void RemoveEvent(IEvent cls, int actionID, System.Action action)
        {
            foreach (RegEventData data in this._regEventData.ToArray())
            {
                if (data.Event == cls && data.ActionID == actionID && data.Action == action)
                {
                    data.Event.Event.RemoveEvent(data.ActionID, data.Action);
                }
            }
        }

        /// <summary>移除事件</summary>
        /// <param name="cls">需要移除注册的接口</param>
        /// <param name="actionID">枚举</param>
        /// <param name="action">事件函数</param>
        public void RemoveEvent(IEvent cls, System.Enum actionID, System.Action<object> action)
        {
            this.RemoveEvent(cls, actionID.ToInt(), action);
        }

        /// <summary>移除事件</summary>
        /// <param name="cls">需要移除注册的接口</param>
        /// <param name="actionID">动作ID</param>
        /// <param name="action">事件函数</param>
        public void RemoveEvent(IEvent cls, int actionID, System.Action<object> action)
        {
            foreach (RegEventData data in this._regEventData.ToArray())
            {
                if (data.Event == cls && data.ActionID == actionID && data.ActionObject == action)
                {
                    data.Event.Event.RemoveEvent(data.ActionID, data.ActionObject);
                }
            }
        }

        /// <summary>清除事件</summary>
        public void ClearEvent()
        {
            foreach (RegEventData data in this._regEventData) //不要ToArray()保证在遍历时不允许有其他的更改
            {
                data.Event.Event.RemoveEvent(data.ActionID, data.Action);
                data.Event.Event.RemoveEvent(data.ActionID, data.ActionObject);
            }
            this._regEventData.Clear();
        }

        #endregion

    }

}