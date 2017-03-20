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
 
namespace CatLib.API { 

    /// <summary>
    /// 调试等级
    /// </summary>
    public enum DebugLevels{

        /// <summary>
        /// 线上 
        /// </summary>
        Online,

        /// <summary>
        /// 仿真模拟
        /// </summary>
        Staging,

        /// <summary>
        /// 开发者模式（在移动设备上允许开启调试）
        /// </summary>
        Dev,

        /// <summary>
        /// 自动模式（如果在编辑器模式下则使用开发者模式（非仿真模拟）如果发布则使用线上模式）
        /// </summary>
        Auto,

    }

}