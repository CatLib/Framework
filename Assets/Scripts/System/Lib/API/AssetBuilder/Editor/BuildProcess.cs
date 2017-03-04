
namespace CatLib.API.AssetBuilder
{

	/// <summary>构建流程</summary>
	public enum BuildProcess{

		/// <summary>准备一系列配置信息</summary>
		Setup = 1,

		/// <summary>清理旧的数据</summary>
		Clear = 10,

		/// <summary>编译文件</summary>
		Build = 20,

        /// <summary>编译出的文件扫描流程</summary>
        Scanning = 30,

        /// <summary>对扫描到的有效文件进行过滤</summary>
        Filter = 40,

		/// <summary>对文件进行加密</summary>
		Encryption = 50,

        /// <summary>生成文件结构</summary>
        GenTable = 60,

		/// <summary>更新目录生成</summary>
		GenPath = 70,

		/// <summary>完成</summary>
		Complete = 80,
		
	}

}