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
using System.Text;
using CatLib.API.Encryption;
using CatLib.Encryption;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatLib.Tests.Encryption
{
    [TestClass]
    public class EncryptionProviderTests
    {
        public IApplication MakeEnv(Action<EncryptionProvider> then = null)
        {
            var app = new Application();
            app.Bootstrap();
            var cls = new EncryptionProvider();
            app.Register(cls);

            if (then != null)
            {
                then.Invoke(cls);
            }
            app.Init();
            return app;
        }

        [TestMethod]
        public void TestDefaultEncryption()
        {
            var app = MakeEnv((cls) =>
            {
                cls.Key = "0123456789123456";
            });

            var encrypter = app.Make<IEncrypter>();

            var code = encrypter.Encrypt(Encoding.Default.GetBytes("helloworld"));
            Assert.AreNotEqual("helloworld", code);
            Assert.AreEqual("helloworld", Encoding.Default.GetString(encrypter.Decrypt(code)));
        }

        [TestMethod]
        public void TestWithFile()
        {
            var app = MakeEnv((cls) =>
            {
                cls.Key = "01234567891234560123456789123456";
                cls.Cipher = "AES-256-CBC";
            });

            var encrypter = App.Make<IEncrypter>();

            if (File.Exists("hello.txt"))
            {
                File.Delete("hello.txt");
            }
            if (File.Exists("enhello.txt"))
            {
                File.Delete("enhello.txt");
            }

            File.WriteAllText("hello.txt", "helloworld", Encoding.ASCII);
            var datastr = File.ReadAllText("hello.txt", Encoding.ASCII);
            var data = Encoding.ASCII.GetBytes(datastr);
            var endata = encrypter.Encrypt(data);
            File.WriteAllText("enhello.txt", endata, Encoding.ASCII);

            var endatastr = File.ReadAllText("enhello.txt", Encoding.ASCII);
            data = encrypter.Decrypt(endatastr);

            Assert.AreEqual("helloworld", Encoding.ASCII.GetString(data));
        }

        [TestMethod]
        public void Test256Encryption()
        {
            var app = MakeEnv((cls) =>
            {
                cls.Key = "01234567891234560123456789123456";
                cls.Cipher = "AES-256-CBC";
            });

            var encrypter = app.Make<IEncrypter>();

            var code = encrypter.Encrypt(Encoding.Default.GetBytes("helloworld"));
            Assert.AreNotEqual("helloworld", code);
            Assert.AreEqual("helloworld", Encoding.Default.GetString(encrypter.Decrypt(code)));
        }

        [TestMethod]
        public void TestEncryptionFaild()
        {
            var app = MakeEnv((cls) =>
            {
                cls.Key = "0123456789123456";
            });

            var encrypter = app.Make<IEncrypter>();

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                encrypter.Decrypt(null);
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                encrypter.Decrypt("");
            });

            ExceptionAssert.Throws<EncryptionException>(() =>
            {
                encrypter.Decrypt("123213");
            });

            ExceptionAssert.Throws<EncryptionException>(() =>
            {
                encrypter.Decrypt("123213:123123,123:123,213:123:123");
            });
        }

        [TestMethod]
        public void TestEmptyKey()
        {
            var app = MakeEnv();
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                var encrypter = app.Make<IEncrypter>();
                encrypter.Encrypt(Encoding.Default.GetBytes("hello world"));
            });
        }

        [TestMethod]
        public void TestNotSupportedKey()
        {
            var app = MakeEnv((cls) =>
            {
                cls.Key = "01234567891";
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                var encrypter = app.Make<IEncrypter>();
                encrypter.Encrypt(Encoding.Default.GetBytes("hello world"));
            });
        }

        [TestMethod]
        public void TestModifyData()
        {
            var app = MakeEnv((cls) =>
            {
                cls.Key = "0123456789123456";
            });

            var encrypter = app.Make<IEncrypter>();

            var code = encrypter.Encrypt(Encoding.Default.GetBytes("helloworld"));
            code = code.Replace("7", "8");
            code = code.Replace("5", "6");
            code = code.Replace("3", "4");
            code = code.Replace("1", "2");
            code = code.Replace("z", "0");
            code = code.Replace("x", "y");
            code = code.Replace("v", "w");
            code = code.Replace("t", "u");
            code = code.Replace("r", "s");
            code = code.Replace("p", "q");
            code = code.Replace("n", "o");
            code = code.Replace("k", "l");
            code = code.Replace("i", "j");
            code = code.Replace("g", "h");
            code = code.Replace("e", "f");
            code = code.Replace("c", "D");
            code = code.Replace("a", "B");
            code = code.Replace("M", "a");
            ExceptionAssert.Throws<EncryptionException>(() =>
            {
                encrypter.Decrypt(code);
            });
        }

        [TestMethod]
        public void TestEmptyByteData()
        {
            var app = MakeEnv((cls) =>
            {
                cls.Key = "0123456789123456";
            });

            var encrypter = app.Make<IEncrypter>();

            var code = encrypter.Encrypt(new byte[] { });
            Assert.AreEqual(0, encrypter.Decrypt(code).Length);
        }

        [TestMethod]
        public void TestDHExchange()
        {
            var app = MakeEnv((cls) =>
            {
                cls.Key = "0123456789123456";
            });

            var encrypter = app.Make<IEncryptionManager>();

            byte[] a, b = null;
            a = encrypter.ExchangeSecret((publicKey) =>
            {
                var key = new byte[]{};
                b = encrypter.ExchangeSecret((pubKey) =>
                {
                    key = pubKey;
                    return publicKey;
                });
                return key;
            });

            var aKey = Encoding.Default.GetString(a);
            var bKey = Encoding.Default.GetString(b);

            Assert.AreEqual(aKey, bKey);
        }
    }
}
