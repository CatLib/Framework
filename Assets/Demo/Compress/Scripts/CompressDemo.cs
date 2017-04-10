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

using CatLib.API;
using CatLib.API.Compress;
using CatLib.API.IO;

namespace CatLib.Demo.Compress
{

    public class CompressDemo : ServiceProvider
    {

        public override void Init()
        {
            App.On(ApplicationEvents.ON_APPLICATION_START_COMPLETE, (sender, e) =>
            {
                //IIOFactory fac = App.Make<IIOFactory>();
                //IDisk disk = fac.Disk();
                //IFile file = disk.File("hello.gz");
                ICompress comp = App.Make<ICompress>();
                byte[] byt = comp.Compress("helloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworld".ToByte());

                UnityEngine.Debug.Log("UnCompress: " + "helloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworld".ToByte().Length);
                UnityEngine.Debug.Log("Compress: " + byt.Length);
                //file.Create(byt);

                byt = comp.UnCompress(byt);
                UnityEngine.Debug.Log(System.Text.Encoding.UTF8.GetString(byt));

                //byte[] debyt = comp.Expand(byt);

                //Debug.Log(System.Text.Encoding.UTF8.GetString(debyt));
            });
        }

        public override void Register() { }
    }
}