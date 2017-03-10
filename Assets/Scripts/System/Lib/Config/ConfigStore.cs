
using System;
using System.Collections.Generic;
using CatLib.API.Config;

namespace CatLib.Config{

	public class ConfigStore : Component , IConfigStore{

		protected Dictionary<string, Dictionary<string, object>> configs;

		public ConfigStore(){

			configs = new Dictionary<string, Dictionary<string, object>>();
			InitConfig();

		}

		public T Get<T>(Type service, string field , T def = default(T)){

			return Get<T>(service.ToString() , field , def);

		}

		public T Get<T>(string service, string field , T def = default(T))
		{

			try{

				object obj = Get(service , field , (object)def);
				if(obj == null){ return def; }

				if(typeof(T) == typeof(int)){

					return (T)Convert.ChangeType(obj, typeof(int));

				}
				if(typeof(T) == typeof(string)){

					return (T)Convert.ChangeType(obj , typeof(string));

				}
				return (T)obj;

			}catch{ throw new ArgumentException(" field [" + field + "] is can not conversion to " + typeof(T).ToString()); }

		}

		public string Get(Type service , string field , string def){

			return Get(service.ToString(), field , def);

		}

		public string Get(string service , string field , string def){

			return Get<string>(service , field , def);

		}

		public object Get(Type service , string field , object def){

			return Get(service.ToString(), field , def);

		}

		public object Get(string service , string field , object def){

			if(!configs.ContainsKey(service)){ return def; }
			if(!configs[service].ContainsKey(field)){ return def; }

			return configs[service][field];

		}

		protected void InitConfig()
        {
            Type[] types = typeof(IConfig).GetChildTypesWithInterface();
			IConfig conf;
			for(int i = 0; i < types.Length ; i++){

				conf = App.Make(types[i].ToString(), null) as IConfig;
				configs.Remove(conf.Service.ToString());
				configs.Add(conf.Service.ToString() , ParseConfig(conf));

			}
        }

		protected Dictionary<string , object> ParseConfig(IConfig config){

			if(config.Config.Length <= 0){ return new Dictionary<string , object>(); }
			if (config.Config.Length % 2 != 0) { throw new ArgumentException("param is not incorrect"); }

			Dictionary<string , object> fields = new Dictionary<string , object>();
			object[] param = config.Config;
            for (int i = 0; i < param.Length; i += 2)
            {
                fields.Add(param[i].ToString() , param[i + 1]);
            }

            return fields;

		}

	}

}