
namespace CatLib.API.Container
{

    public interface IGivenData
    {

        IBindData Given(string service);

        IBindData Given<T>();

    }

}