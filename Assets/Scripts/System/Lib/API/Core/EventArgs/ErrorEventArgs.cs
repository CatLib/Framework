namespace CatLib.API
{

    public class ErrorEventArgs : System.EventArgs
    {

        public System.Exception Error { get; protected set; }

        public ErrorEventArgs(System.Exception ex)
        {
            Error = ex;
        }

    }

}