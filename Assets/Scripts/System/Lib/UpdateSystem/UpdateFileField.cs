
using System;
using CatLib.API.UpdateSystem;

namespace CatLib.UpdateSystem{

	public class UpdateFileField : IUpdateFileField{

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

		public static implicit operator string(UpdateFileField cls){

			return cls.Path + "\t" + cls.MD5 + "\t" + cls.Size + "\n";

		}
	}

}