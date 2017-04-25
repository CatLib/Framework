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

using CatLib.API.Flux;

namespace CatLib.Flux
{
    /// <summary>
    /// 行为
    /// </summary>
    public sealed class FluxAction : IAction
    {
        /// <summary>
        /// 行为行为
        /// </summary>
        private readonly string action;

        /// <summary>
        /// 行为
        /// </summary>
        public string Action
        {
            get { return action; }
        }

        /// <summary>
        /// 附带物
        /// </summary>
        public object Payload { get; set; }

        /// <summary>
        /// 创建一个行为
        /// </summary>
        /// <param name="action">行为</param>
        public FluxAction(string action)
            : this(action, null)
        { }

        /// <summary>
        /// 创建一个行为
        /// </summary>
        /// <param name="action">行为</param>
        /// <param name="payload">附带物体</param>
        public FluxAction(string action, object payload)
        {
            this.action = action;
            Payload = payload;
        }
    }
}