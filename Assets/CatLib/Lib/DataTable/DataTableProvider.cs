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

namespace CatLib.DataTable
{
    /// <summary>
    /// Datatable服务提供商
    /// </summary>
    public sealed class DataTableProvider : ServiceProvider
    {
        /// <summary>
        /// 注册Datatable服务
        /// </summary>
        public override void Register()
        {
            App.Singleton<DataTableFactory>().Alias<IDataTableFactory>().Alias("datatable");
        }
    }
}