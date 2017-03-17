using System;
using System.Collections.Generic;
using CatLib.API.DataTable;

namespace CatLib.DataTable{

	public class DataTable : IDataTable{
		
		/// <summary>
        /// 标题
        /// </summary>
		private Dictionary<string , int> title;

		/// <summary>
        /// 查询集合(不含头)
        /// </summary>
		private List<DataTableResult> dataTableResult;

		/// <summary>
        /// 选择器
        /// </summary>
		private DataTableSelector selector;

		public DataTable(string[][] result){

			GuardResult(result);
			ExtractionTitle(result);
			ExtractionContent(result);
			
		}

        /// <summary>
        /// 将数据表转为数组
        /// </summary>
        /// <returns></returns>
        public string[][] ToArray()
        {
            if (this.title == null || this.dataTableResult == null) { return null; }
            List<string[]> returnData = new List<string[]>();

            string[] title = new string[this.title.Keys.Count];
            this.title.Keys.CopyTo(title, 0);
            returnData.Add(title);

            for(int i = 0; i < dataTableResult.Count; i++)
            {
                returnData.Add(dataTableResult[i].Row);
            }

            return returnData.ToArray();
        }

		/// <summary>
        /// 获取标题对应的下标
        /// </summary>
		public int GetIndex(string field){
			
			if(title == null){ return -1; }
			if(title.ContainsKey(field)){

				return title[field];
			
			}
			return -1;

		}


		/// <summary>
        /// 提取标题
        /// </summary>
		private void ExtractionTitle(string[][] result){

			title = new Dictionary<string , int>();
			string[] titleRow = result[0];
			
			for(int i = 0; i < titleRow.Length ; i++){
				title.Add(titleRow[i] , i);
			}

		}

		/// <summary>
        /// 提取内容
        /// </summary>
		private void ExtractionContent(string[][] result){

			dataTableResult = new List<DataTableResult>();
			for(int i = 1; i < result.Length ; i++){
				if(result[i].Length != title.Count){ continue; }
				dataTableResult.Add( new DataTableResult(this ,  result[i]));
			}
		}

		/// <summary>
		/// 建立一个Where查询
		/// </summary>
		public IDataTableSelector Where(string field, string operators, string val, SelectorLinker linker = SelectorLinker.And){

			this.selector = new DataTableSelector(this);
			return this.selector.Where(field , operators , val , linker);

		}

		/// <summary>
		/// 建立一个Where嵌套查询
		/// </summary>
		public IDataTableSelector Where(Action<IDataTableSelector> nested, SelectorLinker linker = SelectorLinker.And){

			this.selector = new DataTableSelector(this);
			return this.selector.Where(nested , linker);

		}

		/// <summary>
		/// 执行一个查询获取结果集
		/// </summary>
		public IDataTableResult[] Get(){

			if(selector == null){
				
				return dataTableResult.ToArray();

			}

			var result = Parser();

			selector = null;
			
			return result;

		}

		/// <summary>
		/// 根据下标获取一个结果集
		/// </summary>
		public IDataTableResult Get(int index){

			if(dataTableResult.Count <= index){
				return null;
			}
			return dataTableResult[index];

		}

		/// <summary>
		/// 执行一个查询获取结果集
		/// </summary>
		private IDataTableResult[] Parser(){

			List<DataTableResult> result = new List<DataTableResult>();
			DataTableSelectorData[] wheres = selector.Wheres;
			for(int i = 0; i < dataTableResult.Count ; i++){

				if(Filter(dataTableResult[i] , wheres)){
					
					result.Add(dataTableResult[i]);

				}

			}			
			
    		return result.ToArray();

		}

		/// <summary>
		/// 检查结果集是否符合条件如果不符合应该返回false
		/// </summary>
		private bool Filter(DataTableResult row , DataTableSelectorData[] wheres){

			bool statu = true , statuNext;
			for(int i = 0; i < wheres.Length ; i++){

				statuNext = QueryWhere(row , wheres[i]);
				if(wheres[i].Linker == SelectorLinker.And){
					statu = statu && statuNext;
				}else if(wheres[i].Linker == SelectorLinker.Or){
					if(statu){ return statu; }
					statu = statu || statuNext;
				}
			}
			return statu;

		}

		private bool QueryWhere(DataTableResult row , DataTableSelectorData wheres){

			switch(wheres.SelectType){

				case "Nested":return WhereNested(row , wheres);
				case "Basic": return WhereBasic(row , wheres);
				case "NotNull": return WhereNotNull(row , wheres);
				case "Null": return WhereNull(row , wheres);
				default: return false;

			}

		}

		private bool WhereNested(DataTableResult row , DataTableSelectorData wheres){

			return Filter(row , wheres.Selector.Wheres);

		}

		private bool WhereBasic(DataTableResult row , DataTableSelectorData wheres){

			switch(wheres.Operator){

				case "==":
				case "=": return row[wheres.Field] == wheres.Value;
				case "<": return int.Parse(row[wheres.Field]) < int.Parse(wheres.Value);
				case ">": return int.Parse(row[wheres.Field]) > int.Parse(wheres.Value);
				case "<=": return int.Parse(row[wheres.Field]) <= int.Parse(wheres.Value);
				case ">=": return int.Parse(row[wheres.Field]) >= int.Parse(wheres.Value);
				case "<>":
				case "!=": return row[wheres.Field] != wheres.Value;
				default: return false;

			}


		}

		private bool WhereNull(DataTableResult row , DataTableSelectorData wheres){

			return string.IsNullOrEmpty(row[wheres.Field]);

		}

		private bool WhereNotNull(DataTableResult row , DataTableSelectorData wheres){

			return !WhereNull(row, wheres);

		}
		
		private void GuardResult(string[][] result){

			if(result == null){

				throw new ArgumentNullException("illegal result" , "result");
			
			}

			if(result.Length <= 0){

				throw new ArgumentException("illegal result" , "result");
			
			}

		}

	}

}
