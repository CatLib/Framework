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

using System;

namespace CatLib.API
{
    /// <summary>
    /// 全局事件接口
    /// </summary>
    public interface IGlobalEvent
    {
        IGlobalEvent AppendInterface<T>();

        IGlobalEvent AppendInterface(Type t);

        IGlobalEvent SetEventLevel(EventLevel level);

        void Trigger(EventArgs args = null);
    }
}