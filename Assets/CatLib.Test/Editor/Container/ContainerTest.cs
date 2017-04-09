
using NUnit.Framework;
using CatLib.API.Container;

namespace CatLib.Test.Container
{

    [TestFixture]
    public class ContainerTest
    {

        [Test]
        public void BindFunc()
        {

            var container = MakeContainer();
            container.Bind("Test", (cont, param) =>
            {
                return "HelloWorld";
            }, true);

            IBindData bind = container.GetBind("Test");
            bool hasBind = container.HasBind("Test");
            object obj = container.Make("Test");

            Assert.AreNotEqual(null , bind);
            Assert.AreEqual(true , hasBind);
            Assert.AreEqual(true, bind.IsStatic);
            Assert.AreSame("HelloWorld", obj);
            

        }


        protected CatLib.Container.Container MakeContainer()
        {
            CatLib.Container.Container container = new CatLib.Container.Container();
            return container;
        }

    }

}