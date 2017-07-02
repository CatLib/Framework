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

namespace CatLib.API.Debugger
{
    /// <summary>
    /// 调试器
    /// 
    /// </summary>
    public interface IDebugger : ILogger
    {
        /// <summary>
        /// 定义组
        /// </summary>
        /// <param name="groupName">分组名</param>
        /// <param name="nickname">昵称(用于在调试控制器显示)</param>
        void DefinedGroup(string groupName, string nickname);
    }
}
