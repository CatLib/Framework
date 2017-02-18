using XLua;

namespace CatLib.Contracts.Container
{

    public interface IGivenData
    {

        IBindData Given(string service);

        IBindData Given<T>();

    }

}