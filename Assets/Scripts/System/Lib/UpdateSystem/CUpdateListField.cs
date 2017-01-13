
namespace CatLib.UpdateSystem{

	public class CUpdateListField{

		public string MD5{ get; protected set; }

		public string Path{ get; protected set; }

		public long Size{ get; protected set; }

		public CUpdateListField(string str){

			string[] lst = str.Split('\t');
			this.Path = lst[0];
			this.MD5 = lst[1];
			this.Size = long.Parse(lst[2]);

		}

		public CUpdateListField(string path , string md5 , long size){

			this.Path = path;
			this.MD5 = md5;
			this.Size = size;

		}

		public static implicit operator string(CUpdateListField cls){

			return cls.Path + "\t" + cls.MD5 + "\t" + cls.Size;

		}
	}

}