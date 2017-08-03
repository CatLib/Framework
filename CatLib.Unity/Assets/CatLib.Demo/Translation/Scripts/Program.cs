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

using CatLib.Events;
using CatLib.Translation;
using System;

namespace CatLib.Demo.Translation
{
    /// <summary>
    /// 引导程序
    /// </summary>
    public class Bootstraps : IBootstrap
    {
        /// <summary>
        /// 引导程序
        /// </summary>
        public void Bootstrap()
        {
            App.Register(new EventsProvider());
            App.Register(new TranslationProvider());
            App.Register(new TranslationDemo());
        }
    }

    /// <summary>
    /// 程序入口
    /// </summary>
    public class Program : UnityEngine.MonoBehaviour
    {
        /// <summary>
        /// Unity Awake
        /// </summary>
        public void Awake()
        {
            var application = new Application();
            application.OnFindType((type) =>
            {
                return Type.GetType(type);
            });
            application.Bootstrap(new Bootstraps()).Init();
        }
    }
}