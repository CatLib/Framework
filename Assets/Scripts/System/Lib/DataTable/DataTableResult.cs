
using CatLib.API.DataTable;

namespace CatLib.DataTable{

	public class DataTableResult : IDataTableResult{

		private DataTable table;

		private string[] row;

		public DataTableResult(DataTable table , string[] row){

			this.table = table;
			this.row = row;

		}

		public string Get(string field){

			int index = table.GetIndex(field);
			if(index == -1){ return null; }
			return row[index];

		}

        public string this[string field]{ 
			
			get{

				return Get(field);

			} 

		}

	}

}