
namespace CatLib.Contracts.CSV{

	
	/// <summary>
	/// CSV管理器接口
	/// </summary>
	public interface ICSV {


		/// <summary>
        /// 重新载入CSV库
        /// </summary>
		void Reload();

		/// <summary>
        /// 获取一个CSV表
        /// </summary>
		ICSVTable Get(string table);

	}

}
