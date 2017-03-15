
namespace CatLib.API.Event
{
    /// <summary>事件机制接口</summary>
    public interface IEvent
    {

        /// <summary>事件系统</summary>
        IEventAchieve Event { get; }

    }
}
