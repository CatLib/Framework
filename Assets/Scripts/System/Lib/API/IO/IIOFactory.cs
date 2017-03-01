
namespace CatLib.API.IO
{
	public interface IIOFactory{

        IFile File(string path);

		IDirectory Directory(string path);

		IDirectory DataPath{ get; }

		IDirectory AssetPath{ get; }
		
		ICloud Cloud(string name);

		char PathSpliter{ get; }

	}

}