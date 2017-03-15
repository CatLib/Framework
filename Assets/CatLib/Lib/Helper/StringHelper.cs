
namespace CatLib
{
    public static class StringHelper
    {
        
        /// <summary>
        /// 标准化路径
        /// </summary>
        public static string Standard(this string path)
        {
            return path.Replace("\\", "/");
        }

    }

}