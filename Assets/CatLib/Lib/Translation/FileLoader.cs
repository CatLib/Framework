using CatLib.API.INI;
using CatLib.API.IO;

namespace CatLib.Translation{

	public class FileLoader : IFileLoader{

		protected IDisk disk;

		protected IINILoader iniLoader;

		public FileLoader(IDisk disk , IINILoader iniLoader){

			this.disk = disk;
			this.iniLoader = iniLoader;

		}

		public IFileMapping Load(string root , string locale, string file){

			return LoadINIPath(root , locale , file);
			
		}

		protected IFileMapping LoadINIPath(string root , string locale, string file){

			IFile iniFile = disk.File(root + System.IO.Path.AltDirectorySeparatorChar + locale + System.IO.Path.AltDirectorySeparatorChar + file + ".ini");
			if(!iniFile.Exists){
				return null;
			}
			IINIResult result = iniLoader.Load(iniFile);
			return new INIMapping(result);

		}

	}

}