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

using CatLib.API.AssetBuilder;
using UnityEditor;

namespace CatLib.AssetBuilder
{
    /// <summary>
    /// 当编译完成时处理的策略
    /// </summary>
    public sealed class CompleteStrategy : IBuildStrategy
    {
        /// <summary>
        /// 配置的编译流程
        /// </summary>
        public BuildProcess Process
        {
            get { return BuildProcess.Complete; }
        }

        /// <summary>
        /// 执行编译时
        /// </summary>
        /// <param name="context">编译上下文</param>
        public void Build(IBuildContext context)
        {
            AssetDatabase.Refresh();
        }
    }
}