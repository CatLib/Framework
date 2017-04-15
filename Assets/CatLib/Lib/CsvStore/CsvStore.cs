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
using CatLib.API.Container;
using CatLib.API.Csv;
using CatLib.API.CsvStore;
using CatLib.API.DataTable;
using CatLib.API.IO;

namespace CatLib.CsvStore
{
    /// <summary>
    /// Csv存储容器
    /// </summary>
    public sealed class CsvStore : ICsvStore
    {
        /// <summary>
        /// Csv数据表
        /// </summary>
        private readonly Dictionary<string, IDataTable> tables = new Dictionary<string, IDataTable>();

        /// <summary>
        /// 文件夹
        /// </summary>
        private IDirectory directory;

        /// <summary>
        /// Csv解析器
        /// </summary>
        [Dependency]
        public ICsvParser Parser { get; set; }

        /// <summary>
        /// 容器
        /// </summary>
        [Dependency]
        public IContainer Container { get; set; }

        /// <summary>
        /// 设定Csv存储容器的文件夹
        /// </summary>
        /// <param name="dir">以这个文件夹作为根目录</param>
        public void SetDirctory(IDirectory dir)
        {
            directory = dir;
        }

        /// <summary>
        /// 获取一个数据表
        /// </summary>
        /// <param name="table">数据表表名</param>
        /// <returns>数据表</returns>
        public IDataTable Get(string table)
        {
            if (!tables.ContainsKey(table))
            {
                LoadCsvFile(table);
            }
            return tables[table];
        }

        /// <summary>
        /// 获取一个数据表
        /// </summary>
        /// <param name="table">数据表表名</param>
        /// <returns>数据表</returns>
        public IDataTable this[string table]
        {
            get
            {
                return Get(table);
            }
        }

        /// <summary>
        /// 加载Csv文件
        /// </summary>
        /// <param name="filename">文件名</param>
        private void LoadCsvFile(string filename)
        {
            if (directory == null)
            {
                throw new ArgumentNullException("directory", "not set csv file directory");
            }

            var path = System.IO.Path.GetFileNameWithoutExtension(filename) + ".csv";
            var csvFile = directory.File(path);
            var csvData = Parser.Parser(System.Text.Encoding.UTF8.GetString(csvFile.Read()));

            tables.Add(filename, Container.Make<IDataTable>().SetData(csvData));
        }
    }
}
