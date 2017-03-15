using UnityEngine;

namespace CatLib
{

    /// <summary>
    /// CatLib Mono Object
    /// </summary>
    public class MonoObject : MonoBehaviour
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
                    guid = App.Instance.GetGuid();
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