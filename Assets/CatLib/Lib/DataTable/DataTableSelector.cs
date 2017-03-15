using System;
using System.Collections.Generic;
using CatLib.API.DataTable;

namespace CatLib.DataTable{

	public class DataTableSelector : IDataTableSelector {

		/// <summary>
		/// 数据表
		/// </summary>
		private DataTable dataTable;

		/// <summary>
		/// 合法的操作符
		/// </summary>
		protected readonly static string[] operators = {
			"==","=", "<", ">", "<=", ">=", "<>", "!=",
		};

		/// <summary>
		/// 查询数据
		/// </summary>
		private List<DataTableSelectorData> wheres;

		/// <summary>
		/// 获取查询序列
		/// </summary>
		public DataTableSelectorData[] Wheres{

			get{

				return wheres.ToArray();
				
			}

		}

		/// <summary>
		/// 查询器
		/// </summary>
		public DataTableSelector(DataTable dataTable){

			this.dataTable = dataTable;
			wheres = new List<DataTableSelectorData>();
			
		}

		/// <summary>
		/// 直接获取查询结果
		/// </summary>
		public IDataTableResult[] Get(){

			return dataTable.Get();

		}

		/// <summary>
		/// 建立一个Where查询
		/// </summary>
		public IDataTableSelector Where(string field, string operators, string val, SelectorLinker linker = SelectorLinker.And){

			GuardInvalidOperatorAndValue(operators , val);

			if(string.IsNullOrEmpty(val)){
				
				return WhereNull(field, linker, operators != "=");

			}

			wheres.Add(new DataTableSelectorData(){
							SelectType = "Basic",
							Field = field,
							Operator = operators,
							Value = val,
							Linker = linker
						});

			return this;

		}
		
		/// <summary>
		/// 建立一个Where嵌套查询
		/// </summary>
		public IDataTableSelector Where(Action<IDataTableSelector> nested, SelectorLinker linker = SelectorLinker.And){
			
			DataTableSelector selector = NewSelector();
			nested(selector);
			return NestedWhereSelector(selector , linker);

		}

		/// <summary>
		/// andWhere查询
		/// </summary>
		public IDataTableSelector AndWhere(string field, string operators, string val){

			return Where(field , operators ,val , SelectorLinker.And);

		}

		/// <summary>
		/// andWhere 嵌套查询
		/// </summary>
		public IDataTableSelector AndWhere(Action<IDataTableSelector> nested){

			return Where(nested , SelectorLinker.And);

		}

		/// <summary>
		/// andWhere查询
		/// </summary>
		public IDataTableSelector OrWhere(string field, string operators = null, string val = null){

			return Where(field , operators ,val , SelectorLinker.Or);

		}

		/// <summary>
		/// orWhere 嵌套查询
		/// </summary>
		public IDataTableSelector OrWhere(Action<IDataTableSelector> nested){

			return Where(nested, SelectorLinker.Or);

		}

		/// <summary>
		/// 为 null 的查询
		/// </summary>
		protected IDataTableSelector WhereNull(string field, SelectorLinker linker = SelectorLinker.And, bool negate = false)
		{
			wheres.Add(new DataTableSelectorData(){
							SelectType = negate ? "NotNull" : "Null",
							Field = field,
							Linker = linker
						});

			return this;
		}

		/// <summary>
		/// 增加一个嵌套查询
		/// </summary>
		protected IDataTableSelector NestedWhereSelector(DataTableSelector selector, SelectorLinker linker){

			wheres.Add(new DataTableSelectorData(){
							SelectType = "Nested",
							Selector = selector,
							Linker = linker
						});

			return this;

		}

		/// <summary>
		/// 获取一个新的查询器
		/// </summary>
		protected DataTableSelector NewSelector(){

			return new DataTableSelector(dataTable);

		}

		/// <summary>
		/// 检测无效的操作符和值
		/// </summary>
		protected void GuardInvalidOperatorAndValue(string operators, string val){

			var isOperator = operators.Contains(operators);
			
			if(!isOperator){

				throw new ArgumentException("illegal operator combination.");

			}

		}

	}

}