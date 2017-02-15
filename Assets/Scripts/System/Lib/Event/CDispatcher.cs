using UnityEngine;
using System.Collections;
using CatLib.Container;
using CatLib.Base;
using CatLib.Contracts.Event;
using CatLib.Contracts.Base;
using System.Threading;
using CatLib.Contracts.Base;
using System.Collections.Generic;
using System;

namespace CatLib.Event
{
    /// <summary>
    /// 全局事件调度器
    /// </summary>
    public class CDispatcher : CComponent , IDispatcher , IEventAchieve , IUpdate
    {

        //todo: 该类尚未完成
        
        private int mainThreadID;

        private Queue<object[]> actionQueue = new Queue<object[]>();

        private object actionLocker = new object();

        public CDispatcher(){

            mainThreadID = Thread.CurrentThread.ManagedThreadId;

        }

        public void Trigger(string eventName){

            Trigger(eventName , null, EventArgs.Empty);

        }

		public void Trigger(string eventName, EventArgs e){

            Trigger(eventName, null, e);

        }

		public void Trigger(string eventName, object sender){

            Trigger(eventName, sender, EventArgs.Empty);
            
        }

		public void Trigger(string eventName, object sender, EventArgs e){

            if(mainThreadID == Thread.CurrentThread.ManagedThreadId){

                base.Event.Trigger(eventName , sender , e);
                return ;
                
            }

            lock(actionLocker){

                actionQueue.Enqueue(new object[]{ "trigger" , eventName  , sender , e});

            }
            
        }

		public void On(string eventName, EventHandler handler){

            base.Event.On(eventName , handler);
            
        }

		public void One(string eventName , EventHandler handler){

            base.Event.One(eventName , handler);
            
        }

		public void Off(string eventName, EventHandler handler){

            base.Event.Off(eventName , handler);
            
        }

		public void OffOne(string eventName , EventHandler handler){

            base.Event.OffOne(eventName , handler);
            
        }

        public void Update(){

            lock(actionLocker){

                object[] data;
                while(actionQueue.Count > 0){

                    CallAction(actionQueue.Dequeue());

                }

            }

        }

        private void CallAction(object[] data){

            switch(data[0] as string){

                case "trigger": base.Event.Trigger(data[1] as string , data[2] , data[3] as EventArgs); break;
                default: break;

            }
 
        }


    }

}