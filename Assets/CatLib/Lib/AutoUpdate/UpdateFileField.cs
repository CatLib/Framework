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
 
using System;

namespace CatLib.AutoUpdate{

	public class UpdateFileField{

		public string MD5{ get; protected set; }

		public string Path{ get; protected set; }

		public long Size{ get; protected set; }

		public UpdateFileField(string str){

			string[] lst = str.Split('\t');

			if(lst.Length < 3){ throw new Exception("update data field data error"); }

			Path    = lst[0];
			MD5     = lst[1];
			Size    = long.Parse(lst[2]);

		}

		public UpdateFileField(string path , string md5 , long size){

			Path = path;
			MD5  = md5;
			Size = size;

		}

		public override string ToString(){
			
			return Path + "\t" + MD5 + "\t" + Size + "\n";

		}
	}

}