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
 
using System;
using System.Collections.Generic;
using CatLib.API.Config;
using CatLib.API;

namespace CatLib.Config{

	public class ConfigStore : IConfigStore{

        [Dependency]
        public IApplication App { get; set; }

        protected Dictionary<string, Dictionary<string, object>> configs;

		public ConfigStore(){

			configs = new Dictionary<string, Dictionary<string, object>>();

		}

        public void Init()
        {
            InitConfig();
        }

		public T Get<T>(Type name, string field , T def = default(T)){

			return Get<T>(name.ToString() , field , def);

		}

		public T Get<T>(string name, string field , T def = default(T))
		{

			try{

				object obj = Get(name , field , (object)def);
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

		public string Get(Type name , string field , string def){

			return Get(name.ToString(), field , def);

		}

		public string Get(string name , string field , string def){

			return Get<string>(name , field , def);

		}

		public object Get(Type name , string field , object def){

			return Get(name.ToString(), field , def);

		}

		public object Get(string name , string field , object def){

			if(!configs.ContainsKey(name)){ return def; }
			if(!configs[name].ContainsKey(field)){ return def; }

			return configs[name][field];

		}

		protected void InitConfig()
        {
            Type[] types = typeof(IConfig).GetChildTypesWithInterface();
			IConfig conf;
			for(int i = 0; i < types.Length ; i++){

				conf = App.Make(types[i].ToString(), null) as IConfig;
				configs.Remove(conf.Name.ToString());
				configs.Add(conf.Name.ToString() , ParseConfig(conf));

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