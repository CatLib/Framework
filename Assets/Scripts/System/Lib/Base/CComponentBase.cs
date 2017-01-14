using UnityEngine;
using System.Collections;
using CatLib.Event;

namespace CatLib.Base
{

    /// <summary>
    /// 组件基类
    /// </summary>
    public class CComponentBase : MonoBehaviour , IEvent
    {


        protected Transform tran = null;
        /// <summary>
        /// Transform
        /// </summary>
        public Transform Transform { get { return this.tran; } }

        protected GameObject gameObj = null;
        /// <summary>
        /// GameObject
        /// </summary>
        public GameObject GameObject { get { return this.gameObj; } }

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

        public virtual void Awake()
        {
            this.tran = base.transform;
            this.gameObj = base.gameObject;
        }

        private bool isDestroying = false;
        /// <summary>
        /// 是否处于释放状态
        /// </summary>
        public bool IsDestroying
        {
            get { return this.isDestroying; }
            private set { this.isDestroying = value; }
        }

        /// <summary>
        /// 释放GameObject
        /// </summary>
        public virtual void Destroy()
        {
            this.IsDestroying = true;
            //todo: destroy
        }

        /// <summary>
        /// 当析构时
        /// </summary>
        public virtual void OnDestroy()
        {
            this.Event.ClearEvent();
        }

    }

}