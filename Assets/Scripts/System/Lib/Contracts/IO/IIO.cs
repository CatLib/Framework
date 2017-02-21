
namespace CatLib.Contracts.IO
{
	public interface IIO{


		IFile File(string path);

		IDirectory Directory(string path);

		IDirectory DataPath{ get; }

		IDirectory AssetPath{ get; }

		IFile CreateFile(string path, byte[] array, int offset, int count);

	}

}