/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
 
using System.Collections.Generic;
using CatLib.API.Translator;

namespace CatLib.Translation{

	public class Translator : Component , ITranslator {

		/// <summary>
        /// 已经被加载的语言集(locale , file , IFileMapping)
        /// </summary>
		protected Dictionary<string , IFileMapping> loaded;

		/// <summary>
        /// 消息选择器
        /// </summary>
		protected ISelector selector;

		/// <summary>
        /// 根目录
        /// </summary>
		protected string root;

		/// <summary>
        /// 当前语言
        /// </summary>
		protected string locale;

		/// <summary>
        /// 备选语言
        /// </summary>
		protected string fallback;

		/// <summary>
        /// 加载器
        /// </summary>
		protected IFileLoader loader;

		/// <summary>
        /// 已经被转义过的key缓存
        /// </summary>
    	protected Dictionary<string , string[]> parsed;

		public void SetRoot(string root){

			if(!string.IsNullOrEmpty(root)){

				this.root = root;

			}

		}

		public void SetFallback(string fallback){

			if(!string.IsNullOrEmpty(fallback)){

				this.fallback = fallback;
				
			}

		}

		public Translator(){

			loaded = new Dictionary<string , IFileMapping>();
			parsed = new Dictionary<string , string[]>();
			
		}

		public void SetSelector(ISelector selector){

			this.selector = selector;

		}

		public void SetFileLoader(IFileLoader loader ){

			this.loader = loader;

		}

		/// <summary>
        /// 翻译内容
        /// </summary>
        /// <param name="key">键</param>
		/// <param name="replace">替换翻译内容的占位符</param>
        /// <returns></returns>
		public string Trans(string key , params string[] replace){

			string line = Get(locale , key , replace);
			if(line == null){ line = Get(fallback, key , replace); }
			return line;

		}

		/// <summary>
        /// 翻译内容的复数形式
        /// </summary>
        /// <param name="key">键</param>
		/// <param name="number">数值</param>
		/// <param name="local">本次翻译的语言</param>
		/// <param name="replace">替换翻译内容的占位符</param>
        /// <returns></returns>
		public string TransChoice(string key, int number, params string[] replace){

			return Choice(key, number, replace);

		}

		/// <summary>
        /// 获取默认本地语言
        /// </summary>
        /// <returns></returns>
		public string GetLocale(){

			return locale;

		}

		/// <summary>
        /// 设定默认本地语言
        /// </summary>
		/// <param name="locale">设定默认本地语言</param>
        /// <returns></returns>
		public void SetLocale(string locale){

			this.locale = locale;
			
		}

		protected string Choice(string key , int number , string[] replace){

			string locale = this.locale;
			string line = Get(locale, key , replace);
			if(line == null){ 
				line = Get(fallback, key , replace); 
				locale = fallback; 
			}

			replace = new string[]{ "count" , number.ToString() };

			return MakeReplacements(selector.Choose(line, number , locale) , replace);

		}

		protected string Get(string locale , string key, string[] replace)
    	{
			//segments: file , key
			string[] segments = ParseKey(key);

			return GetLine(segments[0] , segments[1] , locale , replace);

		}

		protected string GetLine(string file, string key , string locale , string[] replace){

			Load(file , key , locale);
			
			if(loaded[locale + "." + file] == null){ return null; }

			string line = loaded[locale + "." + file].Get(key , null);

			return MakeReplacements(line, replace);

		}

		/// <summary>
        /// 替换内容
        /// </summary>
		protected string MakeReplacements(string line , string[] replace){

			if(line == null){ return null; }
			if(replace.Length <= 0){ return line; }

			string[] tmp;
			string replaceLeft , replaceRight;
			for(int i = 0; i < replace.Length ;){

				tmp = replace[i].Split(':');
				if(tmp.Length == 2){
					replaceLeft = tmp[0];
					replaceRight = tmp[1];
					i += 1;
				}else{
					if(i + 1 >= replace.Length){ break; }
					replaceLeft = replace[i];
					replaceRight = replace[i + 1];
					i += 2;
				}

				line = line.Replace(":" + replaceLeft , replaceRight);
				line = line.Replace(":" + replaceLeft.ToUpper() , replaceRight.ToUpper());
				if(replaceRight.Length >= 1 && replaceRight.Length >= 1){
					line = line.Replace(":" + replaceLeft.Substring(0,1).ToUpper()+replaceLeft.Substring(1) , replaceRight.Substring(0,1).ToUpper() + replaceRight.Substring(1));
				}

			}

			return line;

		}

		protected void Load(string file, string key , string locale){

			if(loaded.ContainsKey(locale + "." + file)){
				return;
			}

			IFileMapping mapping = loader.Load(root, locale, file);
			loaded.Add(locale + "." + file , mapping);	

		}

		/// <summary>
        /// 格式化key
        /// </summary>
		/// <param name="local">设定默认本地语言</param>
        /// <returns></returns>
		protected string[] ParseKey(string key){

			if(parsed.ContainsKey(key)){ return parsed[key]; }
			
			string[] segments = new string[2];
			
			string[] keySegments = key.Split('.');
			if(keySegments.Length > 1){
				segments[0] = keySegments[0];
				segments[1] = keySegments[1];
			}else{
				
				throw new System.Exception("translator key is invalid");

			}
			
			return segments;


		}
	}

}