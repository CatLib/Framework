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

using CatLib.API.INI;

namespace CatLib.INI
{
    /// <summary>
    /// ini服务提供商
    /// </summary>
    public sealed class IniProvider : ServiceProvider
    {
        /// <summary>
        /// 注册ini服务
        /// </summary>
        public override void Register()
        {
            App.Singleton<IniLoader>().Alias<IIniLoader>();
        }
    }
}