
namespace CatLib.Compress{

	public interface ICompressAdapter{

		byte[] Compress(byte[] bytes, int level);

		byte[] UnCompress(byte[] bytes);
		
	}

}
