
namespace CatLib.API.ResourcesSystem{


	public interface IBuildStrategy {

		BuildProcess Process{ get; }

		void Build(IBuildContext context);

	}


}