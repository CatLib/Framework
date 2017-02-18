using System;

namespace CatLib
{

    public class ErrorEventArgs : EventArgs
    {

        public Exception Error { get; protected set; }

        public ErrorEventArgs(Exception ex)
        {
            Error = ex;
        }

    }

}