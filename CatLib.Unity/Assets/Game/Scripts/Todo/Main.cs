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

using CatLib.API.Routing;
using UnityEngine;

namespace CatLib.Bootstrap
{
    /// <summary>
    /// 用户代码入口
    /// </summary>
    [Routed]
    public class Main
    {
        [Routed("bootstrap://main")]
        public void Bootstrap()
        {
            //todo: user code here
            Debug.Log("hello world! user code here!");
        }
    }
}
