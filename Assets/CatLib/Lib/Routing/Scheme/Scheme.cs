
namespace CatLib.Routing
{

    /// <summary>
    /// 方案
    /// </summary>
    public abstract class Scheme
    {

        /// <summary>
        /// 增加一个路由
        /// </summary>
        /// <param name="route"></param>
        public void AddRoute(Route route) { }

        /// <summary>
        /// 匹配一个路由
        /// </summary>
        /// <param name="request"></param>
        public Route Match(Request request) { return null; }

    }

}