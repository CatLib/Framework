
namespace CatLib.Routing
{

    /// <summary>
    /// 验证器接口
    /// </summary>
    public interface IValidators
    {
        /// <summary>
        /// 是否匹配
        /// </summary>
        /// <param name="route">路由条目</param>
        /// <param name="request">请求</param>
        /// <returns></returns>
        bool Matches(Route route, Request request);
    }

}