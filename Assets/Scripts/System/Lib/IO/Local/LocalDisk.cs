
using System.Collections;
using System.IO;
using CatLib.API.IO;

namespace CatLib.IO{

	
	public class LocalDisk : IDisk{

        public IIOCrypt IOCrypt { get; set; }

		static readonly char[] INVALID_FILE_NAME_CHARS = new char[] { '/', '\\', '<', '>', ':', '|', '"' };

		public static bool IsValidFileName(string name)
        {
            for (int i = 0; i < INVALID_FILE_NAME_CHARS.Length; i++)
            {
                if (name.IndexOf(INVALID_FILE_NAME_CHARS[i]) != -1)
                {
                    return false;
                }
            }
            return true;
        }

		public static void ValidatePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new System.IO.IOException("a path can not be null or empty when searching the project");
            }

            if (path[path.Length - 1] == Path.AltDirectorySeparatorChar)
            {
                throw new System.IO.IOException("all directory paths are expected to not end with a leading slash. ( i.e. the '" + Path.AltDirectorySeparatorChar + "' character )");
            }
        }

		public static string NormalizePath(string path){

            if(path[0] != Path.AltDirectorySeparatorChar){

                return Path.AltDirectorySeparatorChar + path;

            }
            return path;
            
        }

		public IFile File(string path){

			return new File(path, this);

		}

		public IDirectory Directory(string path){

			return new Directory(path , this);

		}

		public IDirectory Root{ 

			get{

				return new Directory(Env.AssetPath , this);

			} 

		}

		public void SetConfig(Hashtable config){

            if(config.ContainsKey("crypt")){

                IOCrypt = App.Instance.Make<IIOCrypt>(config["crypt"].ToString());

            }

		}


	}

}