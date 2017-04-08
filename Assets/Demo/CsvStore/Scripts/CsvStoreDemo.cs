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

using CatLib.API;
using CatLib.API.CsvStore;
using CatLib.API.DataTable;

namespace CatLib.Demo.CsvStore{

	public class CsvStoreDemo : ServiceProvider {

		public override void Init()
        {
            App.On(ApplicationEvents.ON_APPLICATION_START_COMPLETE, (sender, e) =>
            {
				
				ICsvStore csvStore = App.Make<ICsvStore>();

				// csv 文件需要遵守 rfc4180 规范才能被解析
				// csvStore仅仅是用作存储csv文件，这里的用法已经涉及到 datatable 组件
				// 开发者可用理解csv store 仅仅就是存储csv的容器
				IDataTable table = csvStore["items"];
		
				// 使用 datatable 的 active record 进行查询(更多datatable的用法参考datatable的demo)
				// (type == 2 || level < 3) && job == 2
				table.Where((selector)=>
                            {
                                selector.Where("type", "=", "2").OrWhere("level", "<", "3");
                            }).Where("job", "=", "2");

				foreach(var row in table.Get()){

					UnityEngine.Debug.Log(row["name"]);

				}

			});
		}

		public override void Register(){  }

	}

}