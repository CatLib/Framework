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
                    if (!UnityEngine.Application.isPlaying)
                    {
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