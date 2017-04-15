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
#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#elif UNITY_5_2 || UNITY_5_3
using UnityEngine.Experimental.Networking;
#endif
using System.IO;
using CatLib.API;
using CatLib.API.AutoUpdate;
using CatLib.API.IO;
using CatLib.API.Hash;

namespace CatLib.AutoUpdate
{
    /// <summary>
    /// 自动更新
    /// </summary>
    public sealed class AutoUpdate : IAutoUpdate
    {
        /// <summary>
        /// 文件存储服务
        /// </summary>
        [Dependency]
        public IIOFactory IO { get; set; }

        /// <summary>
        /// 哈希服务
        /// </summary>
        [Dependency]
        public IHash Hash { get; set; }

        /// <summary>
        /// 环境配置
        /// </summary>
        [Dependency]
        public IEnv Env { get; set; }

        /// <summary>
        /// 应用程序实例
        /// </summary>
        [Dependency]
        public IApplication App { get; set; }

        /// <summary>
        /// 磁盘
        /// </summary>
        private IDisk disk;

        /// <summary>
        /// 磁盘
        /// </summary>
        private IDisk Disk
        {
            get
            {
                return disk ?? (disk = IO.Disk());
            }
        }

        /// <summary>
        /// 是否开始更新
        /// </summary>
        private bool isUpdate;

        #region Config

        /// <summary>
        /// 获取更新目录的api地址
        /// </summary>
        private string updateAPI;

        /// <summary>
        /// 更新的请求url
        /// </summary>
        private string updateURL;

        /// <summary>
        /// 设定一个api请求地址用于获取更新目录
        /// </summary>
        /// <param name="api">请求api</param>
        public void SetUpdateAPI(string api)
        {
            if (!string.IsNullOrEmpty(api))
            {
                updateAPI = api;
            }
        }

        /// <summary>
        /// 设定更新的请求url
        /// </summary>
        /// <param name="url">更新的url</param>
        public void SetUpdateURL(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                updateURL = url;
            }
        }

        #endregion

        /// <summary>
        /// 更新Asset
        /// </summary>
        /// <returns>迭代器</returns>
        public IEnumerator UpdateAsset()
        {
            //Stading模式下资源目录会被定位到发布文件目录所以不能进行热更新
#if UNITY_EDITOR
            if (Env.DebugLevel == DebugLevels.Staging)
            {
                return JumpUpdate();
            }
            if (Env.DebugLevel == DebugLevels.Auto || Env.DebugLevel == DebugLevels.Dev)
            {
                return JumpUpdate();
            }
#endif
            return StartUpdate();
        }

        /// <summary>
        /// 跳过更新的迭代器
        /// </summary>
        /// <returns>迭代器</returns>
        private IEnumerator JumpUpdate()
        {
            yield break;
        }

        /// <summary>
        /// 启动更新
        /// </summary>
        /// <returns>迭代器</returns>
        private IEnumerator StartUpdate()
        {
            if (isUpdate)
            {
                yield break;
            }

            isUpdate = true;

            var resUrl = string.Empty;

            if (updateAPI != null)
            {
                var request = UnityWebRequest.Get(updateAPI);
                yield return request.Send();
                if (!request.isError && request.responseCode == 200)
                {
                    resUrl = request.downloadHandler.text;
                }
            }

            if (resUrl == string.Empty)
            {
                if (updateURL != null)
                {
                    resUrl = updateURL;
                }
                else
                {
                    App.TriggerGlobal(AutoUpdateEvents.ON_GET_UPDATE_URL_FAILD, this)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();
                    yield break;
                }
            }

            yield return UpdateList(resUrl);
        }

        /// <summary>
        /// 获取文件更新列表
        /// </summary>
        /// <param name="resUrl">更新url</param>
        /// <returns>迭代器</returns>
        private IEnumerator UpdateList(string resUrl)
        {
            App.TriggerGlobal(AutoUpdateEvents.ON_UPDATE_START, this)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();

            resUrl = resUrl + Path.AltDirectorySeparatorChar + Env.PlatformToName(Env.SwitchPlatform);
            var request = UnityWebRequest.Get(resUrl + Path.AltDirectorySeparatorChar + UpdateFileStore.FILE_NAME);

            yield return request.Send();

            if (request.isError || request.responseCode != 200)
            {
                isUpdate = false;
                App.TriggerGlobal(AutoUpdateEvents.ON_UPDATE_LIST_FAILED, this)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();
                yield break;
            }

            App.TriggerGlobal(AutoUpdateEvents.ON_SCANNING_DISK_FILE_HASH_START, this)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();

            var fileStore = App.Make<UpdateFileStore>();
            var newLst = fileStore.LoadFromBytes(request.downloadHandler.data);
            var oldLst = new UpdateFile(); //fileStore.LoadFromPath(Env.AssetPath);

            Disk.Root.Create();
            Disk.Root.Walk((file) =>
            {
                if (Util.StandardPath(file.FullName).EndsWith(".meta"))
                {
                    return;
                }
                var fullName = Util.StandardPath(file.FullName);
                var assetName = fullName.Substring(Env.AssetPath.Length);
                oldLst.Append(assetName, Hash.FileMd5(file.FullName), file.Length);
            });

            App.TriggerGlobal(AutoUpdateEvents.ON_SCANNING_DISK_FILE_HASH_END, this)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();

            UpdateFile needUpdateLst, needDeleteLst;
            oldLst.Comparison(newLst, out needUpdateLst, out needDeleteLst);

            yield return DeleteOldAsset(needDeleteLst);

            yield return UpdateAssetFromUrl(needUpdateLst, resUrl);

            fileStore.Save(Env.AssetPath, newLst);

            App.TriggerGlobal(AutoUpdateEvents.ON_UPDATE_COMPLETE, this)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();
        }

        /// <summary>
        /// 删除旧的资源
        /// </summary>
        /// <param name="needDeleteLst">旧资源列表</param>
        /// <returns>迭代器</returns>
        private IEnumerator DeleteOldAsset(UpdateFile needDeleteLst)
        {
            App.TriggerGlobal(AutoUpdateEvents.ON_DELETE_DISK_OLD_FILE_START, this)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();
            IFile file;
            foreach (UpdateFileField field in needDeleteLst)
            {
                file = Disk.File(Env.AssetPath + field.Path, PathTypes.Absolute);
                if (!file.Exists)
                {
                    continue;
                }
                App.TriggerGlobal(AutoUpdateEvents.ON_DELETE_DISK_OLD_FIELD_ACTION, this)
                    .SetEventLevel(EventLevel.Global)
                    .Trigger();
                file.Delete();
            }

            yield return null;

            App.TriggerGlobal(AutoUpdateEvents.ON_DELETE_DISK_OLD_FILE_END, this)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();
        }

        /// <summary>
        /// 通过url更新资源
        /// </summary>
        /// <param name="needUpdateLst">需要更新的列表</param>
        /// <param name="downloadUrl">下载列表</param>
        /// <returns>迭代器</returns>
        private IEnumerator UpdateAssetFromUrl(UpdateFile needUpdateLst, string downloadUrl)
        {
            string savePath, downloadPath, saveDir;

            var i = 0;
            var updatePath = new string[needUpdateLst.Count];
            foreach (UpdateFileField field in needUpdateLst)
            {
                updatePath[i++] = field.Path;
            }

            App.TriggerGlobal(AutoUpdateEvents.ON_UPDATE_FILE_START, this)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger(new UpdateFileStartEventArgs(updatePath));

            for (i = 0; i < updatePath.Length; i++)
            {
                downloadPath = downloadUrl + Path.AltDirectorySeparatorChar + updatePath[i];
                savePath = Env.AssetPath + Path.AltDirectorySeparatorChar + updatePath[i];

                saveDir = savePath.Substring(0, savePath.LastIndexOf(Path.AltDirectorySeparatorChar));

                using (var request = UnityWebRequest.Get(downloadPath))
                {
                    App.TriggerGlobal(AutoUpdateEvents.ON_UPDATE_FILE_ACTION, this)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger(new UpdateFileActionEventArgs(updatePath[i], request));

                    yield return request.Send();

                    if (request.isError || request.responseCode != 200)
                    {
                        App.TriggerGlobal(AutoUpdateEvents.ON_UPDATE_FILE_FAILD, this)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();
                        yield break;
                    }

                    Disk.Directory(saveDir, PathTypes.Absolute).Create();
                    var saveFile = Disk.File(savePath, PathTypes.Absolute);
                    saveFile.Create(request.downloadHandler.data);
                }
            }

            App.TriggerGlobal(AutoUpdateEvents.ON_UPDATE_FILE_END, this)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();
        }
    }
}