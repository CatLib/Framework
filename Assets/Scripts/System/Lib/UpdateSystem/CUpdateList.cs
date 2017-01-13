using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CatLib.UpdateSystem{

	public class CUpdateList {

		protected StreamReader sd;

		protected StreamWriter sw;

		protected string path;

		public CUpdateList(string path){

			this.path = path + "/list.update";

		}

		public CUpdateList Append(string path , string md5 , long size){

			return this.Append(new CUpdateListField(path ,md5 , size));

		}

		/// <summary>
		/// 写入文件
		/// </summary>
		public CUpdateList Append(CUpdateListField lst){

			if(this.sw == null){ this.sw = new StreamWriter(this.path , true , Encoding.UTF8); }
			this.sw.WriteLine(lst);
			return this;

		}

		/// <summary>
		/// 关闭文件
		/// </summary>
		public void Close(){

			if(this.sw != null){ 

				this.sw.Close(); 
				this.sw = null;

			}

		}

		~CUpdateList(){

			this.Close();

		}
		


	}

}
