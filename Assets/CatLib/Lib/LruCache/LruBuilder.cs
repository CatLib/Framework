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
 
using CatLib.API.LruCache;

namespace CatLib.LruCache{

	public class LruBuilder : ILruBuilder{

		
		/// <summary>
		/// 创建 Lru 缓存
		/// </summary>
		public ILruCache<TKey, TVal> Create<TKey,TVal>(int maxCapacity){

			return new LruCache<TKey , TVal>(maxCapacity);

		} 
		
		
	}

}