using CatLib.FileSystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace CatLib.UpdateSystem{

	public class CUpdateList : IEnumerable {

		protected string path;

        public const string FILE_NAME = "list.update";

        protected List<CUpdateListField> field = new List<CUpdateListField>();

        protected Dictionary<string, CUpdateListField> pkField = new Dictionary<string, CUpdateListField>();

        public CUpdateList(string path){

            this.SetPath(path);

		}

        public CUpdateList(UnityWebRequest request)
        {
            this.Parse(new UTF8Encoding(false).GetString(request.downloadHandler.data));
        }

        public CUpdateList SetPath(string path)
        {
            this.path = path + "/" + CUpdateList.FILE_NAME;
            return this;
        }

        public int Count(){

            return field.Count;

        }

        public IEnumerator GetEnumerator()
        {
            return (new List<CUpdateListField>(field.ToArray())).GetEnumerator();
        }

        public CUpdateList Read()
        {
            if (path.Exists())
            {
                using (StreamReader sr = new StreamReader(path, Encoding.UTF8 , true))
                {
                    this.Parse(sr.ReadToEnd());
                    sr.Close();
                }
            }
            return this;
        }

        public CUpdateList Append(string path , string md5 , long size){

			return this.Append(new CUpdateListField(path ,md5 , size));

		}
        
		public CUpdateList Append(CUpdateListField lst){

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
        public void Comparison(CUpdateList newLst , out CUpdateList needUpdate , out CUpdateList needDelete)
        {
            needUpdate = new CUpdateList(this.path);
            needDelete = new CUpdateList(string.Empty);
            CUpdateListField oldField;
            foreach (CUpdateListField newField in newLst)
            {
                oldField = this.FindPk(newField.Path);
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

            foreach(CUpdateListField old in this)
            {
                if(newLst.FindPk(old.Path) == null && old.Path != "/" + CUpdateList.FILE_NAME)
                {
                    needDelete.Append(old);
                }
            }
        }

        public CUpdateListField FindPk(string pk)
        {
            if (pkField.ContainsKey(pk))
            {
                return pkField[pk];
            }

            return null;

        }

        protected void Parse(string text)
        {
            string[] data = text.Replace("\r\n", "\n").Split('\n');
            foreach (string line in data)
            {
                if (line != string.Empty)
                {
                    this.Append(new CUpdateListField(line));
                }
            }
        }


	}

}
