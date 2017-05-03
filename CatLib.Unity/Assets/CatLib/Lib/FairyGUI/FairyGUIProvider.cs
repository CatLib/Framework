﻿/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using CatLib.API.FairyGUI;

namespace CatLib.FairyGUI
{

    public class FairyGUIProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Singleton<Package>().Alias<IPackage>();
        }

    }

}