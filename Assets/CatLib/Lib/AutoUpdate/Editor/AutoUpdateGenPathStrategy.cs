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

using System.IO;
using CatLib.API.AssetBuilder;
using CatLib.API.IO;
using CatLib.Hash;

namespace CatLib.AutoUpdate{

	public class AutoUpdateGenPathStrategy : IBuildStrategy {

		public BuildProcess Process{ get { return BuildProcess.GenPath; } }

		public void Build(IBuildContext context){

			BuildListFile(context);

		}

		/// <summary>
		/// 编译列表文件
		/// </summary>
		/// <param name="path">路径</param>
		protected void BuildListFile(IBuildContext context){

			UpdateFile lst = new UpdateFile();

            IFile file;
            for(int i = 0; i < context.ReleaseFiles.Length; i++)
            {
                file = context.Disk.File(context.ReleasePath + Path.AltDirectorySeparatorChar + context.ReleaseFiles[i] , PathTypes.Absolute);
                lst.Append(context.ReleaseFiles[i], MD5.ParseFile(file.FullName), file.Length);
            }

            var store = App.Instance.Make(typeof(UpdateFileStore).ToString()) as UpdateFileStore;
			store.Save(context.ReleasePath, lst);
			
		}
	}

}