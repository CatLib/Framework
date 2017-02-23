
using System.Collections;
using System.Collections.Generic;
using CatLib.API.UpdateSystem;

namespace CatLib.UpdateSystem{

	public class UpdateFile : IUpdateFile{

		protected UpdateFileField[] fields;

		protected Dictionary<string, UpdateFileField> pkField = new Dictionary<string, UpdateFileField>();

		public UpdateFile(string text){

			Parse(text);

		}

		public IUpdateFileField Find(string pk)
        {
            if (pkField.ContainsKey(pk))
            {
                return pkField[pk];
            }

            return null;
        }


		public string Data{

			get{

				string returnStr = string.Empty;
				foreach(string str in this)
                {
                    returnStr += str;
                }
				return returnStr;
			}

		}

		public IUpdateFile Append(string path , string md5 , long size){
			
			return this.Append(new UpdateFileField(path ,md5 , size));

		}

		public IEnumerator GetEnumerator()
        {	

			if(fields == null){

				fields = new List<UpdateFileField>(pkField.Values).ToArray();

			}

            return fields.GetEnumerator();
        }

		protected IUpdateFile Append(UpdateFileField lst){

            this.pkField.Add(lst.Path, lst);
			fields = null;
            return this;

		}

		protected void Parse(string text)
        {
            string[] data = text.Replace("\r\n", "\n").Split('\n');
            foreach (string line in data)
            {
                if (line != string.Empty)
                {
                    this.Append(new UpdateFileField(line));
                }
            }
        }
		
	}

}