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
using UnityEngine;

namespace CatLib.Bootstrap
{
    /// <summary>
    /// 启动
    /// </summary>
    public class StartProvider : ServiceProvider
    {
        public override void Init()
        {
            App.On(ApplicationEvents.OnApplicationStartComplete, (sender , e) =>
            {
                Debug.Log("Application Start Complete.");
                //todo: write your code here
            });
        }

        public override void Register()
        {
        }
    }
}
