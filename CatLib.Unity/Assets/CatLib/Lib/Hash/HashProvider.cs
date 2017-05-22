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
using CatLib.API.Hash;

namespace CatLib.Hash
{

    public class HashProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Singleton<Hash>().Alias<IHash>().OnResolving((bind, obj) =>{

                var config = App.Make<IConfig>();
                var hash = obj as Hash;

                hash.SetFactor(config.Get("hash.factor", 6));
                hash.SetGenerateSalt(config.Get<string>("hash.salt"));

                return obj;

            });
        }
    }

}