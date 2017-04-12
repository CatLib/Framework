/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
 
using System;
using System.Collections.Generic;
using CatLib.API;

namespace CatLib{

	public class GlobalEvent : IGlobalEvent {

		private object source;

		private string eventName;

		private List<string> classInterface;

		private EventLevel eventLevel = EventLevel.All;

		public GlobalEvent(object source){

			this.source = source;

		}

		public IGlobalEvent SetEventName(string name){

			eventName = name;
			return this;

		}

		public IGlobalEvent AppendInterface<T>(){
			
			AppendInterface(typeof(T));
			return this;
		}
		public IGlobalEvent AppendInterface(Type t){

			if(classInterface == null){ classInterface = new List<string>(); }
			classInterface.Add(t.ToString());
			return this;
		}

		public IGlobalEvent SetEventLevel(EventLevel level){

			eventLevel = level;
			return this;

		}

		public void Trigger(EventArgs args = null){

			if(string.IsNullOrEmpty(eventName)){

				throw new RuntimeException("global event , event name can not be null");

			}

			if ((eventLevel & EventLevel.Self) > 0)
            {
                IGuid guid = source as IGuid;

                if (guid != null){

					App.Instance.Trigger(eventName + source.GetType() + guid.Guid, source, args);
				}
            }

            if (source != null && (eventLevel & EventLevel.Type) > 0)
            {
            	App.Instance.Trigger(eventName + source.GetType(), source, args);
            }

            if (classInterface != null && (eventLevel & EventLevel.Interface) > 0)
            {
                for (int i = 0; i < classInterface.Count; i++)
                {
                    App.Instance.Trigger(eventName + classInterface[i], source, args);
                }
            }

            if ((eventLevel & EventLevel.Global) > 0)
            {
                App.Instance.Trigger(eventName, source, args);
            }
			
		}
		
	}

}