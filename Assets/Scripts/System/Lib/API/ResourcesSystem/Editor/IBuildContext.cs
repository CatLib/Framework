
using UnityEditor;

namespace CatLib.API.ResourcesSystem{

	public interface IBuildContext{

		/// <summary>
        /// 当前编译目标平台
        /// </summary>
		BuildTarget BuildTarget{ get; set; }

		/// <summary>
        /// 目标平台的名字
        /// </summary>
		string PlatformName{ get; set; }

		/// <summary>
        /// 需要编译的文件路径
        /// </summary>
		string BuildPath{ get; set; }

		/// <summary>
        /// 不需要编译的文件路径
        /// </summary>
		string NoBuildPath{ get; set; }

		/// <summary>
        /// 最终发布的路径
        /// </summary>
		string ReleasePath{ get; set; }

		/// <summary>
        /// 被加密的文件列表
        /// </summary>
		string[] EncryptionFiles{ get; set; }

		
	}

}