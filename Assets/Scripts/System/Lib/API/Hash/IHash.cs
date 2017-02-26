
namespace CatLib.API.Hash{

	public interface IHash  {

        string Bcrypt(string password);

        bool BcryptVerify(string text, string hash);

        /// <summary>
        /// 用于计算文件的md5 ， 您不应该用它进行密码等高敏感的hash加密
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string FileHash(string path);

    }

}