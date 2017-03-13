using CatLib.API.DataTable;

namespace CatLib.DataTable{

	public class DataTableProvider : ServiceProvider {

		public override void Register()
        {

            App.Singleton<DataTableFactory>().Alias<IDataTableFactory>().Alias("datatable");

        }

	}

}