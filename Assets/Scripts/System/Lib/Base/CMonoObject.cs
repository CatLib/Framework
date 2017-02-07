using UnityEngine;
using System.Collections;
using CapLib.Base;
using XLua;

namespace CatLib.Base
{

    /// <summary>
    /// CatLib Mono Object
    /// </summary>
    public class CMonoObject : MonoBehaviour
    {

        protected Transform tran;

        /// <summary>
        /// Transform
        /// </summary>
        public Transform Transform
        {
            get
            {
                if (!tran) { tran = transform; }
                return tran;
            }
        }


        protected GameObject obj;

        /// <summary>
        /// GameObject
        /// </summary>
        public GameObject GameObject
        {
            get
            {
                if (!obj) { obj = gameObject; }
                return obj;
            }
        }

        private long guid;

        public long Guid
        {
            get
            {

                if (guid <= 0)
                {
                    guid = CApp.Instance.GetGuid();
                }
                return guid;
            }
        }
        public string TypeGuid
        {
            get
            {
                return GetType().ToString() + "-" + Guid;
            }
        }


    }

}