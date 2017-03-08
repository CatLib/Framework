
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