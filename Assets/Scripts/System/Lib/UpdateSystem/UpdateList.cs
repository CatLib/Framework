using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CatLib.API.IO;
using CatLib.API.UpdateSystem;
using UnityEngine.Networking;

namespace CatLib.UpdateSystem{

	public class UpdateList : IEnumerable  {

        [Dependency]
        public IIO IO{ get; set; }

		protected string path;

        public const string FILE_NAME = "list.update";

        public UpdateList SetData(UnityWebRequest request)
        {
            this.Parse(new UTF8Encoding(false).GetString(request.downloadHandler.data));
            return this;
        }

        public UpdateList SetData(string path)
        {
            this.path = path + IO.PathSpliter + UpdateList.FILE_NAME;
            return this;
        }

        public int Count(){ return field.Count; }

        public IEnumerator GetEnumerator()
        {
            return (new List<UpdateListField>(field.ToArray())).GetEnumerator();
        }

        public UpdateList Read()
        {
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path, Encoding.UTF8 , true))
                {
                    this.Parse(sr.ReadToEnd());
                    sr.Close();
                }
            }
            return this;
        }

        public UpdateList Append(string path , string md5 , long size){

			return this.Append(new UpdateListField(path ,md5 , size));

		}
        
		public UpdateList Append(UpdateListField lst){

            this.field.Add(lst);
            this.pkField.Add(lst.Path, lst);
            return this;

		}

        public void Save()
        {
            using (StreamWriter sw = new StreamWriter(this.path, false, new UTF8Encoding(false)))
            {
                foreach(string str in field)
                {
                    sw.WriteLine(str);
                }
            }
        }

        /// <summary>
        /// 和新的列表比对，筛选出需要更新的内容和需要删除的内容
        /// </summary>
        /// <param name="newLst"></param>
        /// <returns></returns>
        public void Comparison(UpdateList newLst , out UpdateList needUpdate , out UpdateList needDelete)
        {
            needUpdate = new UpdateList();
            needUpdate.SetData(this.path);

            needDelete = new UpdateList();
            needDelete.SetData(string.Empty);

            UpdateListField oldField;
            foreach (UpdateListField newField in newLst)
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

            foreach(UpdateListField old in this)
            {
                if(newLst.Find(old.Path) == null && old.Path != IO.PathSpliter + UpdateList.FILE_NAME)
                {
                    needDelete.Append(old);
                }
            }
        }

        

        protected void Parse(string text)
        {
            string[] data = text.Replace("\r\n", "\n").Split('\n');
            foreach (string line in data)
            {
                if (line != string.Empty)
                {
                    this.Append(new UpdateListField(line));
                }
            }
        }


	}

}
