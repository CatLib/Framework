
using CatLib.API.Flux;

namespace CatLib.Flux
{

    /// <summary>
    /// 通知
    /// </summary>
    public class Notification : INotification
    {

        /// <summary>
        /// 通知行为
        /// </summary>
        private string action;

        /// <summary>
        /// 附带物
        /// </summary>
        private object payload;

        /// <summary>
        /// 通知行为
        /// </summary>
        public virtual string Action
        {
            get { return action; }
        }

        /// <summary>
        /// 附带物
        /// </summary>
        public virtual object Payload
        {
            get
            {
                return payload;
            }
            set
            {
                payload = value;
            }
        }

        /// <summary>
        /// 创建一个通知
        /// </summary>
        /// <param name="action">通知行为</param>
        public Notification(string action)
            : this(action, null)
        { }

        /// <summary>
        /// 创建一个通知
        /// </summary>
        /// <param name="action">通知行为</param>
        /// <param name="payload">附带物体</param>
        public Notification(string action, object payload)
        {
            this.action = action;
            this.payload = payload;
        }
    }

}