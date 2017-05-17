
namespace CatLib.API.Config
{
    /// <summary>
    /// 配置定位器
    /// </summary>
    public interface IConfigLocator
    {
        /// <summary>
        /// 根据配置名获取配置的值
        /// </summary>
        /// <param name="name">配置名</param>
        /// <param name="value">配置值</param>
        /// <returns>是否获取到配置</returns>
        bool TryGetValue(string name , out string value);
    }
}
