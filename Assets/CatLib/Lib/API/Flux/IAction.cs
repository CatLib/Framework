
namespace CatLib.API.Flux
{

    /// <summary>
    /// 行为
    /// </summary>
    public interface IAction
    {

        /// <summary>
        /// 行为
        /// </summary>
        string Action { get; }

        /// <summary>
        /// 附加数据
        /// </summary>
		object Payload { get; set; }

        /// <summary>
        /// 字符串化行为
        /// </summary>
        /// <returns></returns>
        string ToString();

    }

}