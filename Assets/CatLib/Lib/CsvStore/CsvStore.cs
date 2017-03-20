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
 
using System;
using System.Collections.Generic;
using CatLib.API.Csv;
using CatLib.API.CsvStore;
using CatLib.API.DataTable;
using CatLib.API.IO;

namespace CatLib.CsvStore{

	/// <summary>
	/// CSV存储容器
	/// </summary>
	public class CsvStore : ICsvStore{

		private Dictionary<string , IDataTable> tables = new Dictionary<string , IDataTable>();

		private IDirectory directory;

		[Dependency]
		public ICsvParser Parser{ get; set; }

		[Dependency]
		public IDataTableFactory DataTableFactory{ get;set; }

		public void SetDirctory(IDirectory dir){

			directory = dir;

		}

		public IDataTable Get(string table){

			if(!tables.ContainsKey(table)){
				LoadCsvFile(table);
			}
			return tables[table];

		}

		public IDataTable this[string table]{ 
			
			get{

				return Get(table);

			} 

		}

		private void LoadCsvFile(string filename){

			if(directory == null){

				throw new ArgumentNullException("not set csv file directory");

			}

			string path = System.IO.Path.GetFileNameWithoutExtension(filename) + ".csv";

			IFile csvFile = directory.File(path);

			string[][] csvData = Parser.Parser(System.Text.Encoding.UTF8.GetString(csvFile.Read()));
			
			tables.Add( filename, DataTableFactory.Make(csvData) );
			
		}
		
	}

}
