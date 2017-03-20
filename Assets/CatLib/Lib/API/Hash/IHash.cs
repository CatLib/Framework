/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
 
namespace CatLib.API.Hash{

	public interface IHash  {

        /// <summary>
        /// 对一个字符串进行Hash加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        string Make(string password);

        /// <summary>
        /// 验证一个Hash是否有效
        /// </summary>
        /// <param name="text"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        bool Check(string text, string hash);

        /// <summary>
        /// 用于计算文件的md5 ， 您不应该用它进行密码等高敏感的hash加密
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string FileHash(string path);

    }

}