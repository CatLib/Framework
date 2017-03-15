
namespace CatLib.API.Compress{

	public interface ICompress{

		byte[] Compress(byte[] bytes, int level = 6);

		byte[] UnCompress(byte[] bytes);

	}

}
