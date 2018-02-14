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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatLib.Tests.Routing
{
    [Routed]
    public class AttrCompilerRouting
    {
        [Routed("routed://first-compiler-then-group/{str?}", Group = "DefaultGroup")]
        public void FirstCompilerThenAddGroup(IRequest request, IResponse response)
        {
            response.SetContext(request["str"]);
        }

        [Routed("routed://use-group-and-local-defaults/{str?}", Group = "DefaultGroup2", Defaults = "str=>hello world")]
        public void UseGroupAndLocalDefaults(IRequest request, IResponse response)
        {
            response.SetContext(request["str"]);
        }

        // 可变类型测试
        public class VariantType : IVariant
        {
            public string Value;
            public VariantType(int data)
            {
                Value = data < 100 ? "less 100" : "bigger 100";
            }
        }

        [Routed("routed://autoinject/{key}/{value}")]
        public string TestAutoInject(string key, VariantType value)
        {
            Assert.AreEqual("hello", key);
            return value.Value;
        }
    }
}
