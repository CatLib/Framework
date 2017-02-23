
namespace CatLib.API.UpdateSystem{

	public interface IUpdateFileField
	{

		 string MD5{ get; }

		 string Path{ get; }

		 long Size{ get; }
		
	}


}