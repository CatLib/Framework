
namespace CatLib.Contracts.Time
{

    /// <summary>
    /// 时间运行器状态
    /// </summary>
    public interface ITimeRunner : IUpdate
    {

        bool IsComplete { get; } 
        
    }

}