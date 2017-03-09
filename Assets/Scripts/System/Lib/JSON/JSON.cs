using CatLib.API.JSON;

namespace CatLib.JSON{

    public class JSON : IJSON {

        private IJSONAdapter jsonParse;

        public JSON(IJSONAdapter adapter){

            jsonParse = adapter;

        }

        public T Decode<T>(string json){

            if(string.IsNullOrEmpty(json)){

                return default(T);
                
            }

            if(jsonParse != null){

                return jsonParse.Decode<T>(json);

            }
            return default(T);

        }

		public string Encode(object item){

            if(item == null){ return null; }

            return jsonParse.Encode(item);

        }
        

    }

}
