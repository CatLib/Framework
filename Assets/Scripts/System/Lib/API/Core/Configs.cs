using System;
using System.Collections.Generic;

namespace CatLib.API
{

    /// <summary>
    /// 容器注入配置
    /// </summary>
    public abstract class Configs
    {

        protected class ConfigField
        {
            public string Key { get; set; }

            public object Val { get; set; }

            public ConfigField(string key, object val)
            {
                Key = key;
                Val = val;
            }
        }

        protected Dictionary<string, ConfigField> field = new Dictionary<string, ConfigField>();

        public Configs()
        {
            foreach (ConfigField field in ToField(Field))
            {
                if (!this.field.ContainsKey(field.Key))
                {
                    this.field.Add(field.Key, field);
                }
            }
        }

        /// <summary>
        /// 配置注入的类
        /// </summary>
        public abstract Type Class
        {
            get;
        }

        /// <summary>
        /// 配置字段
        /// </summary>
        protected abstract object[] Field { get; }


        private ConfigField[] ToField(params object[] param)
        {
            if (param.Length % 2 != 0) { throw new ArgumentException("param is not incorrect"); }

            List<ConfigField> fields = new List<ConfigField>();
            for (int i = 0; i < param.Length; i += 2)
            {
                fields.Add(new ConfigField(param[i].ToString(), param[i + 1]));
            }

            return fields.ToArray();

        }

        /// <summary>
        /// 获取一个配置如果获取不到则使用默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public T Get<T>(string field , T def = default(T))
        {
            try
            {
                if (this.field.ContainsKey(field))
                {
                    return (T)this.field[field].Val;
                }
                return def;
            }
            catch { throw new ArgumentException(" field [" + field + "] is can not conversion to " + typeof(T).ToString()); }
        }


        public bool IsExists(string field)
        {
            if (string.IsNullOrEmpty(field)) { return false; }
            if (this.field.ContainsKey(field))
            {
                return true;
            }
            return false;
        }
    }

}