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
 
namespace CatLib.LruCache{

	/// <summary>
    /// 缓存节点
    /// </summary>
	public class CacheNode<TKey, TVal>{

		/// <summary>
		/// 键
		/// </summary>
        public TKey Key { get; private set; }

		/// <summary>
		/// 值
		/// </summary>
		public TVal Val { get; private set; }
		
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
			Key = key;
            Val = val;
        }

	}

}