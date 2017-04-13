
namespace CatLib.Crypt
{
    public interface ICrypt
    {
        string Encrypt(string str);

        string Decrypt(string str);
    }
}

