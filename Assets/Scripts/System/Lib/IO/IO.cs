
using System;
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

            IDisk disk = null;
            string service = typeof(IDisk).ToString();
            
            Hashtable cloudConfig = null;
            if(Config != null && Config.IsExists(name)){
                cloudConfig = Config.Get<Hashtable>(name);
                if(cloudConfig.ContainsKey("driver")){
                    service = cloudConfig["driver"].ToString();
                }
            }
            if(disk == null){
                disk = App.Make<IDisk>(service);
                if(disk == null){
                    throw new Exception("undefind disk : " + name); 
                }
            }
            if(cloudConfig != null){ disk.SetConfig(cloudConfig); }

            return disk;
            
        }

    }

}