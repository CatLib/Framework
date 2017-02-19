
namespace CatLib.Contracts.Time
{
    /// <summary>
    /// 时间
    /// </summary>
    public interface ITime
    {

        float Time { get; }

        float DeltaTime { get; }

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