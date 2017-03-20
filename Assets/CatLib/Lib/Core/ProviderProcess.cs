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
 
namespace CatLib
{

    /// <summary>
    /// 服务提供者启动流程（服务提供商级流程）
    /// </summary>
    public enum ProviderProcess
    {

        /// <summary>
        /// 顶级初始化流程
        /// </summary>
        Inited = 1,

        /// <summary>
        /// 程序自动更新流程
        /// </summary>
        ProgramAutoUpdate= 5,

        /// <summary>
        /// 资源自动更新流程
        /// </summary>
        ResourcesAutoUpdate = 10,

        /// <summary>
        /// Lua代码自动载入流程
        /// </summary>
        CodeAutoLoad = 20,

        /// <summary>
        /// 资源准备流程
        /// </summary>
        ResourcesLoad = 30,

        /// <summary>
        /// 标准准备流程
        /// </summary>
        Normal = 100,

        /// <summary>
        /// 延迟标准流程
        /// </summary>
        LateNormal = 200,

    }

}