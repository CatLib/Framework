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

using System.Collections;
using CatLib.API;
using CatLib.API.Config;
using CatLib.Config.Locator;
using UnityEngine;

namespace CatLib.Demo.Config
{
    /// <summary>
    /// Unity设置Demo
    /// </summary>
    public class UnitySettingDemo : ServiceProvider
    {
        public override IEnumerator Init()
        {
            App.On(ApplicationEvents.OnStartComplete, (sender, e) =>
            {
                var manager = App.Make<IConfigManager>();
                manager.Default.AddLocator(new UnitySettingLocator());

                manager.Default.Set("demokey", 100);
                Debug.Log(manager.Default.Get("demokey", 0));
                Debug.Log(manager.Default.Get("undefiend", 100));
            });
            return base.Init();
        }

        public override void Register()
        {
        }
    }
}