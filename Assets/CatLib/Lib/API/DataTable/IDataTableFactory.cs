
namespace CatLib.API.DataTable{

	public interface IDataTableFactory{

		IDataTable Make(string[][] result);

	}

}