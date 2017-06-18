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

using CatLib.API.Translation;
using CatLib.Translation;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

namespace CatLib.Tests.Translation
{
    [TestClass]
    public class SelectorTests
    {
        [TestMethod]
        public void TestBaseSelect()
        {
            var selector = new Selector();
            Assert.AreEqual("hello this is test", selector.Choose("hello this is test", 10, Language.Chinese));
        }

        [TestMethod]
        public void TestRangSelect()
        {
            var selector = new Selector();
            Assert.AreEqual("world", selector.Choose("[*,9]hello|[10,20]world", 10, Language.Chinese));
            Assert.AreEqual("hello", selector.Choose("[*,9]hello|[10,20]world", 7, Language.Chinese));
            Assert.AreEqual("hello", selector.Choose("[*,9]hello|[10,20]world", -1, Language.Chinese));
            Assert.AreEqual("hello", selector.Choose("[*,9}hello|{10,20]world", 7, Language.Chinese));

            Assert.AreEqual("world", selector.Choose("[*,9]hello|[10,*]world", 30, Language.Chinese));
            Assert.AreEqual("hello", selector.Choose("[*,*]hello|[10,*]world", 30, Language.Chinese));

            Assert.AreEqual("hello", selector.Choose("{*}hello|[10,*]world", 30, Language.Chinese));
            Assert.AreEqual("hello", selector.Choose("[*]hello|[10,*]world", 30, Language.Chinese));

            Assert.AreEqual("catlib", selector.Choose("[*,9]hello|[10,20]world|{21,*}catlib", 30, Language.Chinese));

            //如果什么都没有则过滤区间范围使用复数规则
            Assert.AreEqual("hello", selector.Choose("[*,9]hello|[10,20]world", 30, Language.Chinese));
            Assert.AreEqual("hello", selector.Choose("[*,9]hello|[10,20]world|catlib", 30, Language.Chinese));
        }

        [TestMethod]
        public void TestNullEmptyLine()
        {
            var selector = new Selector();
            Assert.AreEqual(string.Empty, selector.Choose(null, 30, Language.Chinese));
            Assert.AreEqual(string.Empty, selector.Choose("", 30, Language.Chinese));
        }

        [TestMethod]
        public void TestErrorRange()
        {
            var selector = new Selector();
            Assert.AreEqual("world", selector.Choose("[*,9,10]hello|[10,20,3030]world", 10, Language.Chinese));
            Assert.AreEqual(string.Empty, selector.Choose("[*,9,10]|[19,20,3030]", 10, Language.Chinese));
            Assert.AreEqual("world", selector.Choose("[*,9,1hello|[10,20,3030]world", 10, Language.Chinese));
            Assert.AreEqual("[*,9,1hello", selector.Choose("[*,9,1hello|[10,20,3030]world", 2, Language.Chinese));
        }

        [TestMethod]
        public void TestPluralOfZero()
        {
            var selector = new Selector();

            var langs = new []
            {
                Language.Azerbaijani,
                Language.Tibetan,
                Language.Bhutani,
                Language.Indonesian,
                Language.Japanese,
                Language.Javanese,
                Language.Georgian,
                Language.Cambodian,
                Language.Kannada,
                Language.Korean,
                Language.Malay,
                Language.Thai,
                Language.Turkish,
                Language.Vietnamese,
                Language.Chinese,
                Language.ChineseTw,
            };

            foreach (var lang in langs)
            {
                Assert.AreEqual("hello", selector.Choose("[*,9]hello|[10,20]world", 30, lang));
                Assert.AreEqual("hello", selector.Choose("hello|world", 8, lang));
            }
        }

        [TestMethod]
        public void TestPluralMoreOne()
        {
            var selector = new Selector();

            var langs = new[]
            {
                Language.Afrikaans,
                Language.Bengali,
                Language.Bulgarian,
                Language.Catalan,
                Language.Danish,
                Language.German,
                Language.Greek,
                Language.English,
                Language.Esperanto,
                Language.Spanish,
                Language.Estonian,
                Language.Basque,
                Language.Farsi,
                Language.Finnish,
                Language.Faeroese,
                Language.Frisian,
                Language.Galician,
                Language.Gujarati,
                Language.Hausa,
                Language.Hebrew,
                Language.Hungarian,
                Language.Icelandic,
                Language.Italian,
                Language.Kurdish,
                Language.Malayalam,
                Language.Mongolian,
                Language.Marathi,
                Language.Nepali,
                Language.Dutch,
                Language.Norwegian,
                Language.Oromo,
                Language.Oriya,
                Language.Punjabi,
                Language.Pashto,
                Language.Portuguese,
                Language.Somali,
                Language.Albanian,
                Language.Swedish,
                Language.Swahili,
                Language.Tamil,
                Language.Telugu,
                Language.Turkmen,
                Language.Urdu,
                Language.Zulu,
            };

            foreach (var lang in langs)
            {
                Assert.AreEqual("world", selector.Choose("[*,9]hello|[10,20]world", 30, lang));
                Assert.AreEqual("world", selector.Choose("hello|world", -10, lang));
                Assert.AreEqual("hello", selector.Choose("hello|world", 1, lang));
            }
        }
    }
}
