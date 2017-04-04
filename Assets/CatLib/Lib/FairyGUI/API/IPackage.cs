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

using FairyGUI;

namespace CatLib.API.FairyGUI
{

    public interface IPackage
    {

        UIPackage AddPackage(string assetPath);

    }

}