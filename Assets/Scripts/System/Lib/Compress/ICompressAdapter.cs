
namespace CatLib.Compress{

	public interface ICompressAdapter{

		byte[] Compress(byte[] bytes);

		byte[] Expand(byte[] bytes);
		
	}

}
