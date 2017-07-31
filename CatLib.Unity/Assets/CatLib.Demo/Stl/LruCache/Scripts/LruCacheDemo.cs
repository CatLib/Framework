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

namespace CatLib.Demo.LruCache
{
    /// <summary>
    /// LruCache Demo
    /// </summary>
    public class LruCacheDemo : IServiceProvider
    {
        /// <summary>
        /// 初始化服务提供者
        /// </summary>
        public void Init()
        {
            App.On(ApplicationEvents.OnStartCompleted, (payload) =>
            {
                var cache = new LruCache<string, string>(3);

                cache.Add("key_1", "val_1");
                cache.Add("key_2", "val_2");
                cache.Add("key_3", "val_3");

                foreach (var kv in cache)
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

        /// <summary>
        /// 注册服务提供者
        /// </summary>
        public void Register()
        {
        }
    }
}