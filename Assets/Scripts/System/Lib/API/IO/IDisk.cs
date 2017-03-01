
using System.Collections;

namespace CatLib.API.IO{

	/// <summary>文件驱动</summary>
	public interface IDisk{

		IFile File(string path);

		IDirectory Directory(string path);

		IDirectory Root{ get; }
		
		void SetConfig(Hashtable config);

	}

}