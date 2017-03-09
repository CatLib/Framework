
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

				return new ZipStorerAdapter();

			}).Alias("compress.parse");

		}
		
	}

}