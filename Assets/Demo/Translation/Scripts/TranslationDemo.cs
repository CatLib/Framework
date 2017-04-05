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

using CatLib.API;
using CatLib.API.Translation;

namespace CatLib.Demo.Translation{

	public class TranslationDemo : ServiceProvider {

		public override void Init()
        {
            App.On(ApplicationEvents.ON_APPLICATION_START_COMPLETE, (sender, e) =>
            {
				ITranslator trans = App.Make<ITranslator>();
				trans.SetLocale("zh");
				UnityEngine.Debug.Log(trans.Trans("bag.title" , "name" , "catlib"));
				UnityEngine.Debug.Log(trans.TransChoice("bag.morecat" , 18 , "name" , "catlib"));
				UnityEngine.Debug.Log(trans.Trans("bag.catage" ,  "name" , "catlib",  "age" , "18"));

				trans.SetLocale("en");
				UnityEngine.Debug.Log(trans.Trans("bag.title" , "name" , "catlib"));
				UnityEngine.Debug.Log(trans.TransChoice("bag.morecat" , 18 , "name" , "catlib"));
				UnityEngine.Debug.Log(trans.Trans("bag.catage" ,  "name" , "catlib",  "age" , "18"));
			});
		}

		public override void Register(){  }

	}

}