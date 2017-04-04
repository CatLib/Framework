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

namespace CatLib
{

    /// <summary>
    /// 驱动脚本
    /// </summary>
    public class DriverBehaviour : MonoBehaviour
    {

        /// <summary>
        /// 驱动器
        /// </summary>
        protected Driver driver;

        /// <summary>
        /// Awake
        /// </summary>
        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 设定驱动器
        /// </summary>
        /// <param name="driver"></param>
        public void SetDriver(Driver driver)
        {
            this.driver = driver;
        }

        /// <summary>
        /// 每帧更新时
        /// </summary>
        public void Update()
        {
            if(driver != null)
            {
                driver.Update();
            }
        }

        /// <summary>
        /// 在每帧更新时之后
        /// </summary>
        public void LateUpdate()
        {
            if (driver != null)
            {
                driver.LateUpdate();
            }
        }

        /// <summary>
        /// 当释放时
        /// </summary>
        public void OnDestroy()
        {
            if (driver != null)
            {
                driver.OnDestroy();
            }
        }

    }

}