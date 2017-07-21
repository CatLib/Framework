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

using System;

namespace CatLib
{
    /// <summary>
    /// 时间
    /// </summary>
    public static class SystemTime
    {
        /// <summary>
        /// 将linux时间戳转为DateTime
        /// </summary>
        /// <param name="time">时间戳</param>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTime(int time)
        {
            var datetStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var toNow = new TimeSpan(long.Parse(time + "0000000"));
            return datetStart.Add(toNow);
        }

        /// <summary>
        /// 转为Linux时间戳
        /// </summary>
        /// <param name="time">DateTime</param>
        /// <returns>linux时间戳</returns>
        public static int Timestamp(this DateTime time)
        {
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
    }
}
