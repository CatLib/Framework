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
using CatLib.API.IO;
using CatLib.API.INI;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CatLib.INI
{

    /// <summary>
    /// INI结果集
    /// </summary>
    public class INIResult : IINIResult
    {

        private static char[] IGNORE_CHAR = { '#', ';' };

        private IFile file;
        private Dictionary<string, Dictionary<string, string>> iniDict = new Dictionary<string, Dictionary<string, string>>();

        public INIResult(IFile file)
        {
            this.file = file;
            LoadData();
        }

        /// <summary>
        /// 获取一个ini配置
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string section, string key, string def = null)
        {
            if (iniDict.ContainsKey(section))
            {
                if (iniDict[section].ContainsKey(key))
                {
                    return iniDict[section][key];
                }
            }
            return def;
        }

        /// <summary>
        /// 设定一个ini配置
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Set(string section, string key, string val)
        {
            if (key == string.Empty) { return; }
            if (!iniDict.ContainsKey(section))
            {
                iniDict.Add(section, new Dictionary<string, string>());
            }

            if (iniDict[section].ContainsKey(key))
            {
                iniDict[section][key] = val;
            }
            else
            {
                iniDict[section].Add(key, val);
            }
        }

        /// <summary>
        /// 移除一个ini配置
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        public void Remove(string section, string key)
        {
            if (iniDict.ContainsKey(section))
            {
                iniDict[section].Remove(key);
                if(iniDict[section].Count <= 0)
                {
                    iniDict.Remove(section);
                }
            }
        }

        /// <summary>
        /// 移除一个ini区块
        /// </summary>
        /// <param name="section"></param>
        public void Remove(string section)
        {
            iniDict.Remove(section);
        }

        /// <summary>
        /// 保存ini文件
        /// </summary>
        /// <returns></returns>
        public void Save()
        {
            StringBuilder data = new StringBuilder();
            foreach(var dict in iniDict)
            {
                data.AppendLine("[" + dict.Key + "]");
                foreach(var kv in dict.Value)
                {
                    data.AppendLine(kv.Key + "=" + kv.Value);
                }
            }
            file.Delete();
            file.Create(data.ToString().ToByte());
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        protected void LoadData()
        {
            if (!file.Exists)
            {
                throw new IOException("file is not exists:" + file.FullName);
            }

            if (file.Extension != ".ini")
            {
                throw new ArgumentException("ini file path is invalid", "path");
            }
            ParseData(file.Read());
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="data"></param>
        protected void ParseData(byte[] data)
        {
            string fileString = Encoding.UTF8.GetString(data);
            fileString = fileString.Replace("\r\n", "\n");
            string[] fileArr = fileString.Split('\n');

            string line;
            string section = string.Empty;
            for (int i = 0; i < fileArr.Length; i++)
            {
                line = fileArr[i].Trim();
                if (line == string.Empty) { continue; }
                if (IsIgnore(line)) { continue; }
                section = ParseSection(section , line);
                ParseKV(section, line);
            }
        }

        protected bool IsIgnore(string line)
        {
            for(int i = 0; i < IGNORE_CHAR.Length; i++)
            {
                if(line.Length <= 0){ return true; }
                if (line[0] == IGNORE_CHAR[i]) { return true; }
            }
            return false;
        }

        protected string ParseSection(string section ,string line)
        {   
            line = line.Trim();
            if(line.Length <= 0){ return section; }
            int start = line.IndexOf('[');
            int end = line.IndexOf(']');
            if (start != 0){ return section; }
            if (end < 0) { return section; }
            if (start == end || end < start) { return section; }
            if (start + 1 == end) { return string.Empty; }
            return line.Substring(start + 1, end - start - 1);

        }

        protected void ParseKV(string section , string line)
        {
            int equal = line.IndexOf('=');
            if (equal <= 0) { return; }

            string key = line.Substring(0, equal);
            string val = (equal >= line.Length) ? string.Empty : line.Substring(equal + 1, line.Length - equal - 1);
            Set(section, key, val);
        }

    }

}