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

using System.Collections;
using System.Collections.Generic;

namespace CatLib.API.LruCache{

	public interface ILruCache<TKey, TVal> :  IEnumerator , IEnumerator<KeyValuePair<TKey , TVal>> ,IEnumerable , IEnumerable<KeyValuePair<TKey , TVal>>{

		/// <summary>
		/// 在lru缓存中增加一个元素
		/// </summary>
		void Add(TKey key, TVal value);

		/// <summary>
		/// 根据key获取val，如果被淘汰则返回传入的默认值
		/// </summary>
		TVal Get(TKey key , TVal defaultValue = default(TVal));

		/// <summary>
		/// 获取Lru缓存中的元素数量
		/// </summary>
		int Count{ get; }

		/// <summary>
		/// 根据key获取val，如果被淘汰则返回默认值
		/// </summary>
		TVal this[TKey key]{ get; }

	}

}