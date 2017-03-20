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
 
namespace CatLib
{

    /// <summary>
    /// 门面基类
    /// </summary>
    public class Facade<T>
    {

        public static T Instance
        {
            get
            {
                return App.Instance.Make<T>();
            }
        }
    }

}