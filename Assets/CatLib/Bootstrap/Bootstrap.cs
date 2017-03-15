
using System;

namespace CatLib{

	public class Bootstrap{

		public static Type[] BootStrap
		{
			get
			{
				return new Type[]
				{
					typeof(RegisterProvidersBootstrap)
				};
			}

		}

	}

}