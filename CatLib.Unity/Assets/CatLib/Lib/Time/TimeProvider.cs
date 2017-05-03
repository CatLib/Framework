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
    public class TimeProvider : ServiceProvider
    {
        /// <summary>
        /// 注册时间服务
        /// </summary>
        public override void Register()
        {
            App.Singleton<TimeSystem>().Alias<ITime>();
        }
    }
}