
namespace CatLib
{
    public static class StringHelper
    {

        /// <summary>
        /// 字符串后缀
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Suffix(this string str)
        {
            string[] array = str.Split(new char[] { '.' });
            if (array.Length <= 0) { return string.Empty; }
            return '.' + array[array.Length - 1];
        }

    }

}