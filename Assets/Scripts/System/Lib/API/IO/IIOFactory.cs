
namespace CatLib.API.IO
{
	public interface IIOFactory{

		IDisk Disk(string name = null);

	}

}