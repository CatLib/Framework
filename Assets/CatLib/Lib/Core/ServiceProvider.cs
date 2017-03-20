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
 
using System;
using System.Collections;

namespace CatLib
{
    /// <summary>
    /// 服务提供者
    /// </summary>
    public abstract class ServiceProvider : Component
    {

        public virtual void Init() { }

        public virtual ProviderProcess ProviderProcess { get { return ProviderProcess.Normal; } }

        public virtual IEnumerator OnProviderProcess() { yield break; }

        public virtual Type[] ProviderDepend { get { return new Type[] { }; } }

        abstract public void Register();

    }

}