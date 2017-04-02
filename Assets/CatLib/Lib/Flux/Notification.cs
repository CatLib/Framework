namespace CatLib.Flux
{

    /// <summary>
    /// 通知
    /// </summary>
    public class Notification : INotification
    {

        /// <summary>
        /// 通知名
        /// </summary>
        private string name;

        /// <summary>
        /// 附带物
        /// </summary>
        private object payload;

        /// <summary>
        /// 通知发送者
        /// </summary>
        private object sender;

        /// <summary>
        /// 通知名
        /// </summary>
        public virtual string Name
        {
            get { return name; }
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
        /// 通知发送者
        /// </summary>
        public virtual object Sender
        {
            get
            {
                return sender;
            }
            set
            {
                sender = value;
            }
        }

        /// <summary>
        /// 创建一个通知
        /// </summary>
        /// <param name="name"></param>
        public Notification(string name)
            : this(name, null, null)
        { }


        /// <summary>
        /// 创建一个通知
        /// </summary>
        /// <param name="name">通知名</param>
        /// <param name="sender">发送者</param>
        public Notification(string name, object sender)
            : this(name, sender, null)
        { }

        /// <summary>
        /// 创建一个通知
        /// </summary>
        /// <param name="name">通知名</param>
        /// <param name="sender">发送者</param>
        /// <param name="payload">附带物体</param>
        public Notification(string name , object sender, object payload)
        {
            this.name = name;
            this.sender = sender;
            this.payload = payload;
        }
    }

}