
using System.Collections;
using System.Collections.Generic;

namespace CatLib.UpdateSystem{

	public class UpdateFile{
		
		protected UpdateFileField[] fields;

		protected UpdateFileField[] Fields{

			get{

				if(fields == null){

					fields = new List<UpdateFileField>(pkField.Values).ToArray();

				}
				return fields;

			}

		}

		protected Dictionary<string, UpdateFileField> pkField = new Dictionary<string, UpdateFileField>();

		public UpdateFile(){ }

		public UpdateFile(string text){

			Parse(text);

		}

		public void Parse(string text)
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

		public UpdateFileField Find(string pk)
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
				foreach(UpdateFileField field in this)
                {
                    returnStr += field.ToString();
                }
				return returnStr;
			}

		}

		public int Count{

			get{
				
				return pkField.Count;
			
			}

		}

		public UpdateFile Append(string path , string md5 , long size){
			
			return this.Append(new UpdateFileField(path ,md5 , size));

		}

		public UpdateFile Append(UpdateFileField lst){

            pkField.Add(lst.Path, lst);
			fields = null;
            return this;

		}

		public IEnumerator GetEnumerator()
        {	
            return Fields.GetEnumerator();
        }

		/// <summary>
        /// 和新的列表比对，筛选出需要更新的内容和需要删除的内容
        /// </summary>
        /// <param name="newLst"></param>
        /// <returns></returns>
        public void Comparison(UpdateFile newLst , out UpdateFile needUpdate , out UpdateFile needDelete)
        {

            needUpdate = new UpdateFile();
            needDelete = new UpdateFile();

            UpdateFileField oldField;
            foreach (UpdateFileField newField in newLst)
            {
                oldField = Find(newField.Path);
                if (oldField != null)
                {
                    if (oldField.MD5 != newField.MD5)
                    {
                        needUpdate.Append(newField);
                    }
                }else
                {
                    needUpdate.Append(newField);
                }   
            }

            foreach(UpdateFileField old in this)
            {
                if(newLst.Find(old.Path) == null && old.Path != IO.IO.PATH_SPLITTER + UpdateFileStore.FILE_NAME)
                {
                    needDelete.Append(old);
                }
            }
        }
		
	}

}