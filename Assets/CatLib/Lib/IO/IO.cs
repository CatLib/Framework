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
using System.Collections;
using CatLib.API.IO;
using CatLib.API.Container;

namespace CatLib.IO
{
    /// <summary>
    /// 文件服务
    /// </summary>
    public class IO : IIOFactory
    {
        /// <summary>
        /// 容器
        /// </summary>
        [Dependency]
        public IContainer Conatiner { get; set; }

        /// <summary>
        /// 配置查询器
        /// </summary>
        private Func<string, Hashtable> configSearch;

        /// <summary>
        /// 设定配置查询
        /// </summary>
        /// <param name="search">查询的配置</param>
        public void SetQuery(Func<string, Hashtable> search)
        {
            configSearch = search;
        }

        /// <summary>
        /// 获取磁盘驱动器
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>驱动器</returns>
        public IDisk Disk(string name = null)
        {
            var service = typeof(IDisk).ToString();

            Hashtable cloudConfig = null;
            if (configSearch != null)
            {
                cloudConfig = configSearch(name ?? service);
                if (cloudConfig != null && cloudConfig.ContainsKey("driver"))
                {
                    service = cloudConfig["driver"].ToString();
                }
            }

            var disk = Conatiner.Make<IDisk>(service);
            if (disk == null)
            {
                throw new Exception("undefind disk : " + name);
            }

            if (cloudConfig != null)
            {
                disk.SetConfig(cloudConfig);
            }

            return disk;
        }
    }
}