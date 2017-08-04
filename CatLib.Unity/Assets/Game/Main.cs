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

using CatLib;
using CatLib.API.Routing;
using CatLib.Facade;
using UnityEngine;

namespace YourNameSpace
{
    /// <summary>
    /// 用户代码入口
    /// </summary>
    [Routed]
    public class Main
    {
        [Routed("bootstrap://config")]
        public void Config()
        {
            //可以在这里完成常规配置（如果有的话）。
            Debug.Log("config code here!");
        }

        [Routed("bootstrap://start")]
        public void Bootstrap()
        {
            //todo: user code here
            Debug.Log("hello world! user code here!");
            GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
    }
}