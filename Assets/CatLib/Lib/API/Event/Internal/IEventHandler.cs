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