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
 
namespace CatLib.API
{
	
	///<summary>延迟刷新</summary>
	public interface ILateUpdate{

		void LateUpdate();
		
	}

}