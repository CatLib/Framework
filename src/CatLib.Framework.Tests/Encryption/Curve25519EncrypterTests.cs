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

using CatLib.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace CatLib.Tests.Encryption
{
    [TestClass]
    public class Curve25519EncrypterTests
    {
        [TestMethod]
        public void TestCurve25519Encrypter()
        {
            var alice = new Curve25519Encrypter();
            var pinker = new Curve25519Encrypter();

            var alicePublic = alice.Encrypt(null);
            var pinkerPublic = pinker.Encrypt(null);

            var pinkerSay = Encoding.Default.GetString(alice.Decrypt(pinkerPublic));
            var aliceSay = Encoding.Default.GetString(pinker.Decrypt(alicePublic));

            Assert.AreEqual(aliceSay, pinkerSay);
        }
    }
}
