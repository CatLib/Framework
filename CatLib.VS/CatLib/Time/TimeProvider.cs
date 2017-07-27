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

#if CATLIB
using CatLib.API.Config;
using CatLib.API.Time;

namespace CatLib.Time
{
    /// <summary>
    /// 时间服务
    /// </summary>
    public sealed class TimeProvider : IServiceProvider
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// 注册时间服务
        /// </summary>
        public void Register()
        {
            RegisterTimeManager();
        }

        /// <summary>
        /// 注册时间服务管理器
        /// </summary>
        private void RegisterTimeManager()
        {
            App.Singleton<TimeManager>().Alias<ITimeManager>().Alias<ITime>().OnResolving((bind, obj) =>
            {
                var timeManager = obj as TimeManager;

                timeManager.Extend(() => new UnityTime());

                var config = App.Make<IConfigManager>();

                if (config != null)
                {
                    config.Default.Watch("time.default", (value) =>
                    {
                        timeManager.SetDefault(value.ToString());
                    });
                    timeManager.SetDefault(config.Default.Get("time.default", "default"));
                }

                return timeManager;
            });
        }
    }
}
#endif