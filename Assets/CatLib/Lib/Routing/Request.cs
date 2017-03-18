
using CatLib.API.Routing;

namespace CatLib.Routing
{

    /// <summary>
    /// 请求
    /// </summary>
    public class Request : IRequest
    {

        /// <summary>
        /// 方案
        /// </summary>
        public string Scheme { get; protected set; }

        public Request(string uri , object context)
        {

        }

        /// <summary>
        /// 获取字符串附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetString(string key, string defaultValue = null)
        {
            return defaultValue;
        }

        /// <summary>
        /// 获取整型的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetInt(string key, int defaultValue = 0)
        {
            return defaultValue;
        }

        /// <summary>
        /// 获取长整型的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public long GetLong(string key, long defaultValue = 0)
        {
            return defaultValue;
        }

        /// <summary>
        /// 获取短整型的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public short GetShort(string key, short defaultValue = 0)
        {
            return defaultValue;
        }

        /// <summary>
        /// 获取字符的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public char GetChar(string key, char defaultValue = char.MinValue)
        {
            return defaultValue;
        }

        /// <summary>
        /// 获取浮点数的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public float GetFloat(string key, float defaultValue = 0)
        {
            return defaultValue;
        }

        /// <summary>
        /// 获取双精度浮点数的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public double GetDouble(string key, double defaultValue = 0)
        {
            return defaultValue;
        }

        /// <summary>
        /// 获取布尔值的附加物
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool GetBoolean(string key, bool defaultValue = false)
        {
            return defaultValue;
        }

    }

}