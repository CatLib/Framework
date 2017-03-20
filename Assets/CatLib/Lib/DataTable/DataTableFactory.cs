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

	public class DataTableFactory : IDataTableFactory {

		public IDataTable Make(string[][] result){

			return new DataTable(result);

		}
		
	}

}