
namespace CatLib.Container { 

    /// <summary>
    /// 构建步骤
    /// </summary>
    public enum BuildStages
    {

        /// <summary>
        /// 在构建之前
        /// </summary>
        PreCreation,

        /// <summary>
        /// 构建时
        /// </summary>
        Creation,

        /// <summary>
        /// 初始化
        /// </summary>
        Initialization,

        /// <summary>
        /// 后期初始化
        /// </summary>
        PostInitialization,

    }

}