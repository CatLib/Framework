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

using CatLib.API.Environment;
using CatLib.Environment;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Environment
{
    [TestClass]
    public class EnvTests
    {   
        [TestMethod]
        public void TestSetDebugLevel()
        {
            var env = new UnityEnvironment();
            env.SetDebugLevel(DebugLevels.Prod);

            Assert.AreEqual(DebugLevels.Prod, env.DebugLevel);
        }
    }
}
