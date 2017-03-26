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
using CatLib.API.FilterChain;

namespace CatLib.Demo.FilterChain
{

    public class FilterChainDemo : ServiceProvider
    {

        public override void Init()
        {
            App.On(ApplicationEvents.ON_APPLICATION_START_COMPLETE, (sender, e) =>
            {

                IFilterChain filterChain = App.Make<IFilterChain>();

                var filters = filterChain.Create<string>();

                filters.Add((data, next) =>
                {
                    UnityEngine.Debug.Log("hello this is filter chain 1 , " + data);
                    next.Do(data);
                    UnityEngine.Debug.Log("back filter chain 1");
                });

                filters.Add((data, next) =>
                {
                    UnityEngine.Debug.Log("hello this is filter chain 2 , " + data);
                    next.Do(data);
                    UnityEngine.Debug.Log("back filter chain 2");
                });

                filters.Then((data) =>
                {
                    UnityEngine.Debug.Log("filter end , " + data);
                });

                filters.Do("hello world");

            });
        }

        public override void Register(){ }
    }
}