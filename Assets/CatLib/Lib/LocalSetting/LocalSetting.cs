
using CatLib.API.LocalSetting;

namespace CatLib.LocalSetting
{

    public class LocalSetting : ILocalSetting
    {

        private ILocalSetting settingStore;

        public void SetSettingStore(ILocalSetting setting)
        {
            settingStore = setting;
        }

        public void Save()
        {
            settingStore.Save();
        }

        public bool Has(string key)
        {
            return settingStore.Has(key);
        }

        public void Remove(string key)
        {
            settingStore.Remove(key);
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            return settingStore.GetBool(key , defaultValue);
        }

        public void SetBool(string key, bool val)
        {
            settingStore.SetBool(key, val);
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            return settingStore.GetInt(key, defaultValue);
        }

        public void SetInt(string key, int val)
        {
            settingStore.SetInt(key, val);
        }

        public float GetFloat(string key, float defaultValue = 0)
        {
            return settingStore.GetFloat(key, defaultValue);
        }

        public void SetFloat(string key, float val)
        {
            settingStore.SetFloat(key, val);
        }

        public string GetString(string key, string defaultValue = null)
        {
            return settingStore.GetString(key, defaultValue);
        }

        public void SetString(string key, string val)
        {
            settingStore.SetString(key, val);
        }

        public T GetObject<T>(string key, T defaultValue = default(T))
        {
            return settingStore.GetObject<T>(key, defaultValue);
        }

        public void SetObject<T>(string key, T obj)
        {
            settingStore.SetObject<T>(key, obj);
        }

    }

}
