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

using UnityEngine;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Assembly-CSharp-Editor") , 
           InternalsVisibleTo("Assembly-CSharp-Editor-firstpass")]

namespace CatLib
{
    /// <summary>
    /// 程序入口
    /// Program Entry
    /// </summary>
    public sealed class Program : MonoBehaviour
    {
        /// <summary>
        /// 初始化程序
        /// </summary>
        public void Awake()
        {
            new Application(this).Bootstrap(Bootstrap.BootStrap).Init();
        }
    }
}

