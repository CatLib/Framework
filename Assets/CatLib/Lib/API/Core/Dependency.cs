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

namespace CatLib
{
    /// <summary>
    /// 依赖标记
    /// </summary>
    public class Dependency : Attribute
    {

        public string Alias { get; protected set; } 

        public Dependency(string alias)
        {
            Alias = alias;
        }

        public Dependency() { }

    }
}