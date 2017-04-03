
namespace CatLib.API.Flux
{

    /// <summary>
    /// 通知
    /// </summary>
    public interface INotification
    {

        /// <summary>
        /// 通知行为
        /// </summary>
        string Action { get; }

        /// <summary>
        /// 附加数据
        /// </summary>
		object Payload { get; set; }

        /// <summary>
        /// 字符串化通知
        /// </summary>
        /// <returns></returns>
        string ToString();

    }

}