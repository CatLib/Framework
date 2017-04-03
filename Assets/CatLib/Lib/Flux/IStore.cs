
namespace CatLib.Flux
{

    /// <summary>
    /// 存储
    /// </summary>
    public interface IStore
    {

        /// <summary>
        /// 存储的名字
        /// </summary>
		string Name { get; }

        /// <summary>
        /// 被管理的数据对象
        /// </summary>
		object Data { get; set; }

        /// <summary>
        /// 当移除时
        /// </summary>
        void OnRemove();

    }

}