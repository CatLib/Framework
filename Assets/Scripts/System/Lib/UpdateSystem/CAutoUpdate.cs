using UnityEngine;
using System.Collections;
using CatLib.Base;
using CatLib.Support;
using CatLib.FileSystem;
using CatLib.ResourcesSystem;
using CatLib.Container;
using CatLib.Contracts.UpdateSystem;
using UnityEngine.Networking;

namespace CatLib.UpdateSystem
{

    public class CAutoUpdate : CComponent , IAutoUpdate
    {

        [CDependency]
        public CApplication Application { get; set; }

        [CDependency]
        public CConfig Config { get; set; }

        public class Events {

            public static string ON_UPDATE_LIST_FAILED = "autoupdate.update.list.falid";

            public static string ON_SCANNING_DISK_FILE_HASH_START = "autoupdate.disk.file.hash.start";

            public static string ON_SCANNING_DISK_FILE_HASH_END = "autoupdate.disk.file.hash.end";

            public static string ON_DELETE_DISK_OLD_FILE_START = "autoupdate.disk.delete.old.file.start";

            public static string ON_DELETE_DISK_OLD_FIELD_ACTION = "autoupdate.disk.delete.old.file.action";

            public static string ON_DELETE_DISK_OLD_FILE_END = "autoupdate.disk.delete.file.end";

            public static string ON_UPDATE_FILE_START = "autoupdate.update.file.start";

            public static string ON_UPDATE_FILE_ACTION = "autoupdate.update.file.action";

            public static string ON_UPDATE_FILE_END = "autoupdate.update.file.end";

            public static string ON_UPDATE_FILE_FAILD = "autoupdate.update.file.faild";

            public static string ON_UPDATE_COMPLETE = "autoupdate.uupdate.complete";

        }

        protected bool isUpdate;

        public bool UpdateAsset()
        {
            string[] assetUrl = Config.Get<string[]>("update.url");
            return UpdateAsset(assetUrl[Random.Range(0, assetUrl.Length)]);
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
            UnityWebRequest request = UnityWebRequest.Get(resUrl + "/" + CUpdateList.FILE_NAME);
            yield return request.Send();
            if (request.downloadHandler.text == string.Empty || !string.IsNullOrEmpty(request.error) || request.responseCode != 200)
            {
                Debug.Log(request.downloadHandler.text);
                this.isUpdate = false;
                base.Event.Trigger(Events.ON_UPDATE_LIST_FAILED);
                yield break;
            }

            base.Event.Trigger(Events.ON_SCANNING_DISK_FILE_HASH_START);

            var newLst = new CUpdateList(request).SetPath(CEnv.AssetPath);

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

                ;

                using (UnityWebRequest request = UnityWebRequest.Get(downloadPath))
                {
                    yield return request.Send();
                    if (!string.IsNullOrEmpty(request.error) || request.responseCode != 200)
                    {
                        base.Event.Trigger(Events.ON_UPDATE_FILE_FAILD);
                        yield break;
                    }
                    CDirectory.CreateDir(saveDir);
                    savePath.Cover(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
                }

            }

            base.Event.Trigger(Events.ON_UPDATE_FILE_END);

        }
    }

}