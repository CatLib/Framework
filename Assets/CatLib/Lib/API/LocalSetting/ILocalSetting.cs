

namespace CatLib.API.LocalSetting
{

    /// <summary>
    /// 本地配置
    /// </summary>
    public interface ILocalSetting
    {

        void Save();

        bool Has(string key);

        void Remove(string key);

        bool GetBool(string key, bool defaultValue = false);

        void SetBool(string key, bool val);

        int GetInt(string key, int defaultValue = 0);

        void SetInt(string key, int val);

        float GetFloat(string key, float defaultValue = 0);

        void SetFloat(string key, float val);

        string GetString(string key, string defaultValue = null);

        void SetString(string key, string val);

        T GetObject<T>(string key, T defaultValue = default(T));

        void SetObject<T>(string key, T obj);

    }


}