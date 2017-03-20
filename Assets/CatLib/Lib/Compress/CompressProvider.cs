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
 
using CatLib.API.Compress;

namespace CatLib.Compress{

	public class CompressProvider : ServiceProvider {

		public override void Register()
		{

			RegisterParse();
			App.Singleton<CompressStore>().Alias<ICompress>().Alias("compress");

		}

		protected void RegisterParse(){

			App.Singleton<ICompressAdapter>((app , param) => {

				return new ShareZipLibAdapter();

			}).Alias("compress.parse");

		}
		
	}

}