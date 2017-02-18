using System;
using CatLib.Contracts;

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