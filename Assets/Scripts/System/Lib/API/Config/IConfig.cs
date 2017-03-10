
namespace CatLib.API.Config{
	public interface IConfig{

		object Service{ get; }
		
		object[] Config { get; }

	}

}