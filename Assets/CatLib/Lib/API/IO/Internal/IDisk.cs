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
 
using System.Collections;

namespace CatLib.API.IO{

	/// <summary>文件驱动</summary>
	public interface IDisk{

		IFile File(string path, PathTypes type = PathTypes.Relative);

		IDirectory Directory(string path , PathTypes type = PathTypes.Relative);

		IDirectory Root{ get; }
		
		bool IsCrypt{ get; }
		
		void SetConfig(Hashtable config);

	}

}