
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using CatLib.API.LocalSetting;

namespace CatLib.LocalSetting
{

    public class UnitySetting : ILocalSetting
    {

        public void Save()
        {
            PlayerPrefs.Save();
        }

        public bool Has(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void Remove(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) != 0;
        }

        public void SetBool(string key, bool val)
        {
            PlayerPrefs.SetInt(key, val ? 1 : 0);
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public void SetInt(string key, int val)
        {
            PlayerPrefs.SetInt(key, val);
        }

        public float GetFloat(string key, float defaultValue = 0)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public void SetFloat(string key, float val)
        {
            PlayerPrefs.SetFloat(key, val);
        }

        public string GetString(string key, string defaultValue = null)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public void SetString(string key, string val)
        {
            PlayerPrefs.SetString(key, val);
        }

        public T GetObject<T>(string key, T defaultValue = default(T))
        {
            IFormatter formatter = new BinaryFormatter();
            byte[] buffer = Convert.FromBase64String(GetString(key));
            MemoryStream stream = new MemoryStream(buffer);
            defaultValue = (T)formatter.Deserialize(stream);
            stream.Flush();
            stream.Close();
            return defaultValue;
        }

        public void SetObject<T>(string key, T obj)
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, obj);
            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Close();
            SetString(key, Convert.ToBase64String(buffer));
        }

    }

}