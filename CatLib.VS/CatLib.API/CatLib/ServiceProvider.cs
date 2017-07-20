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

#if CATLIB
namespace CatLib.API
{
    /// <summary>
    /// 服务提供者
    /// </summary>
    public abstract class ServiceProvider : IServiceProvider
    {
        /// <summary>
        /// CatLib实例
        /// </summary>
        public IApplication App
        {
            get
            {
                return API.App.Instance;
            }
        }

        /// <summary>
        /// 服务提供者初始化
        /// </summary>
        /// <returns>迭代器</returns>
        public virtual void Init()
        {
        }

        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public abstract void Register();

        /// <summary>
        /// 相等性比较
        /// </summary>
        /// <param name="obj">比较对象</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return GetType() == obj.GetType();
        }

        /// <summary>
        /// 获取HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }
    }
}
#endif