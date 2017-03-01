
using System.Collections;
using CatLib.API.IO;

namespace CatLib.IO
{

    /// <summary>
    /// 文件服务
    /// </summary>
    public class IO : Component , IIOFactory
    {

        [Dependency]
        public Configs Config{ get; set; }

        public IDisk Disk(string name = null){

            if(name == null || Config == null || !Config.IsExists(name)){

                return App.Make(typeof(IDisk)) as IDisk;

            }

            var cloudConfig = Config.Get<Hashtable>(name);
            string service = typeof(IDisk).ToString();
            IDisk disk = null;
            if(cloudConfig.ContainsKey("service")){
                service = cloudConfig["service"].ToString();
                disk = App.Make(service) as IDisk;
                if(disk == null){ 
                    disk = App.Make(typeof(IDisk)) as IDisk; 
                }
            }else{
                disk = App.Make(service) as IDisk;
            }
            if(disk != null){ disk.SetConfig(cloudConfig); }
            return disk;
            
        }

    }

}