
namespace CatLib.Contracts.Thread
{

    public interface IThread
    {
        void Task(System.Action task);

        void Task(System.Action task, System.Action onComplete);

        void Task(System.Action task, System.Action onComplete, System.Action<System.Exception> onError);

        void Task(System.Func<object> task);

        void Task(System.Func<object> task, System.Action<object> onComplete);

        void Task(System.Func<object> task, System.Action<object> onComplete, System.Action<System.Exception> onError);
    }

}