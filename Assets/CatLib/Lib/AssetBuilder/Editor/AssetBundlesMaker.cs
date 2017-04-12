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

using UnityEditor;
using System.Collections.Generic;
using CatLib.API.AssetBuilder;

namespace CatLib.AssetBuilder
{
    /// <summary>
    /// AssetBundle构建器
    /// </summary>
    public class AssetBundlesMaker
    {
        /// <summary>
        /// 编译Asset Bundle
        /// </summary>
        [MenuItem("CatLib/Asset Builder/Build Current Platform")]
        public static void BuildAllAssetBundles()
        {
            var strategys = new List<IBuildStrategy>();

            foreach (var t in typeof(IBuildStrategy).GetChildTypesWithInterface())
            {
                strategys.Add(App.Instance.Make(t.ToString()) as IBuildStrategy);
            }

            strategys.Sort((left, right) => ((int)left.Process).CompareTo((int)right.Process));

            var context = new BuildContext();
            foreach (var buildStrategy in strategys.ToArray())
            {
                buildStrategy.Build(context);
            }
        }
    }
}