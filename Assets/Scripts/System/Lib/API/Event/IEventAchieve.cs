using System;

namespace CatLib.API.Event{
	
	public interface IEventAchieve {

		void Trigger(string eventName);

		void Trigger(string eventName, EventArgs e);

		void Trigger(string eventName, object sender);

		void Trigger(string eventName, object sender, EventArgs e);

		void On(string eventName, EventHandler handler);

		void One(string eventName , EventHandler handler);

		void Off(string eventName, EventHandler handler);

		void OffOne(string eventName , EventHandler handler);

	}

}