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
using CatLib.API.INI;
using System.Collections.Generic;
using System.Text;

namespace CatLib.INI
{
    /// <summary>
    /// ini结果集
    /// </summary>
    public sealed class IniResult : IIniResult
    {
        /// <summary>
        /// 忽略的符号
        /// </summary>
        private static readonly char[] IGNORE_CHAR = { '#', ';' };

        /// <summary>
        /// 当存储时
        /// </summary>
        private event Action<string> onSave;

        /// <summary>
        /// 当存储时
        /// </summary>
        public event Action<string> OnSave
        {
            add { onSave += value; }
            remove { onSave -= value; }
        }

        /// <summary>
        /// ini字典
        /// </summary>
        private readonly Dictionary<string, Dictionary<string, string>> iniDict = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// 构造一个ini结果集
        /// </summary>
        /// <param name="data">要被解析的数据</param>
        public IniResult(byte[] data)
        {
            ParseData(data);
        }

        /// <summary>
        /// 构造一个ini结果集
        /// </summary>
        /// <param name="data">要被解析的数据</param>
        public IniResult(string data)
        {
            ParseData(data);
        }

        /// <summary>
        /// 获取一个ini配置
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        /// <param name="def">默认值</param>
        /// <returns>配置值</returns>
        public string Get(string section, string key, string def = null)
        {
            if (!iniDict.ContainsKey(section))
            {
                return def;
            }
            return iniDict[section].ContainsKey(key) ? iniDict[section][key] : def;
        }

        /// <summary>
        /// 设定一个ini配置
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        public void Set(string section, string key, string val)
        {
            if (key == string.Empty)
            {
                return;
            }
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
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        public void Remove(string section, string key)
        {
            if (!iniDict.ContainsKey(section))
            {
                return;
            }
            iniDict[section].Remove(key);
            if (iniDict[section].Count <= 0)
            {
                iniDict.Remove(section);
            }
        }

        /// <summary>
        /// 移除一个ini区块
        /// </summary>
        /// <param name="section">节</param>
        public void Remove(string section)
        {
            iniDict.Remove(section);
        }

        /// <summary>
        /// 保存ini文件
        /// </summary>
        public void Save()
        {
            var data = new StringBuilder();
            foreach (var dict in iniDict)
            {
                data.AppendLine("[" + dict.Key + "]");
                foreach (var kv in dict.Value)
                {
                    data.AppendLine(kv.Key + "=" + kv.Value);
                }
            }

            if (onSave != null)
            {
                onSave.Invoke(data.ToString());
            }
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="data">要被解析的数据</param>
        private void ParseData(byte[] data)
        {
            var fileString = Encoding.UTF8.GetString(data);
            ParseData(fileString);
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="data">要被解析的数据</param>
        private void ParseData(string data)
        {

            data = data.Replace("\r\n", "\n");
            var fileArr = data.Split('\n');

            var section = string.Empty;
            string line;
            for (var i = 0; i < fileArr.Length; i++)
            {
                line = fileArr[i].Trim();
                if (line == string.Empty || IsIgnore(line))
                {
                    continue;
                }
                section = ParseSection(section, line);
                ParseKV(section, line);
            }
        }

        /// <summary>
        /// 是否忽略这一行
        /// </summary>
        /// <param name="line">当前行的数据</param>
        /// <returns>是否忽略</returns>
        private bool IsIgnore(string line)
        {
            for (var i = 0; i < IGNORE_CHAR.Length; i++)
            {
                if (line.Length <= 0)
                {
                    return true;
                }
                if (line[0] == IGNORE_CHAR[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 解析节
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="line">当前行的数据</param>
        /// <returns>节</returns>
        private string ParseSection(string section, string line)
        {
            line = line.Trim();
            if (line.Length <= 0)
            {
                return section;
            }
            var start = line.IndexOf('[');
            var end = line.IndexOf(']');
            if (start != 0)
            {
                return section;
            }
            if (end < 0)
            {
                return section;
            }
            if (start == end || end < start)
            {
                return section;
            }
            return start + 1 == end ? string.Empty : line.Substring(start + 1, end - start - 1);
        }

        /// <summary>
        /// 解析key value
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="line">当前行的数据</param>
        private void ParseKV(string section, string line)
        {
            var equal = line.IndexOf('=');
            if (equal <= 0)
            {
                return;
            }
            var key = line.Substring(0, equal);
            var val = (equal >= line.Length) ? string.Empty : line.Substring(equal + 1, line.Length - equal - 1);
            Set(section, key, val);
        }
    }
}