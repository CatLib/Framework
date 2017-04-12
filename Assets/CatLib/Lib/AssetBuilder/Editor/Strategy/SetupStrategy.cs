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

using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using CatLib.API.AssetBuilder;
using CatLib.API;
using CatLib.API.IO;

namespace CatLib.AssetBuilder
{
    /// <summary>
    /// 文件设定策略
    /// </summary>
    public sealed class SetupStrategy : IBuildStrategy
    {
        /// <summary>
        /// 环境配置
        /// </summary>
        [Dependency]
        public IEnv Env { get; set; }

        /// <summary>
        /// 配置的编译流程
        /// </summary>
        public BuildProcess Process
        {
            get { return BuildProcess.Setup; }
        }

        /// <summary>
        /// 执行编译时
        /// </summary>
        /// <param name="context">编译上下文</param>
        public void Build(IBuildContext context)
        {
            if (UnityEngine.Application.isPlaying)
            {
                throw new Exception("please stop playing");
            }

            var switchPlatform = Env.SwitchPlatform;
            context.BuildTarget = PlatformToBuildTarget(switchPlatform);
            context.PlatformName = Env.PlatformToName(switchPlatform);
            context.ReleasePath = Env.DataPath + Env.ReleasePath + Path.AltDirectorySeparatorChar + context.PlatformName;
            context.BuildPath = Env.DataPath + Env.ResourcesBuildPath;
            context.NoBuildPath = Env.DataPath + Env.ResourcesNoBuildPath;

            var ioFactory = (App.Instance.Make(typeof(IIOFactory)) as IIOFactory);
            if (ioFactory == null)
            {
                throw new Exception("can not find disk , please check io service provider");
            }
            context.Disk = ioFactory.Disk();
        }

        /// <summary>
        /// 将平台转为编译目标
        /// </summary>
        /// <param name="platform">运行时的平台</param>
        /// <returns></returns>
        private BuildTarget PlatformToBuildTarget(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.LinuxPlayer:
                    return BuildTarget.StandaloneLinux64;
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    return BuildTarget.StandaloneWindows64;
                case RuntimePlatform.Android:
                    return BuildTarget.Android;
                case RuntimePlatform.IPhonePlayer:
                    return BuildTarget.iOS;
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    return BuildTarget.StandaloneOSXIntel64;
                default:
                    throw new ArgumentException("Undefined Platform");
            }
        }
    }
}