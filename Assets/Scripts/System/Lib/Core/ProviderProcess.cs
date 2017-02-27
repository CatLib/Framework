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
        INITED = 1,

        /// <summary>
        /// 程序自动更新流程
        /// </summary>
        PROGRAM_AUTO_UPDATE = 5,

        /// <summary>
        /// 资源自动更新流程
        /// </summary>
        RESOURCES_AUTO_UPDATE = 10,

        /// <summary>
        /// Lua代码自动载入流程
        /// </summary>
        CODE_AUTO_LOAD = 20,

        /// <summary>
        /// 资源准备流程
        /// </summary>
        RESOURCES_LOAD = 30,

        /// <summary>
        /// 标准准备流程
        /// </summary>
        NORMAL = 100,

        /// <summary>
        /// 延迟标准流程
        /// </summary>
        LATE_NORMAL = 200,

    }

}