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

namespace CatLib.Crypt{

	public class CryptProvider : ServiceProvider {

		public override void Register()
        {
            App.Singleton<Crypt>().Alias<ICrypt>().Resolving((app, bind , obj)=>{
                
                IConfigStore config = app.Make<IConfigStore>();
                Crypt crypt = obj as Crypt;

                crypt.SetKey(config.Get(typeof(Crypt) , "key" , null));

                return obj;
                
            });
        }
	}

}