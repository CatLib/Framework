using UnityEngine;
using System.Collections;
using CatLib.Contracts.Base;

namespace CapLib.Base
{

    public class CApp
    {

        protected static IApplication instance;

        public static IApplication Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new CNullReferenceException("application not instance");
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