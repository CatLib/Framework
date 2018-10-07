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
using System.Reflection;
using CatLib.API.Routing;
using CatLib.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatLib.Tests.Routing
{
    [TestClass]
    public class TestMiddlewareException
    {
        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void TestMiddlewareThrow()
        {
            var app = new Application();
            app.Bootstrap();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });
            app.Register(new RoutingProvider());
            var router = app.Make<IRouter>();
            router.Reg("test://throw", () =>
            {
                string str = null;
                var l = str.Length;
            });

            router.Dispatch("test://throw");
        }
    }
}
