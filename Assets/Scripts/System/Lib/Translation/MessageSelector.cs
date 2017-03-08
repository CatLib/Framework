
using System.Text.RegularExpressions;

namespace CatLib.Translation{

	public class MessageSelector : ISelector {


		public string Choose(string line , int number, string locale){

			string[] segments = line.Split('|');
			
			string val;
			if ((val = Extract(segments, number)) != null) {

            	return val.Trim();
        	
			}

			segments = StripConditions(segments);
			int pluralIndex = GetPluralIndex(locale , number);

			if(segments.Length == 1 || segments.Length < pluralIndex){
				
				return segments[0];
			
			}

			return segments[pluralIndex];

		}

		protected string Extract(string[] segments , int number){
			
			string line = null;
			for(int i = 0 ; i < segments.Length ; i++){

				if((line = RangeExtract(segments[i], number)) != null){

					return line;

				}

			}
			return line;
		}

		protected string RangeExtract(string parts , int number){

			string regstr = @"^[\{\[]([^\[\]\{\}]*)[\}\]](.*)";
			Regex reg = new Regex(regstr);
			
			MatchCollection mc = reg.Matches(parts);
			if(mc.Count < 1){ return null; }
			if(mc[0].Groups.Count != 3){ return null; }

			string condition = mc[0].Groups[1].ToString();
			string val = mc[0].Groups[2].ToString();

			if(condition.Contains(",")){
				string[] fromTo = condition.Split(new char[]{ ',' } , 2);
				 if (fromTo[1] == "*" && number >= int.Parse(fromTo[0])) {
					return val;
				} else if (fromTo[0] == "*" && number <= int.Parse(fromTo[1])) {
					return val;
				} else if (number >= int.Parse(fromTo[0]) && number <= int.Parse(fromTo[1])) {
					return val;
				}
			}

			return null;

		}

		protected string[] StripConditions(string[] segments){

			for(int i = 0; i < segments.Length ; i++){

				segments[i] = Regex.Replace(segments[i], @"^[\{\[]([^\[\]\{\}]*)[\}\]]", string.Empty);

			}

			return segments;

		}

		protected int GetPluralIndex(string locale , int number){

			switch (locale) {
				case "az":
				case "bo":
				case "dz":
				case "id":
				case "ja":
				case "jv":
				case "ka":
				case "km":
				case "kn":
				case "ko":
				case "ms":
				case "th":
				case "tr":
				case "vi":
				case "zh":
					return 0;
				case "af":
				case "bn":
				case "bg":
				case "ca":
				case "da":
				case "de":
				case "el":
				case "en":
				case "eo":
				case "es":
				case "et":
				case "eu":
				case "fa":
				case "fi":
				case "fo":
				case "fur":
				case "fy":
				case "gl":
				case "gu":
				case "ha":
				case "he":
				case "hu":
				case "is":
				case "it":
				case "ku":
				case "lb":
				case "ml":
				case "mn":
				case "mr":
				case "nah":
				case "nb":
				case "ne":
				case "nl":
				case "nn":
				case "no":
				case "om":
				case "or":
				case "pa":
				case "pap":
				case "ps":
				case "pt":
				case "so":
				case "sq":
				case "sv":
				case "sw":
				case "ta":
				case "te":
				case "tk":
				case "ur":
				case "zu":
					return (number == 1) ? 0 : 1;
				case "am":
				case "bh":
				case "fil":
				case "fr":
				case "gun":
				case "hi":
				case "hy":
				case "ln":
				case "mg":
				case "nso":
				case "xbr":
				case "ti":
				case "wa":
					return ((number == 0) || (number == 1)) ? 0 : 1;
				case "be":
				case "bs":
				case "hr":
				case "ru":
				case "sr":
				case "uk":
					return ((number % 10 == 1) && (number % 100 != 11)) ? 0 : (((number % 10 >= 2) && (number % 10 <= 4) && ((number % 100 < 10) || (number % 100 >= 20))) ? 1 : 2);
				case "cs":
				case "sk":
					return (number == 1) ? 0 : (((number >= 2) && (number <= 4)) ? 1 : 2);
				case "ga":
					return (number == 1) ? 0 : ((number == 2) ? 1 : 2);
				case "lt":
					return ((number % 10 == 1) && (number % 100 != 11)) ? 0 : (((number % 10 >= 2) && ((number % 100 < 10) || (number % 100 >= 20))) ? 1 : 2);
				case "sl":
					return (number % 100 == 1) ? 0 : ((number % 100 == 2) ? 1 : (((number % 100 == 3) || (number % 100 == 4)) ? 2 : 3));
				case "mk":
					return (number % 10 == 1) ? 0 : 1;
				case "mt":
					return (number == 1) ? 0 : (((number == 0) || ((number % 100 > 1) && (number % 100 < 11))) ? 1 : (((number % 100 > 10) && (number % 100 < 20)) ? 2 : 3));
				case "lv":
					return (number == 0) ? 0 : (((number % 10 == 1) && (number % 100 != 11)) ? 1 : 2);
				case "pl":
					return (number == 1) ? 0 : (((number % 10 >= 2) && (number % 10 <= 4) && ((number % 100 < 12) || (number % 100 > 14))) ? 1 : 2);
				case "cy":
					return (number == 1) ? 0 : ((number == 2) ? 1 : (((number == 8) || (number == 11)) ? 2 : 3));
				case "ro":
					return (number == 1) ? 0 : (((number == 0) || ((number % 100 > 0) && (number % 100 < 20))) ? 1 : 2);
				case "ar":
					return (number == 0) ? 0 : ((number == 1) ? 1 : ((number == 2) ? 2 : (((number % 100 >= 3) && (number % 100 <= 10)) ? 3 : (((number % 100 >= 11) && (number % 100 <= 99)) ? 4 : 5))));
				default:
					return 0;
			}

		}

	}

}