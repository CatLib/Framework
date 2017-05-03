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
using CatLib.API.Resources;
using UnityEngine;

namespace CatLib.Demo.Flux
{

    public class FluxDemo : ServiceProvider
    {

        public override void Init()
        {
            App.On(ApplicationEvents.ON_APPLICATION_START_COMPLETE, (sender, e) =>
            {
                var go = App.Make<IResources>().Load<GameObject>("flux", LoadTypes.Resources);
                go.Instantiate();
            });
        }

        public override void Register() { }

    }

}