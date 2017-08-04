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

            App.Singleton<Test>();
            App.Make<Test>();

            Router.Instance.Reg("bootstrap://1", (req, res) => { });
            Router.Instance.Reg("bootstrap://2", (req, res) => { });
            Router.Instance.Reg("bootstrap://3", (req, res) => { });
            Router.Instance.Reg("bootstrap://4", (req, res) => { });
            Router.Instance.Reg("bootstrap://5", (req, res) => { });
            Router.Instance.Reg("bootstrap://6", (req, res) => { });
            Router.Instance.Reg("bootstrap://7", (req, res) => { });
            Router.Instance.Reg("bootstrap://8", (req, res) => { });
            Router.Instance.Reg("bootstrap://9", (req, res) => { });
            Router.Instance.Reg("bootstrap://10", (req, res) => { });
            Router.Instance.Reg("bootstrap://11", (req, res) => { });
            Router.Instance.Reg("bootstrap://12", (req, res) => { });
            Router.Instance.Reg("bootstrap://13", (req, res) => { });
            Router.Instance.Reg("bootstrap://14", (req, res) => { });
            Router.Instance.Reg("bootstrap://15", (req, res) => { });
            Router.Instance.Reg("bootstrap://16", (req, res) => { });
            Router.Instance.Reg("bootstrap://17", (req, res) => { });
            Router.Instance.Reg("bootstrap://18", (req, res) => { });
            Router.Instance.Reg("bootstrap://19", (req, res) => { });
            Router.Instance.Reg("bootstrap://20", (req, res) => { });
            Router.Instance.Reg("bootstrap://21", (req, res) => { });
            Router.Instance.Reg("bootstrap://22", (req, res) => { });
            Router.Instance.Reg("bootstrap://23", (req, res) => { });
            Router.Instance.Reg("bootstrap://24", (req, res) => { });
            Router.Instance.Reg("bootstrap://25", (req, res) => { });
            Router.Instance.Reg("bootstrap://26", (req, res) => { });
            Router.Instance.Reg("bootstrap://27", (req, res) => { });
            Router.Instance.Reg("bootstrap://28", (req, res) => { });
            Router.Instance.Reg("bootstrap://29", (req, res) => { });
            Router.Instance.Reg("bootstrap://30", (req, res) => { });
            Router.Instance.Reg("bootstrap://31", (req, res) => { });
            Router.Instance.Reg("bootstrap://32", (req, res) => { });
            Router.Instance.Reg("bootstrap://33", (req, res) => { });
            Router.Instance.Reg("bootstrap://34", (req, res) => { });
            Router.Instance.Reg("bootstrap://35", (req, res) => { });
            Router.Instance.Reg("bootstrap://36", (req, res) => { });
            Router.Instance.Reg("bootstrap://37", (req, res) => { });
            Router.Instance.Reg("bootstrap://38", (req, res) => { });
            Router.Instance.Reg("bootstrap://39", (req, res) => { });
            Router.Instance.Reg("bootstrap://40", (req, res) => { });
            Router.Instance.Reg("bootstrap://41", (req, res) => { });
            Router.Instance.Reg("bootstrap://42", (req, res) => { });
            Router.Instance.Reg("bootstrap://43", (req, res) => { });
            Router.Instance.Reg("bootstrap://44", (req, res) => { });
            Router.Instance.Reg("bootstrap://45", (req, res) => { });
            Router.Instance.Reg("bootstrap://46", (req, res) => { });
            Router.Instance.Reg("bootstrap://47", (req, res) => { });
            Router.Instance.Reg("bootstrap://48", (req, res) => { });
            Router.Instance.Reg("bootstrap://49", (req, res) => { });
            Router.Instance.Reg("bootstrap://50", (req, res) => { });
        }

        [Routed("bootstrap://test")]
        public void Test()
        {
            
        }
    }

    public class Test: IUpdate
    {
        public void Update()
        {
            Router.Instance.Dispatch("bootstrap://50");
            Router.Instance.Dispatch("bootstrap://50");
            Router.Instance.Dispatch("bootstrap://50");
            Router.Instance.Dispatch("bootstrap://50");
            Router.Instance.Dispatch("bootstrap://50");
            Router.Instance.Dispatch("bootstrap://50");
            Router.Instance.Dispatch("bootstrap://50");
            Router.Instance.Dispatch("bootstrap://50");
            Router.Instance.Dispatch("bootstrap://50");
            Router.Instance.Dispatch("bootstrap://50");
        }
    }
}