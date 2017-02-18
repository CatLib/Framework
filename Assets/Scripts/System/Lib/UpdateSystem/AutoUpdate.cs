using System.Collections;
using UnityEngine.Networking;
using CatLib.Secret;
using CatLib.Contracts.UpdateSystem;
using CatLib.Contracts.IO;

namespace CatLib.UpdateSystem
{

    public class AutoUpdate : Component , IAutoUpdate
    {

        [Dependency]
        public Configs Config { get; set; }

        [Dependency]
        public IFile File { get; set; }

        [Dependency]
        public IDirectory Directory { get; set; }

        protected bool isUpdate;

        protected int needUpdateNum;
        public int NeedUpdateNum{ get{ return needUpdateNum; } }

        protected int updateNum;
        public int UpdateNum{ get{ return updateNum; } }

        public IEnumerator UpdateAsset()
        {
            return StartUpdate();
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

                    base.Event.Trigger(AutoUpdateEvents.ON_GET_UPDATE_URL_FAILD);
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
            base.Event.Trigger(AutoUpdateEvents.ON_UPDATE_START);
            resUrl = resUrl + "/" + Env.PlatformToName(Env.SwitchPlatform);
            UnityWebRequest request = UnityWebRequest.Get(resUrl + "/" + UpdateSystem.UpdateList.FILE_NAME);
            yield return request.Send();
            if (request.isError || request.responseCode != 200)
            {
                this.isUpdate = false;
                base.Event.Trigger(AutoUpdateEvents.ON_UPDATE_LIST_FAILED);
                yield break;
            }

            base.Event.Trigger(AutoUpdateEvents.ON_SCANNING_DISK_FILE_HASH_START);

            var newLst = new UpdateList(request).SetPath(Env.AssetPath);

            UpdateList oldLst = new UpdateList(Env.AssetPath);

            Directory.Create(Env.AssetPath);

            Directory.Walk(Env.AssetPath , (file) => {

                if (!file.FullName.Standard().EndsWith(".meta"))
                {
                    string fullName = file.FullName.Standard();
                    string assetName = fullName.Substring(Env.AssetPath.Length);
                    oldLst.Append(assetName, MD5.ParseFile(file), file.Length);
                }

            });

            base.Event.Trigger(AutoUpdateEvents.ON_SCANNING_DISK_FILE_HASH_END);

            UpdateList needUpdateLst, needDeleteLst;
            oldLst.Comparison(newLst, out needUpdateLst, out needDeleteLst);

            yield return this.DeleteOldAsset(needDeleteLst);

            yield return this.UpdateAssetFromUrl(needUpdateLst, resUrl);

            newLst.Save();

            base.Event.Trigger(AutoUpdateEvents.ON_UPDATE_COMPLETE);

        }

        protected IEnumerator DeleteOldAsset(UpdateList needDeleteLst)
        {
            base.Event.Trigger(AutoUpdateEvents.ON_DELETE_DISK_OLD_FILE_START);

            string filePath;
            foreach (UpdateListField field in needDeleteLst)
            {
                filePath = Env.AssetPath + field.Path;
                if (File.Exists(filePath))
                {
                    base.Event.Trigger(AutoUpdateEvents.ON_DELETE_DISK_OLD_FIELD_ACTION);
                    File.Delete(filePath);
                }

            }

            yield return null;

            base.Event.Trigger(AutoUpdateEvents.ON_DELETE_DISK_OLD_FILE_END);

        }

        protected IEnumerator UpdateAssetFromUrl(UpdateList needUpdateLst, string downloadUrl)
        {

            updateNum = 0;
            needUpdateNum = needUpdateLst.Count();
            string savePath, downloadPath, saveDir;
            base.Event.Trigger(AutoUpdateEvents.ON_UPDATE_FILE_START);
            foreach (UpdateListField field in needUpdateLst)
            {
                downloadPath = downloadUrl + field.Path;
                savePath = Env.AssetPath + field.Path;

                saveDir = savePath.Substring(0, savePath.LastIndexOf('/'));

                updateNum++;
                base.Event.Trigger(AutoUpdateEvents.ON_UPDATE_FILE_ACTION);
                
                using (UnityWebRequest request = UnityWebRequest.Get(downloadPath))
                {
                    yield return request.Send();
                    if (request.isError || request.responseCode != 200)
                    {
                        base.Event.Trigger(AutoUpdateEvents.ON_UPDATE_FILE_FAILD);
                        yield break;
                    }
                    Directory.Create(saveDir);
                    File.Create(savePath , request.downloadHandler.data, 0, request.downloadHandler.data.Length);
                }
                
            }

            base.Event.Trigger(AutoUpdateEvents.ON_UPDATE_FILE_END);

        }
    }

}