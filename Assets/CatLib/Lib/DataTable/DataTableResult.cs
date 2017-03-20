/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
 
using CatLib.API.DataTable;

namespace CatLib.DataTable{

	public class DataTableResult : IDataTableResult{

		private DataTable table;

		private string[] row;

        public string[] Row { get { return row; } }

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