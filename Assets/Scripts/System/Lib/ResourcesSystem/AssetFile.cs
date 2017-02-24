using System.Collections;
using System.Collections.Generic;

namespace CatLib.ResourcesSystem
{

    public class AssetFile
    {

        protected AssetFileField[] fields;

        protected AssetFileField[] Fields
        {

            get
            {

                if (fields == null)
                {

                    fields = new List<AssetFileField>(pkField.Values).ToArray();

                }
                return fields;

            }

        }

        protected Dictionary<string, AssetFileField> pkField = new Dictionary<string, AssetFileField>();

        public AssetFile() { }

        public AssetFile(string text)
        {

            Parse(text);

        }

        public void Parse(string text)
        {
            string[] data = text.Replace("\r\n", "\n").Split('\n');
            foreach (string line in data)
            {
                if (line != string.Empty)
                {
                    Append(new AssetFileField(line));
                }
            }
        }

        public AssetFileField Find(string pk)
        {
            if (pkField.ContainsKey(pk))
            {
                return pkField[pk];
            }

            return null;
        }


        public string Data
        {

            get
            {

                string returnStr = string.Empty;
                foreach (AssetFileField field in this)
                {
                    returnStr += field.ToString();
                }
                return returnStr;
            }

        }

        public int Count
        {

            get
            {

                return pkField.Count;

            }

        }

        public AssetFile Append(string assetBundle, bool isEncryption)
        {

            return Append(new AssetFileField(assetBundle, isEncryption));

        }

        public AssetFile Append(AssetFileField lst)
        {
            pkField.Add(lst.AssetBundle, lst);
            fields = null;
            return this;
        }

        public IEnumerator GetEnumerator()
        {
            return Fields.GetEnumerator();
        }

    }


}