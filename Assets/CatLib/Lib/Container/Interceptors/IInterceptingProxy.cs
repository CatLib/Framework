
namespace CatLib.Container
{
    /// <summary>
    /// 拦截动态代理接口
    /// </summary>
    public interface IInterceptingProxy
    {

        /// <summary>
        /// 添加拦截器脚本
        /// </summary>
        /// <param name="interceptor">拦截器</param>
        void Interception(IInterceptionBehavior interceptor);

    }

}