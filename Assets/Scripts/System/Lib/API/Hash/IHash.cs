
namespace CatLib.API.Hash{

	public interface IHash  {

        string Bcrypt(string password);

        bool BcryptVerify(string text, string hash);

    }

}