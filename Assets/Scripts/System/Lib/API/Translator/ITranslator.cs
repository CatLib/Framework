
using UnityEngine;

namespace CatLib.API.Translator{

	/// <summary>
	/// 翻译(国际化)
	/// </summary>
	public interface ITranslator{
		
		/// <summary>
        /// 翻译内容
        /// </summary>
        /// <param name="key">键</param>
		/// <param name="replace">替换翻译内容的占位符</param>
        /// <returns></returns>
		string Trans(string key , params string[] replace);

		/// <summary>
        /// 翻译内容
        /// </summary>
        /// <param name="key">键</param>
		/// <param name="local">本次翻译的语言</param>
		/// <param name="replace">替换翻译内容的占位符</param>
        /// <returns></returns>
		string Trans(string key , SystemLanguage local , params string[] replace);

		/// <summary>
        /// 翻译内容的复数形式
        /// </summary>
        /// <param name="key">键</param>
		/// <param name="number">数值</param>
		/// <param name="local">本次翻译的语言</param>
		/// <param name="replace">替换翻译内容的占位符</param>
        /// <returns></returns>
		string TransChoice(string key, int number, SystemLanguage local , params string[] replace);

		/// <summary>
        /// 翻译内容的复数形式
        /// </summary>
        /// <param name="key">键</param>
		/// <param name="num">数值</param>
		/// <param name="replace">替换翻译内容的占位符</param>
        /// <returns></returns>
		string TransChoice(string key, int number, params string[] replace);

		/// <summary>
        /// 获取默认本地语言
        /// </summary>
        /// <returns></returns>
		SystemLanguage GetLocale();

		/// <summary>
        /// 设定默认本地语言
        /// </summary>
		/// <param name="local">设定默认本地语言</param>
        /// <returns></returns>
		void SetLocale(SystemLanguage local);

	}

}