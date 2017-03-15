

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