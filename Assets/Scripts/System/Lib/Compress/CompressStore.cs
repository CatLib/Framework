using CatLib.API.Compress;

namespace CatLib.Compress{

		
	public class CompressStore : ICompress{

		ICompressAdapter compress;

		public CompressStore(ICompressAdapter adapter){

			compress = adapter;

		}

		public byte[] Compress(byte[] bytes){

			if(bytes == null){ return null; }
			if(bytes.Length <= 0){ return bytes; }

			return compress.Compress(bytes);

		}

		public byte[] Expand(byte[] bytes){

			if(bytes == null){ return null; }
			if(bytes.Length <= 0){ return bytes; }

			return compress.Expand(bytes);

		}
		
	}

}

