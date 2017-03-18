using System;
using System.Collections.Generic;
using CatLib.API;

namespace CatLib{
	public class GlobalEvent : IGlobalEvent {

		private object score;

		private string eventName;

		private List<string> classInterface;

		private EventLevel eventLevel = EventLevel.All;

		public GlobalEvent(object score){

			this.score = score;

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

				throw new RunTimeException("global event , event name can not be null");

			}

			if ((eventLevel & EventLevel.Self) > 0)
            {
				if(score != null){
					
					if(score is IGuid){
						App.Instance.Trigger(eventName + (score as IGuid).TypeGuid, score, args);
					}
				}
            }

            if ((eventLevel & EventLevel.Type) > 0)
            {
				if(score != null){
                	App.Instance.Trigger(eventName + score.GetType().ToString(), score, args);
				}
            }

            if ((eventLevel & EventLevel.Interface) > 0)
            {
				if(classInterface != null){

					for(int i = 0 ; i < classInterface.Count ; i++){

						App.Instance.Trigger(eventName + classInterface[i], score, args);

					}

				}  
            }

            if ((eventLevel & EventLevel.Global) > 0)
            {
                App.Instance.Trigger(eventName, score, args);
            }
			
		}
		
	}

}