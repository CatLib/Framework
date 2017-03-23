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

using System.Collections.Generic;

namespace CatLib.LruCache{

	/// <summary>
    /// 缓存节点
    /// </summary>
	public class CacheNode<TKey, TVal>{

		/// <summary>
		/// 键值
		/// </summary>
		public KeyValuePair<TKey , TVal> KeyValue{ get; private set; }
		
		/// <summary>
		/// 上一个节点
		/// </summary>
        public CacheNode<TKey, TVal> Previous { get; set; }
        
		/// <summary>
		/// 下一个节点
		/// </summary>
		public CacheNode<TKey, TVal> Next { get; set; }

		/// <summary>
		/// 创建一个缓存节点
		/// </summary>
        public CacheNode(TKey key , TVal val)
        {	
			KeyValue = new KeyValuePair<TKey , TVal>(key , val);
        }

	}

}