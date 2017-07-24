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

namespace CatLib.Bootstrap
{
    /// <summary>
    /// 类型扫描器引导
    /// </summary>
    public class TypeFinderBootstrap : IBootstrap
    {
        /// <summary>
        /// 引导程序
        /// </summary>
        public void Bootstrap()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                int sort;
                if (Assemblys.Assembly.TryGetValue(assembly.GetName().Name, out sort))
                {
                    App.OnFindType((finder) =>
                    {
                        return assembly.GetType(finder);
                    }, sort);
                }
            }
        }
    }
}