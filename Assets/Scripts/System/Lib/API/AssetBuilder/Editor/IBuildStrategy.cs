
namespace CatLib.API.AssetBuilder
{


	public interface IBuildStrategy {

		BuildProcess Process{ get; }

		void Build(IBuildContext context);

	}


}