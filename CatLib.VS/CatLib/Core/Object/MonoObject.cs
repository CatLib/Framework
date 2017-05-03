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

using UnityEngine;
using CatLib.API;

namespace CatLib
{
    /// <summary>
    /// CatLib Mono Object
    /// </summary>
    public class MonoObject : MonoBehaviour, IGuid
    {
        /// <summary>
        /// Transform
        /// </summary>
        private Transform tran;

        /// <summary>
        /// GameObject
        /// </summary>
        private GameObject obj;

        /// <summary>
        /// Guid
        /// </summary>
        private long guid;

        /// <summary>
        /// Transform
        /// </summary>
        public Transform Transform
        {
            get
            {
                if (!tran)
                {
                    tran = transform;
                }
                return tran;
            }
        }

        /// <summary>
        /// GameObject
        /// </summary>
        public GameObject GameObject
        {
            get
            {
                if (!obj)
                {
                    obj = gameObject;
                }
                return obj;
            }
        }

        /// <summary>
        /// 唯一标识符
        /// </summary>
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
    }
}