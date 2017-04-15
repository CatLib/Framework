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
 
namespace CatLib.API.Event
{
    /// <summary>事件机制接口</summary>
    public interface IEvent
    {

        /// <summary>事件系统</summary>
        IEventAchieve Event { get; }

    }
}
