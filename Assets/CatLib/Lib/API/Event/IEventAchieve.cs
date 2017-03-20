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