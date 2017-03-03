using System.Collections;
using UnityEngine.Networking;
using System.IO;
using CatLib.API;
using CatLib.API.AutoUpdate;
using CatLib.API.IO;
using CatLib.API.Hash;

namespace CatLib.AutoUpdate
{

    public class AutoUpdate : Component , IAutoUpdate
    {

        [Dependency]
        public Configs Config { get; set; }

        [Dependency]
        public IIOFactory IO { get; set; }

        [Dependency]
        public IHash Hash { get; set; }

        [Dependency]
        public IEnv Env { get; set; }

        private IDisk disk;

        /// <summary>
        /// 磁盘
        /// </summary>
        private IDisk Disk{

            get{
                return disk ?? (disk = IO.Disk());
            }
        }

        protected bool isUpdate;

        protected int needUpdateNum;
        public int NeedUpdateNum{ get{ return needUpdateNum; } }

        protected int updateNum;
        public int UpdateNum{ get{ return updateNum; } }

        public IEnumerator UpdateAsset()
        {
            if (Env.DebugLevel == DebugLevels.Staging)
            {
                return JumpUpdate();
            }
            return StartUpdate();
        }

        protected IEnumerator JumpUpdate(){

            yield break;

        }

        protected IEnumerator StartUpdate(){

            if (this.isUpdate) { yield break; }
            this.isUpdate = true;

            string resUrl = string.Empty;

            if(Config == null){ yield break; }

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
            resUrl = resUrl + Path.AltDirectorySeparatorChar + Env.PlatformToName(Env.SwitchPlatform);
            UnityWebRequest request = UnityWebRequest.Get(resUrl + Path.AltDirectorySeparatorChar + UpdateFileStore.FILE_NAME);
            yield return request.Send();
            if (request.isError || request.responseCode != 200)
            {
                this.isUpdate = false;
                base.Event.Trigger(AutoUpdateEvents.ON_UPDATE_LIST_FAILED);
                yield break;
            }

            base.Event.Trigger(AutoUpdateEvents.ON_SCANNING_DISK_FILE_HASH_START);

            var fileStore = new UpdateFileStore();
            var newLst = fileStore.LoadFromBytes(request.downloadHandler.data);
            var oldLst = new UpdateFile(); //fileStore.LoadFromPath(Env.AssetPath);

            Disk.Root.Create();
            Disk.Root.Walk((file) => {

                if (!file.FullName.Standard().EndsWith(".meta"))
                {
                    string fullName = file.FullName.Standard();
                    string assetName = fullName.Substring(Env.AssetPath.Length);
                    oldLst.Append(assetName, Hash.FileHash(file.FullName), file.Length);
                }

            });

            base.Event.Trigger(AutoUpdateEvents.ON_SCANNING_DISK_FILE_HASH_END);

            UpdateFile needUpdateLst, needDeleteLst;
            oldLst.Comparison(newLst, out needUpdateLst, out needDeleteLst);

            yield return this.DeleteOldAsset(needDeleteLst);

            yield return this.UpdateAssetFromUrl(needUpdateLst, resUrl);

            fileStore.Save(Env.AssetPath , newLst);

            base.Event.Trigger(AutoUpdateEvents.ON_UPDATE_COMPLETE);

        }

        protected IEnumerator DeleteOldAsset(UpdateFile needDeleteLst)
        {

            base.Event.Trigger(AutoUpdateEvents.ON_DELETE_DISK_OLD_FILE_START);

            IFile file;
            foreach (UpdateFileField field in needDeleteLst)
            {
                file = Disk.File(Env.AssetPath + field.Path, PathTypes.Absolute);
                if (file.Exists)
                {
                    base.Event.Trigger(AutoUpdateEvents.ON_DELETE_DISK_OLD_FIELD_ACTION);
                    file.Delete();
                }

            }

            yield return null;

            base.Event.Trigger(AutoUpdateEvents.ON_DELETE_DISK_OLD_FILE_END);

        }

        protected IEnumerator UpdateAssetFromUrl(UpdateFile needUpdateLst, string downloadUrl)
        {

            updateNum = 0;
            needUpdateNum = needUpdateLst.Count;
            string savePath, downloadPath, saveDir;
            base.Event.Trigger(AutoUpdateEvents.ON_UPDATE_FILE_START);
            foreach (UpdateFileField field in needUpdateLst)
            {
                downloadPath = downloadUrl + field.Path;
                savePath = Env.AssetPath + field.Path;

                saveDir = savePath.Substring(0, savePath.LastIndexOf(Path.AltDirectorySeparatorChar));

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
                    Disk.Directory(saveDir, PathTypes.Absolute).Create();
                    IFile saveFile = Disk.File(savePath, PathTypes.Absolute);
                    saveFile.Create(request.downloadHandler.data);
                }
                
            }

            base.Event.Trigger(AutoUpdateEvents.ON_UPDATE_FILE_END);

        }
    }

}