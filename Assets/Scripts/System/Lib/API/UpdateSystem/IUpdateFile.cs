
using System.Collections;

namespace CatLib.API.UpdateSystem{

	public interface IUpdateFile : IEnumerable
	{

		string Data{ get; }

		IUpdateFileField Find(string pk);

		IUpdateFile Append(string path , string md5 , long size);

		
		
		
	}


}