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

using System.Collections;
using CatLib.API.Config;
using CatLib.API.IO;

namespace CatLib.IO
{
    /// <summary>
    /// IO服务提供商
    /// </summary>
    public sealed class IOProvider : ServiceProvider
    {
        /// <summary>
        /// 注册IO服务
        /// </summary>
        public override void Register()
        {
            App.Singleton<IO>().Alias<IIOFactory>().OnResolving((bind, obj) =>
            {
                var config = App.Make<IConfigStore>();

                if (config == null)
                {
                    return obj;
                }

                var io = obj as IO;
                io.SetQuery((name) => config.Get<Hashtable>(typeof(IO), name));

                return obj;
            });
            App.Bind<LocalDisk>().Alias<IDisk>();
        }
    }
}