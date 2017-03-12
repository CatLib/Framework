
namespace CatLib.API.Csv{

	
	/// <summary>
	/// CSV管理器接口
	/// </summary>
	public interface ICsv {


		/// <summary>
        /// 重新载入CSV库
        /// </summary>
		void Reload();

		/// <summary>
        /// 获取一个CSV表
        /// </summary>
		ICsvTable Get(string table);

	}

}
