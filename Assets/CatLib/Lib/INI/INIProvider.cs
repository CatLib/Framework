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
 
using CatLib.API.INI;

namespace CatLib.INI
{

    /// <summary>
    /// INI服务提供商
    /// </summary>
    public class INIProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Singleton<INILoader>().Alias<IINILoader>();
        }

    }

}