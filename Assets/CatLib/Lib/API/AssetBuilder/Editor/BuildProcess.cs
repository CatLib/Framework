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
    /// 构建流程
    /// </summary>
    public enum BuildProcess
    {
        /// <summary>
        /// 准备一系列配置信息
        /// </summary>
        Setup = 1,

        /// <summary>
        /// 清理旧的数据
        /// </summary>
        Clear = 10,

        /// <summary>
        /// 预编译
        /// </summary>
        Precompiled = 20,

        /// <summary>
        /// 编译文件
        /// </summary>
        Build = 30,

        /// <summary>
        /// 编译出的文件扫描流程
        /// </summary>
        Scanning = 40,

        /// <summary>
        /// 对扫描到的有效文件进行过滤
        /// </summary>
        Filter = 50,

        /// <summary>
        /// 对文件进行加密
        /// </summary>
        Encryption = 60,

        /// <summary>
        /// 生成文件结构
        /// </summary>
        GenTable = 70,

        /// <summary>
        /// 更新目录生成
        /// </summary>
        GenPath = 80,

        /// <summary>
        /// 完成
        /// </summary>
        Complete = 90,
    }
}