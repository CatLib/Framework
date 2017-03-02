
using System.Collections;

namespace CatLib.API.IO{

	/// <summary>文件驱动</summary>
	public interface IDisk{

		IFile File(string path, PathTypes type = PathTypes.Relative);

		IDirectory Directory(string path , PathTypes type = PathTypes.Relative);

		IDirectory Root{ get; }
		
		void SetConfig(Hashtable config);

	}

}