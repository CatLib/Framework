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
 
namespace CatLib.Csv
{

    /// <summary>
    /// 使用的标准
    /// </summary>
    public interface IStandard
    {

        string[] Parse(string line);

    }


}