using UnityEngine;
using System.Collections;
using CatLib.Base;
using CatLib.Support;
using CatLib.FileSystem;
using CatLib.Container;
using CatLib.Contracts.UpdateSystem;
using UnityEngine.Networking;
using CatLib.Secret;

namespace CatLib.UpdateSystem
{

    public class CAutoUpdate : CComponent , IAutoUpdate
    {

        [CDependency]
        public CConfig Config { get; set; }

        protected bool isUpdate;

        protected int needUpdateNum;
        public int NeedUpdateNum{ get{ return needUpdateNum; } }

        protected int updateNum;
        public int UpdateNum{ get{ return updateNum; } }

        public void UpdateAsset()
        {
            Application.StartCoroutine(this.StartUpdate());
        }

        protected IEnumerator StartUpdate(){

            if (this.isUpdate) { yield break; }
            this.isUpdate = true;

            string resUrl = string.Empty;

            if(Config.IsExists("update.api")){

                string apiUrl = Config.Get<string>("update.api");
                UnityWebRequest request = UnityWebRequest.Get(apiUrl);
                yield return request.Send();
                if (!request.isError && request.responseCode == 200)
                {
                    resUrl = request.downloadHandler.text;
                }

            }

            if(resUrl == string.Empty){

                if(Config.IsExists("update.url")){

                    resUrl = Config.Get<string>("update.url");
                
                }else{

                    base.Event.Trigger(CAutoUpdateEvents.ON_GET_UPDATE_URL_FAILD);
                    yield break;

                }

            }

            yield return this.UpdateList(resUrl);

        }

        /// <summary>
        /// 获取文件更新列表
        /// </summary>
        /// <returns></returns>
        protected IEnumerator UpdateList(string resUrl)
        {
            base.Event.Trigger(CAutoUpdateEvents.ON_UPDATE_START);
            resUrl = resUrl + "/" + CEnv.PlatformToName(CEnv.SwitchPlatform);
            UnityWebRequest request = UnityWebRequest.Get(resUrl + "/" + CUpdateList.FILE_NAME);
            yield return request.Send();
            if (request.isError || request.responseCode != 200)
            {
                this.isUpdate = false;
                base.Event.Trigger(CAutoUpdateEvents.ON_UPDATE_LIST_FAILED);
                yield break;
            }

            base.Event.Trigger(CAutoUpdateEvents.ON_SCANNING_DISK_FILE_HASH_START);

            var newLst = new CUpdateList(request).SetPath(CEnv.AssetPath);

            CUpdateList oldLst = new CUpdateList(CEnv.AssetPath);

            CDirectory.CreateDir(CEnv.AssetPath);

            CDirectory.Walk(CEnv.AssetPath , (file) => {

                if (!file.Standard().EndsWith(".meta"))
                {
                    string fullName = file.Standard();
                    string assetName = fullName.Substring(CEnv.AssetPath.Length);
                    oldLst.Append(assetName, CMD5.ParseFile(file), file.Length);
                }

            });

            base.Event.Trigger(CAutoUpdateEvents.ON_SCANNING_DISK_FILE_HASH_END);

            CUpdateList needUpdateLst, needDeleteLst;
            oldLst.Comparison(newLst, out needUpdateLst, out needDeleteLst);

            yield return this.DeleteOldAsset(needDeleteLst);

            yield return this.UpdateAssetFromUrl(needUpdateLst, resUrl);

            newLst.Save();

            base.Event.Trigger(CAutoUpdateEvents.ON_UPDATE_COMPLETE);

        }

        protected IEnumerator DeleteOldAsset(CUpdateList needDeleteLst)
        {
            base.Event.Trigger(CAutoUpdateEvents.ON_DELETE_DISK_OLD_FILE_START);

            string filePath;
            foreach (CUpdateListField field in needDeleteLst)
            {
                filePath = CEnv.AssetPath + field.Path;
                if (CFile.Exists(filePath))
                {
                    base.Event.Trigger(CAutoUpdateEvents.ON_DELETE_DISK_OLD_FIELD_ACTION);
                    CFile.Delete(filePath);
                }

            }

            yield return null;

            base.Event.Trigger(CAutoUpdateEvents.ON_DELETE_DISK_OLD_FILE_END);

        }

        protected IEnumerator UpdateAssetFromUrl(CUpdateList needUpdateLst, string downloadUrl)
        {

            updateNum = 0;
            needUpdateNum = needUpdateLst.Count();
            string savePath, downloadPath, saveDir;
            base.Event.Trigger(CAutoUpdateEvents.ON_UPDATE_FILE_START);
            foreach (CUpdateListField field in needUpdateLst)
            {
                downloadPath = downloadUrl + field.Path;
                savePath = CEnv.AssetPath + field.Path;

                saveDir = savePath.Substring(0, savePath.LastIndexOf('/'));

                updateNum++;
                base.Event.Trigger(CAutoUpdateEvents.ON_UPDATE_FILE_ACTION);
                
                using (UnityWebRequest request = UnityWebRequest.Get(downloadPath))
                {
                    yield return request.Send();
                    if (request.isError || request.responseCode != 200)
                    {
                        base.Event.Trigger(CAutoUpdateEvents.ON_UPDATE_FILE_FAILD);
                        yield break;
                    }
                    CDirectory.CreateDir(saveDir);
                    CFile.Cover(savePath , request.downloadHandler.data, 0, request.downloadHandler.data.Length);
                }
                
            }

            base.Event.Trigger(CAutoUpdateEvents.ON_UPDATE_FILE_END);

        }
    }

}