using System;
using CatLib.API;

namespace CatLib
{

    public class App
    {

        protected static IApplication instance;

        public static IApplication Instance
        {
            get
            {
                if (instance == null)
                {
                    #if UNITY_EDITOR
                        if(!UnityEngine.Application.isPlaying){
                            return instance = new Application().Bootstrap(Bootstrap.BootStrap);
                        }
                    #endif
                    throw new NullReferenceException("application not instance");
                }
                return instance;
            }
            set
            {
                if (instance == null)
                {
                    instance = value;
                }
            }
        }
    }

}