
namespace CatLib.API.Thread
{

    public interface IThread
    {

        ITask Task(System.Action task);

        ITask Task(System.Func<object> task);

    }

}