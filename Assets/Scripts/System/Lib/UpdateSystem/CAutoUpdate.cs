using UnityEngine;
using System.Collections;
using CatLib.Base;
using CatLib.Support;
using CatLib.FileSystem;
using CatLib.ResourcesSystem;
using CatLib.Container;

namespace CatLib.UpdateSystem
{

    public class CAutoUpdate : CManagerBase
    {

        [CDependency]
        public CApplication Application { get; set; }

        public enum Events {

            ON_UPDATE_LIST_FAILED = 1,

            ON_SCANNING_DISK_FILE_HASH_START = 2,

            ON_SCANNING_DISK_FILE_HASH_END = 3,

            ON_DELETE_DISK_OLD_FILE_START = 4,

            ON_DELETE_DISK_OLD_FIELD_ACTION = 5,

            ON_DELETE_DISK_OLD_FILE_END = 6,

            ON_UPDATE_FILE_START = 7,

            ON_UPDATE_FILE_ACTION = 8,

            ON_UPDATE_FILE_END = 9,

            ON_UPDATE_FILE_FAILD = 10,

            ON_UPDATE_COMPLETE = 11,

        }

        protected bool isUpdate;

        public void Start()
        {
            #region this is test code
            this.UpdateAsset("http://pvp.oss-cn-shanghai.aliyuncs.com");
            #endregion
        }
        
        /// <summary>
        /// 请求更新资源文件
        /// </summary>
        public bool UpdateAsset(string resUrl)
        {
            if (this.isUpdate) { return false; }
            this.isUpdate = true;
            Application.StartCoroutine(this.UpdateList(resUrl));
            return true;

        }

        /// <summary>
        /// 获取文件更新列表
        /// </summary>
        /// <returns></returns>
        protected IEnumerator UpdateList(string resUrl)
        {
            resUrl = resUrl + "/" + CEnv.PlatformToName(CEnv.SwitchPlatform);
            WWW www = new WWW(resUrl + "/" + CUpdateList.FILE_NAME);
            yield return www;

            if (www.text == string.Empty || !string.IsNullOrEmpty(www.error))
            {
                this.isUpdate = false;
                base.Event.Trigger(Events.ON_UPDATE_LIST_FAILED);
                yield break;
            }

            base.Event.Trigger(Events.ON_SCANNING_DISK_FILE_HASH_START);

            var newLst = new CUpdateList(www).SetPath(CEnv.AssetPath);

            CUpdateList oldLst = new CUpdateList(CEnv.AssetPath);

            CDirectory.CreateDir(CEnv.AssetPath);

            CEnv.AssetPath.Walk((file) => {

                if (!file.Standard().EndsWith(".meta"))
                {
                    string fullName = file.Standard();
                    string assetName = fullName.Substring(CEnv.AssetPath.Length);
                    oldLst.Append(assetName, CMD5.ParseFile(file), file.Length);
                }

            });

            base.Event.Trigger(Events.ON_SCANNING_DISK_FILE_HASH_END);

            CUpdateList needUpdateLst, needDeleteLst;
            oldLst.Comparison(newLst, out needUpdateLst, out needDeleteLst);

            yield return this.DeleteOldAsset(needDeleteLst);

            yield return this.UpdateAssetFromUrl(needUpdateLst, resUrl);

            newLst.Save();

            base.Event.Trigger(Events.ON_UPDATE_COMPLETE);

        }

        protected IEnumerator DeleteOldAsset(CUpdateList needDeleteLst)
        {
            base.Event.Trigger(Events.ON_DELETE_DISK_OLD_FILE_START);

            string filePath;
            foreach (CUpdateListField field in needDeleteLst)
            {
                filePath = CEnv.AssetPath + field.Path;
                if (filePath.Exists())
                {
                    base.Event.Trigger(Events.ON_DELETE_DISK_OLD_FIELD_ACTION);
                    filePath.Delete();
                }

            }

            yield return null;

            base.Event.Trigger(Events.ON_DELETE_DISK_OLD_FILE_END);

        }

        protected IEnumerator UpdateAssetFromUrl(CUpdateList needUpdateLst, string downloadUrl)
        {

            string savePath, downloadPath, saveDir;
            base.Event.Trigger(Events.ON_UPDATE_FILE_START);
            foreach (CUpdateListField field in needUpdateLst)
            {
                downloadPath = downloadUrl + field.Path;
                savePath = CEnv.AssetPath + field.Path;

                saveDir = savePath.Substring(0, savePath.LastIndexOf('/'));

                base.Event.Trigger(Events.ON_UPDATE_FILE_ACTION);
                using (WWW www = new WWW(downloadPath))
                {
                    yield return www;
                    if (!string.IsNullOrEmpty(www.error))
                    {
                        base.Event.Trigger(Events.ON_UPDATE_FILE_FAILD);
                        yield break;
                    }
                    CDirectory.CreateDir(saveDir);
                    savePath.Cover(www.bytes, 0, www.bytesDownloaded);
                }

            }

            base.Event.Trigger(Events.ON_UPDATE_FILE_END);

        }
    }

}