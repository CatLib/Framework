
using System.Collections;

namespace CatLib.API.IO{

	public interface ICloud{

		void Upload(string localFile);

		byte[] Get(string cloudPath);

		void SetConfig(Hashtable config);
		
	}

}