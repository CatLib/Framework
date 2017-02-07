using XLua;

namespace CatLib.Contracts.Container
{

    public interface ITmpData
    {

        IBindData Given(string service);

        IBindData Given<T>();

    }

}