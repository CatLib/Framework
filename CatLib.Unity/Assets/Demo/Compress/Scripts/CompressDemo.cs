/*
 * This file is part of the CatLib package.
 *
 * (c) Lrving <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System.Text;
using CatLib.API;
using CatLib.API.Compress;
using CatLib.API.IO;

namespace CatLib.Demo.Compress
{

    public class CompressDemo : ServiceProvider
    {

        public override void Init()
        {
            App.On(ApplicationEvents.OnApplicationStartComplete, (sender, e) =>
            {
                string str = @"helloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworld
                                helloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworld
                                helloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworld
                                helloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworld
                                helloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworld";

                UnityEngine.Debug.Log("Compress String before: " + str);
                UnityEngine.Debug.Log("Compress Length before: " + Encoding.UTF8.GetBytes(str).Length);

                ICompress comp = App.Make<ICompress>();
                byte[] byt = comp.Compress(Encoding.UTF8.GetBytes(str));

                UnityEngine.Debug.Log("Compress Length: " + byt.Length);

                byt = comp.UnCompress(byt);
                UnityEngine.Debug.Log("UnCompress String" +  System.Text.Encoding.UTF8.GetString(byt));
            });
        }

        public override void Register() { }
    }
}