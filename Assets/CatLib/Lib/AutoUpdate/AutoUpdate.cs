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
using UnityEngine.Networking;
using System.IO;
using CatLib.API;
using CatLib.API.AutoUpdate;
using CatLib.API.IO;
using CatLib.API.Hash;
using UnityEngine.Experimental.Networking;

namespace CatLib.AutoUpdate
{

    public class AutoUpdate : IAutoUpdate
    {

        [Dependency]
        public IIOFactory IO { get; set; }

        [Dependency]
        public IHash Hash { get; set; }

        [Dependency]
        public IEnv Env { get; set; }

        [Dependency]
        public IApplication App { get; set; }

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


        #region Config

        private string updateAPI;

        public void SetUpdateAPI(string api){

            if(!string.IsNullOrEmpty(api)){
                
                updateAPI = api;
            
            }

        }

        private string updateURL;

        public void SetUpdateURL(string url){

            if(!string.IsNullOrEmpty(url)){
                
                updateURL = url;
            
            }

        }

        #endregion


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

        protected IEnumerator JumpUpdate(){

            yield break;

        }

        protected IEnumerator StartUpdate(){

            if (isUpdate) { yield break; }
            isUpdate = true;

            string resUrl = string.Empty;

            if(updateAPI != null){

                UnityWebRequest request = UnityWebRequest.Get(updateAPI);
                yield return request.Send();
                if (!request.isError && request.responseCode == 200)
                {
                    resUrl = request.downloadHandler.text;
                }

            }

            if(resUrl == string.Empty){

                if(updateURL != null){

                    resUrl = updateURL;
                
                }else{

                    App.Trigger(this).SetEventName(AutoUpdateEvents.ON_GET_UPDATE_URL_FAILD)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();
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
            App.Trigger(this).SetEventName(AutoUpdateEvents.ON_UPDATE_START)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();
            resUrl = resUrl + Path.AltDirectorySeparatorChar + Env.PlatformToName(Env.SwitchPlatform);
            UnityWebRequest request = UnityWebRequest.Get(resUrl + Path.AltDirectorySeparatorChar + UpdateFileStore.FILE_NAME);
            yield return request.Send();
            if (request.isError || request.responseCode != 200)
            {
                isUpdate = false;
                App.Trigger(this).SetEventName(AutoUpdateEvents.ON_UPDATE_LIST_FAILED)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();
                yield break;
            }

            App.Trigger(this).SetEventName(AutoUpdateEvents.ON_SCANNING_DISK_FILE_HASH_START)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();

            var fileStore = App.Make<UpdateFileStore>();
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

            App.Trigger(this).SetEventName(AutoUpdateEvents.ON_SCANNING_DISK_FILE_HASH_END)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();

            UpdateFile needUpdateLst, needDeleteLst;
            oldLst.Comparison(newLst, out needUpdateLst, out needDeleteLst);

            yield return this.DeleteOldAsset(needDeleteLst);

            yield return this.UpdateAssetFromUrl(needUpdateLst, resUrl);

            fileStore.Save(Env.AssetPath , newLst);

            App.Trigger(this).SetEventName(AutoUpdateEvents.ON_UPDATE_COMPLETE)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();

        }

        protected IEnumerator DeleteOldAsset(UpdateFile needDeleteLst)
        {

            App.Trigger(this).SetEventName(AutoUpdateEvents.ON_DELETE_DISK_OLD_FILE_START)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();

            IFile file;
            foreach (UpdateFileField field in needDeleteLst)
            {
                file = Disk.File(Env.AssetPath + field.Path, PathTypes.Absolute);
                if (file.Exists)
                {
                    App.Trigger(this).SetEventName(AutoUpdateEvents.ON_DELETE_DISK_OLD_FIELD_ACTION)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();
                    file.Delete();
                }
            }

            yield return null;

            App.Trigger(this).SetEventName(AutoUpdateEvents.ON_DELETE_DISK_OLD_FILE_END)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();

        }

        protected IEnumerator UpdateAssetFromUrl(UpdateFile needUpdateLst, string downloadUrl)
        {

            string savePath, downloadPath, saveDir;

            int i = 0;
            string[] updatePath = new string[needUpdateLst.Count];
            foreach (UpdateFileField field in needUpdateLst)
            {
                updatePath[i++] = field.Path;
            }

            App.Trigger(this).SetEventName(AutoUpdateEvents.ON_UPDATE_FILE_START)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger(new UpdateFileStartEventArgs(updatePath));

            for(i = 0; i < updatePath.Length; i++)
            {

                downloadPath = downloadUrl + Path.AltDirectorySeparatorChar + updatePath[i];
                savePath = Env.AssetPath + Path.AltDirectorySeparatorChar + updatePath[i];

                saveDir = savePath.Substring(0, savePath.LastIndexOf(Path.AltDirectorySeparatorChar));

                using (UnityWebRequest request = UnityWebRequest.Get(downloadPath))
                {
                    App.Trigger(this).SetEventName(AutoUpdateEvents.ON_UPDATE_FILE_ACTION)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger(new UpdateFileActionEventArgs(updatePath[i], request));
                    yield return request.Send();
                    if (request.isError || request.responseCode != 200)
                    {
                        App.Trigger(this).SetEventName(AutoUpdateEvents.ON_UPDATE_FILE_FAILD)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();
                        yield break;
                    }
                    Disk.Directory(saveDir, PathTypes.Absolute).Create();
                    IFile saveFile = Disk.File(savePath, PathTypes.Absolute);
                    saveFile.Create(request.downloadHandler.data);
                }
                
            }
            App.Trigger(this).SetEventName(AutoUpdateEvents.ON_UPDATE_FILE_END)
                                     .SetEventLevel(EventLevel.Global)
                                     .Trigger();

        }
    }

}