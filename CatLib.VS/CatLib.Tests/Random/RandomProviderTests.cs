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

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using CatLib.API.Random;
using CatLib.Events;
using CatLib.Random;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Random
{
    [TestClass]
    public class RandomProviderTests
    {
        private IContainer MakeEnv()
        {
            var container = new Application();
            container.Bootstrap();
            container.Register(new EventsProvider());
            container.Register(new RandomProvider());
            container.Init();
            return container;
        }

        private void TestRandomNext(RandomTypes type)
        {
            var env = MakeEnv();
            //env.Make<IRandom>()
        }
    }
}
