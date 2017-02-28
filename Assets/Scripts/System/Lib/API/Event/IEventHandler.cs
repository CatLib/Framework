
namespace CatLib.API.Event{

	public interface IEventHandler {

		/// <summary>
		/// 取消注册的事件
		/// </summary>
		bool Cancel();
		
		/// <summary>
		/// 剩余调用次数
		/// </summary>
		int Life{ get; }

	}

}