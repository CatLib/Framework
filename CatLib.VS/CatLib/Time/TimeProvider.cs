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

using CatLib.API.Time;

namespace CatLib.Time
{
    /// <summary>
    /// 时间服务
    /// </summary>
    public sealed class TimeProvider : ServiceProvider
    {
        /// <summary>
        /// 注册时间服务
        /// </summary>
        public override void Register()
        {
            RegisterTimeManager();
        }

        /// <summary>
        /// 注册时间服务管理器
        /// </summary>
        private void RegisterTimeManager()
        {
            App.Singleton<TimeManager>().Alias<ITimeManager>().OnResolving((bind, obj) =>
            {
                if (obj == null)
                {
                    return null;
                }

                var timeManager = obj as ITimeManager;
                timeManager.Extend(() => new UnityTime());

                return timeManager;
            });
        }
    }
}