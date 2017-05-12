/*
 * This file is part of the CatLib package.
 *
 * (c) Ming ming <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System;
using CatLib.API;
using CatLib.Event;
using CatLib.MsgPack;
using CatLib.Resources;
using CatLib.IO;
using CatLib.Time;
using CatLib.LocalSetting;
using CatLib;

namespace CatLib.Demo.MsgPack
{
    /**
     * 这个类提供了当前demo演示时用到的组件 
     */
    public class Bootstraps : IBootstrap
    {

        public void Bootstrap()
        {
            App.Instance.Register(typeof(CoreProvider));
            App.Instance.Register(typeof(IOProvider));
            App.Instance.Register(typeof(ResourcesProvider));
            App.Instance.Register(typeof(TimeProvider));
            App.Instance.Register(typeof(LocalSettingProvider));
            App.Instance.Register(typeof(MsgPackProvider));
            App.Instance.Register(typeof(EventProvider));
            App.Instance.Register(typeof(MsgPackDemo));
        }

    }

    public class Program : UnityEngine.MonoBehaviour {

    	// Use this for initialization
        public void Awake() {
            (new Application(this)).Bootstrap(new Type[] { typeof(Bootstraps)}).Init();
    	}

    }
}