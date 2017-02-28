
using CatLib.API.Event;

namespace CatLib.Event{

	public class EventHandler : IEventHandler{
		
		protected IEventAchieve RegisterTarget{ get; set; }

		protected System.EventHandler Handler{ get; set; }

		protected string EventName{ get; set; }

		public int Life{ get; protected set; }
		
		public bool IsLife{ get{ return Life > 0 || Life < 0; } }

		private bool isCancel;

		//激活后表示已经进入事件列表
		private bool isActive;

		public EventHandler(IEventAchieve registerTarget , string eventName , System.EventHandler eventHandler, int life){

			Handler = eventHandler;
			RegisterTarget = registerTarget;
			EventName = eventName;
			Life = life;
			isCancel = false;
			isActive = false;
		}

		public bool Cancel(){

			if(isCancel){ return false; }
			if(!isActive){ return false; }
			if(RegisterTarget != null){

				RegisterTarget.Off(EventName , this);

			}
			isCancel = true;
			return true;
		}

		
		public void Active(){

			isActive = true;

		}

		public void Call(object sender, System.EventArgs e){
			
			if(Handler == null){ Life = 0; return; }
			Handler.Invoke(sender , e);
			if(Life > 0){ Life--; }

		}
	}

}