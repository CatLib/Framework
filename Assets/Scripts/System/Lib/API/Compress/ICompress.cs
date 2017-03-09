
namespace CatLib.API.Compress{

	public interface ICompress{

		byte[] Compress(byte[] bytes);

		byte[] Expand(byte[] bytes);

	}

}
