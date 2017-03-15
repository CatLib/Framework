
using CatLib.API.DataTable;

namespace CatLib.DataTable{

	public class DataTableFactory : IDataTableFactory {

		public IDataTable Make(string[][] result){

			return new DataTable(result);

		}
		
	}

}