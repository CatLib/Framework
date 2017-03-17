

namespace CatLib.API.LocalStore
{

    /// <summary>
    /// 本地存储
    /// </summary>
    public interface ILocalStore
    {

        /// <summary>
        /// 获取一个命名空间下的本地存储控制句柄
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        ILocalStoreHandle Namespace(string ns);

        /// <summary>
        /// 获取一个命名空间下的本地存储控制句柄
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        ILocalStoreHandle this[string ns] { get; }

    }


}