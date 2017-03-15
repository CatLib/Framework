using System;

namespace CatLib.API.Event{
	
	public interface IEventAchieve {

		void Trigger(string eventName);

		void Trigger(string eventName, EventArgs e);

		void Trigger(string eventName, object sender);

		void Trigger(string eventName, object sender, EventArgs e);

		IEventHandler On(string eventName, System.EventHandler handler , int life = -1);

		IEventHandler One(string eventName , System.EventHandler handler);

		void Off(string eventName, IEventHandler handler);

	}

}