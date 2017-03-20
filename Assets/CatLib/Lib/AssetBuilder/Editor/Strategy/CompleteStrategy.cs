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
 
using CatLib.API.Resources;
using CatLib.API.AssetBuilder;
using UnityEditor;

namespace CatLib.AssetBuilder
{

	public class CompleteStrategy : IBuildStrategy {

		public BuildProcess Process{ get { return BuildProcess.Complete; } }

		public void Build(IBuildContext context){

			AssetDatabase.Refresh();

		}



	}

}