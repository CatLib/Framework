﻿/*
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
using CatLib.API.LruCache;
using UnityEngine;

namespace CatLib.Demo.LruCache
{

    public class LruCacheDemo : ServiceProvider
    {

        public override void Init()
        {
            App.On(ApplicationEvents.ON_APPLICATION_START_COMPLETE, (sender, e) =>
            {

                ILruBuilder builder = App.Make<ILruBuilder>();

                ILruCache<string,string> cache = builder.Create<string, string>(3);

                cache.Add("key_1", "val_1");
                cache.Add("key_2", "val_2");
                cache.Add("key_3", "val_3");

                foreach(var kv in cache)
                {
                    Debug.Log(kv.Key + " , " + kv.Value);
                }

                var key = cache["key_2"];

                Debug.Log("key is :" + key);

                Debug.Log("*******************");

                foreach (var kv in cache)
                {
                    Debug.Log(kv.Key + " , " + kv.Value);
                }

                cache.Add("key_4", "key_4");

                Debug.Log("*******************");

                foreach (var kv in cache)
                {
                    Debug.Log(kv.Key + " , " + kv.Value);
                }

            });
        }

        public override void Register(){ }

    }

}