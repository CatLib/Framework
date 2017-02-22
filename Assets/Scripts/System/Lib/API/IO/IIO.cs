
namespace CatLib.API.IO
{
	public interface IIO{


		IFile File(string path);

		IDirectory Directory(string path);

		IDirectory DataPath{ get; }

		IDirectory AssetPath{ get; }

	}

}