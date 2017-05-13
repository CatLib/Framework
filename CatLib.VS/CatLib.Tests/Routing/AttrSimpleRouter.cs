using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CatLib.API.Routing;

namespace CatLib.Tests.Routing
{
    // 路由的最简使用方式
    // 如果类或者方法没有给定名字那么会自动使用类名或者方法名作为路由路径
    // 如下面的路由会被默认给定名字：attr-routing-simple/call 
    [Routed]
    public class AttrRoutingSimple
    {
        [Routed]
        public void Call(IRequest request, IResponse response)
        {
            response.SetContext("AttrRoutingSimple.Call");
        }

        //连续的大写会被视作一个整体最终的路由路径就是：catlib://attr-routing-simple/call-mtest
        [Routed]
        public void CallMTest(IRequest request, IResponse response)
        {
            response.SetContext("AttrRoutingSimple.CallMTest");
        }
    }
}
