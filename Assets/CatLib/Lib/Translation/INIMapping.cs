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
 
using CatLib.API.INI;

namespace CatLib.Translation{

	public class INIMapping : IFileMapping {

		private IINIResult result;

		public INIMapping(IINIResult result){

			this.result = result;

		}

		public string Get(string key, string def = null){
			
			return result.Get(string.Empty , key, def);

		}

	}


}