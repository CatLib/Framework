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

using CatLib.API.Config;
using CatLib.API.Crypt;

namespace CatLib.Crypt
{
    /// <summary>
    /// 加密服务提供商
    /// </summary>
    public class CryptProvider : ServiceProvider
    {
        /// <summary>
        /// 注册一个加密服务
        /// </summary>
        public override void Register()
        {
            App.Singleton<Crypt>().Alias<ICrypt>().OnResolving((bind, obj) =>
            {
                var config = App.Make<IConfigStore>();
                var crypt = obj as Crypt;

                crypt.SetAdapter(new HMacAes256());

                if (config != null)
                {
                    crypt.SetKey(config.Get(typeof(Crypt), "key", null));
                }

                return obj;
            });
        }
    }
}