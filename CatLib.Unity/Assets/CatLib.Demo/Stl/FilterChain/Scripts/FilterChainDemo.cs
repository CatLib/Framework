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

namespace CatLib.Demo.FilterChain
{
    /// <summary>
    /// 过滤器链Demo
    /// </summary>
    public class FilterChainDemo : IServiceProvider
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            App.On(ApplicationEvents.OnStartCompleted, (payload) =>
            {
                var filters = new FilterChain<string>();

                var isCall = true;
                filters.Add((data, next) =>
                {
                    if (isCall)
                    {
                        isCall = false;
                        filters.Do("sub", (d) => UnityEngine.Debug.Log("filter end , " + d));
                    }
                    UnityEngine.Debug.Log("hello this is filter chain 1 , " + data);
                    next(data);
                    UnityEngine.Debug.Log("back filter chain 1");
                });

                filters.Add((data, next) =>
                {
                    UnityEngine.Debug.Log("hello this is filter chain 2 , " + data);
                    next(data);
                    UnityEngine.Debug.Log("back filter chain 2");
                });

                filters.Do("hello world", (data) => UnityEngine.Debug.Log("filter end , " + data));
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