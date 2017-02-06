using XLua;

namespace CatLib.Contracts.Container
{
    [LuaCallCSharp]

    public interface ITmpData
    {

        IBindData Given(string service);

        IBindData Given<T>();

    }

}