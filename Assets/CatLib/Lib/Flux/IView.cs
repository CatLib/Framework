
namespace CatLib.Flux
{

    /// <summary>
    /// 视图接口
    /// </summary>
    public interface IView
    {

        /// <summary>
        /// 视图的名字
        /// </summary>
		string Name { get; }

        /// <summary>
        /// 当注册时
        /// </summary>
        void OnRegister();

        /// <summary>
        /// 当视图销毁时
        /// </summary>
        void OnDestroy();

    }

}