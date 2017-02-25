
using CatLib.API.Resources;
using UnityEditor;
using UnityEngine;
using System;

namespace CatLib.Resources{

	public class SetupStrategy : IBuildStrategy {

		public BuildProcess Process{ get { return BuildProcess.Setup; } }

		public void Build(IBuildContext context){

            if (UnityEngine.Application.isPlaying)
            {
                throw new Exception("please stop playing");
            }

			RuntimePlatform switchPlatform = Env.SwitchPlatform;
			context.BuildTarget  = PlatformToBuildTarget(switchPlatform);
			context.PlatformName = Env.PlatformToName(switchPlatform);
			context.ReleasePath  = Env.DataPath + Env.ReleasePath + IO.IO.PATH_SPLITTER + context.PlatformName;
			context.BuildPath    = Env.DataPath + Env.ResourcesBuildPath;
			context.NoBuildPath  = Env.DataPath + Env.ResourcesNoBuildPath;

		}

		/// <summary>
		/// 将平台转为编译目标
		/// </summary>
		protected BuildTarget PlatformToBuildTarget(RuntimePlatform platform){

			switch(platform){
				
				case RuntimePlatform.LinuxPlayer: return BuildTarget.StandaloneLinux64;
				case RuntimePlatform.WindowsPlayer: 
				case RuntimePlatform.WindowsEditor: return BuildTarget.StandaloneWindows64;
				case RuntimePlatform.Android: return BuildTarget.Android;
				case RuntimePlatform.IPhonePlayer: return BuildTarget.iOS;
				case RuntimePlatform.OSXEditor:
				case RuntimePlatform.OSXPlayer: return BuildTarget.StandaloneOSXIntel64;
				default: throw new ArgumentException("Undefined Platform");

			}

		}
	}

}