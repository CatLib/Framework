
namespace CatLib.API.Resources{


	public interface IBuildStrategy {

		BuildProcess Process{ get; }

		void Build(IBuildContext context);

	}


}