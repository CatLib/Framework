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

namespace CatLib.API.CsvStore{

	
	/// <summary>
	/// Csv存储器接口
	/// </summary>
	public interface ICsvStore {

		/// <summary>
        /// 获取一个csv库
        /// </summary>
		IDataTable Get(string table);


		/// <summary>
        /// 获取csv库
        /// </summary>
		IDataTable this[string table]{ get; }

	}

}
