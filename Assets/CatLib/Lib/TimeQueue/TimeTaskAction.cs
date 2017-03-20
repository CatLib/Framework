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

namespace CatLib.TimeQueue{

	public class TimeTaskAction
    {

		public TimeTaskActionTypes Type{ get ; set; }
		
		public int[] IntArgs{ get; set; }

		public float[] FloatArgs { get; set; }

		public Func<bool> FuncBoolArg { get; set; }

	}

}