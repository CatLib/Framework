
using System.Collections;
using System.IO;
using CatLib.API.IO;

namespace CatLib.IO{

	
	public class LocalDisk : IDisk{

        public IIOCrypt IOCrypt { get; set; }

        private string iocryptName;

        private string path;

		static readonly char[] INVALID_FILE_NAME_CHARS = new char[] { '/', '\\', '<', '>', ':', '|', '"' };

        public LocalDisk()
        {
            path = Env.AssetPath;
        }

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
                throw new IOException("a path can not be null or empty when searching the project");
            }

            if (path[path.Length - 1] == Path.AltDirectorySeparatorChar)
            {
                throw new IOException("all directory paths are expected to not end with a leading slash. ( i.e. the '" + Path.AltDirectorySeparatorChar + "' character )");
            }
        }

		public static string NormalizePath(string path){

            if(path[0] != Path.AltDirectorySeparatorChar){

                return Path.AltDirectorySeparatorChar + path;

            }
            return path;
            
        }

		public IFile File(string path , PathTypes pathType = PathTypes.Relative)
        {

            if (pathType == PathTypes.Absolute)
            {
                return new File(path, this);
            }else
            {
                return new File(this.path + Path.AltDirectorySeparatorChar + path.Trim(Path.AltDirectorySeparatorChar), this);
            }

		}

		public IDirectory Directory(string path , PathTypes pathType = PathTypes.Relative)
        {
            if (pathType == PathTypes.Absolute)
            {
                return new Directory(path, this);
            }
            else
            {
                return new Directory(this.path + Path.AltDirectorySeparatorChar + path.Trim(Path.AltDirectorySeparatorChar), this);
            }

        }

		public IDirectory Root{ 

			get{

				return new Directory(path, this);

			} 

		}

		public void SetConfig(Hashtable config){

            if(config.ContainsKey("crypt")){

                if (iocryptName != config["crypt"].ToString())
                {
                    iocryptName = config["crypt"].ToString();
                    IOCrypt = App.Instance.Make<IIOCrypt>(iocryptName);
                }

            }

            if (config.ContainsKey("root"))
            {
                path = config["root"].ToString();
            }

		}


	}

}