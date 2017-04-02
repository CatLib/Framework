
namespace CatLib.Flux
{
    /// <summary>
    /// 存储
    /// </summary>
    public class Store<TPayload>
    {

        /// <summary>
        /// 调度器
        /// </summary>
        protected Dispatcher<TPayload> dispatcher;

        public Store(Dispatcher<TPayload> dispatcher)
        {
            this.dispatcher = dispatcher;
        }

    }
}
