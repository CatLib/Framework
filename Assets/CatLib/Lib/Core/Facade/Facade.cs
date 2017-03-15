namespace CatLib
{

    /// <summary>
    /// 门面基类
    /// </summary>
    public class Facade<T>
    {

        public static T Instance
        {
            get
            {
                return App.Instance.Make<T>();
            }
        }
    }

}