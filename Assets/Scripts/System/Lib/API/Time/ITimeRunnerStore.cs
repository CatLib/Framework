
namespace CatLib.API.Time
{
    public interface ITimeRunnerStore
    {

        ITimeQueue CreateQueue();

        /// <summary>
        /// 推入一个时间运行器
        /// </summary>
        /// <param name="runner"></param>
        bool Runner(ITimeRunner runner);

        /// <summary>
        /// 停止一个时间运行器
        /// </summary>
        /// <param name="runner"></param>
        bool StopRunner(ITimeRunner runner);
    }

}