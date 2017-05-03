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

using FairyGUI;
using UnityEngine;

namespace CatLib.FairyGUI
{
    /// <summary>
    /// 加载器
    /// </summary>
    public class Loader : GLoader
    {
        protected override void LoadExternal()
        {
            Debug.Log(url);
            base.LoadExternal();
        }
    }
}