
namespace CatLib.Contracts.Secret{

	public interface ISecret{

		IHash Hash{ get; }

		ICrypt Crypt{ get; }

	}

}