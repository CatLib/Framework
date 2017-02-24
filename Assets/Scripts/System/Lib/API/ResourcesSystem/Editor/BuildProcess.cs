
namespace CatLib.API.ResourcesSystem{

	/// <summary>构建流程</summary>
	public enum BuildProcess{

		/// <summary>准备一系列配置信息</summary>
		Setup = 1,

		/// <summary>清理旧的数据</summary>
		Clear = 2,

		/// <summary>编译文件</summary>
		Build = 3,

		/// <summary>对文件进行加密</summary>
		Encryption = 4,

		/// <summary>目录生成</summary>
		GenPath = 5,

		/// <summary>完成</summary>
		Complete = 6,
		
	}

}