
namespace CatLib.API.INI
{

    public interface IINIResult
    {

        /// <summary>
        /// 获取一个ini配置
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(string section, string key, string def = null);

        /// <summary>
        /// 设定一个ini配置
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        void Set(string section, string key, string val);

        /// <summary>
        /// 移除一个配置
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        void Remove(string section, string key);

        /// <summary>
        /// 移除一个区块
        /// </summary>
        /// <param name="section"></param>
        void Remove(string section);

        /// <summary>
        /// 保存ini文件
        /// </summary>
        /// <returns></returns>
        void Save();

    }

}