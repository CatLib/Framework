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

namespace CatLib.API.AssetBuilder
{
    /// <summary>
    /// 编译策略
    /// </summary>
    public interface IBuildStrategy
    {
        /// <summary>
        /// 编译流水线位置
        /// </summary>
        BuildProcess Process { get; }

        /// <summary>
        /// 当编译时
        /// </summary>
        /// <param name="context">上下文</param>
        void Build(IBuildContext context);
    }
}