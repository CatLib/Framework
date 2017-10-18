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

using System.Threading;

namespace CatLib.Socket.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// 等待完成
        /// </summary>
        /// <param name="wait"></param>
        /// <param name="maxWaitTimeMs"></param>
        /// <param name="sleep"></param>
        /// <returns></returns>
        public static bool Wait(IAwait wait, int maxWaitTimeMs, int sleep = 1)
        {
            var time = 0;
            while (!wait.IsDone && time < maxWaitTimeMs)
            {
                Thread.Sleep(sleep);
                time += sleep;
            }

            return wait.IsDone;
        }

        public static bool Wait<T>(ref T[] arr, int maxWaitTimeMs, int sleep = 1)
        {
            var time = 0;
            while (arr != null && arr.Length <= 0 && time < maxWaitTimeMs)
            {
                Thread.Sleep(sleep);
                time += sleep;
            }

            return arr != null && arr.Length > 0;
        }
    }
}
