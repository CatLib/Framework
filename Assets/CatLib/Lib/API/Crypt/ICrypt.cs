
namespace CatLib.API.Crypt{

	public interface ICrypt{

        string Encrypt(string toEncrypt);

        string Decrypt(string toDecrypt);


    }

}