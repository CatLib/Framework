
namespace CatLib.API.Secret{

	public interface ISecret{

		IHash Hash{ get; }

		ICrypt Crypt{ get; }

	}

}