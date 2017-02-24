using CatLib.Secret;
using CatLib.API.IO;
using CatLib.API.ResourcesSystem;


namespace CatLib.UpdateSystem{

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
                file = IO.IO.MakeFile(context.ReleasePath + IO.IO.PATH_SPLITTER + context.ReleaseFiles[i]);
                lst.Append(context.ReleaseFiles[i], MD5.ParseFile(file.FileInfo), file.FileInfo.Length);
            }

            var store = new UpdateFileStore();
			store.Save(context.ReleasePath, lst);
			
		}
	}

}