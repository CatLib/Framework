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

using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace CatLib.AutoUpdate
{
    /// <summary>
    /// 更新文件数据
    /// 这个类格式化了更新记录以方便读取和写入
    /// </summary>
    public sealed class UpdateFile
    {
        /// <summary>
        /// 更新文件字段列表
        /// </summary>
        private UpdateFileField[] fields;

        /// <summary>
        /// 更新文件字段列表
        /// </summary>
        private UpdateFileField[] Fields
        {
            get { return fields ?? (fields = new List<UpdateFileField>(pkField.Values).ToArray()); }
        }

        /// <summary>
        /// 主键字段列表
        /// </summary>
        private readonly Dictionary<string, UpdateFileField> pkField = new Dictionary<string, UpdateFileField>();

        /// <summary>
        /// 构建一个更新文件数据
        /// </summary>
        public UpdateFile() { }

        /// <summary>
        /// 构建一个更新文件数据
        /// </summary>
        /// <param name="text">更新文件内容</param>
        public UpdateFile(string text)
        {
            Parse(text);
        }

        /// <summary>
        /// 解析传入的数据
        /// </summary>
        /// <param name="text">更新文件内容</param>
        public void Parse(string text)
        {
            var data = text.Replace("\r\n", "\n").Split('\n');
            foreach (var line in data)
            {
                if (line != string.Empty)
                {
                    Append(new UpdateFileField(line));
                }
            }
        }

        /// <summary>
        /// 查找一个更新文件字段
        /// </summary>
        /// <param name="pk">主键</param>
        /// <returns>更新文件字段</returns>
        public UpdateFileField Find(string pk)
        {
            if (pkField.ContainsKey(pk))
            {
                return pkField[pk];
            }
            return null;
        }

        /// <summary>
        /// 将更新文件数据表转为字符串形式的数据
        /// </summary>
        public string Data
        {
            get
            {
                var returnStr = string.Empty;
                foreach (UpdateFileField field in this)
                {
                    returnStr += field.ToString();
                }
                return returnStr;
            }
        }

        /// <summary>
        /// 更新文件数据的字段数量
        /// </summary>
        public int Count
        {
            get
            {
                return pkField.Count;
            }
        }

        /// <summary>
        /// 增加一条更新文件记录
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="md5">文件md5值</param>
        /// <param name="size">文件大小(字节)</param>
        /// <returns>当前对象实例</returns>
        public UpdateFile Append(string path, string md5, long size)
        {
            return Append(new UpdateFileField(path, md5, size));
        }

        /// <summary>
        /// 增加一条更新文件记录
        /// </summary>
        /// <param name="lst">更新文件字段</param>
        /// <returns>当前对象实例</returns>
        public UpdateFile Append(UpdateFileField lst)
        {
            pkField.Add(lst.Path, lst);
            fields = null;
            return this;
        }

        /// <summary>
        /// 迭代器
        /// </summary>
        /// <returns>迭代器</returns>
        public IEnumerator GetEnumerator()
        {
            return Fields.GetEnumerator();
        }

        /// <summary>
        /// 和新的列表比对，筛选出需要更新的内容和需要删除的内容
        /// </summary>
        /// <param name="newLst">新的更新文件数据的列表</param>
        /// <param name="needUpdate">需要更新的文件列表</param>
        /// <param name="needDelete">需要删除的文件列表</param>
        public void Comparison(UpdateFile newLst, out UpdateFile needUpdate, out UpdateFile needDelete)
        {
            needUpdate = new UpdateFile();
            needDelete = new UpdateFile();

            UpdateFileField oldField;
            foreach (UpdateFileField newField in newLst)
            {
                oldField = Find(newField.Path);
                if (oldField != null)
                {
                    if (oldField.MD5 != newField.MD5)
                    {
                        needUpdate.Append(newField);
                    }
                }
                else
                {
                    needUpdate.Append(newField);
                }
            }

            foreach (UpdateFileField old in this)
            {
                if (newLst.Find(old.Path) == null && old.Path != Path.AltDirectorySeparatorChar + UpdateFileStore.FILE_NAME)
                {
                    needDelete.Append(old);
                }
            }
        }
    }
}