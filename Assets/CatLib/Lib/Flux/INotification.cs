
namespace CatLib.Flux
{

    /// <summary>
    /// 通知
    /// </summary>
    public interface INotification
    {
        
        /// <summary>
        /// 通知名
        /// </summary>
		string Name { get; }

        /// <summary>
        /// 附加数据
        /// </summary>
		object Payload { get; set; }

        /// <summary>
        /// 发送者
        /// </summary>
        object Sender { get; set; }

        /// <summary>
        /// 字符串化通知
        /// </summary>
        /// <returns></returns>
        string ToString();

    }

}