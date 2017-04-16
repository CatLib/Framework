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
using CatLib.API.IO;

namespace CatLib.Translation{

	public class FileLoader : IFileLoader{

		protected IDisk disk;

		protected IIniLoader iniLoader;

		public FileLoader(IDisk disk , IIniLoader iniLoader){

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
			IIniResult result = iniLoader.Load(iniFile);
			return new INIMapping(result);

		}

	}

}