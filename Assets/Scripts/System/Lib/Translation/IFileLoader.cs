
namespace CatLib.Translation{

	public interface IFileLoader{


		IFileMapping Load(string root , string locale, string file);


	}

}