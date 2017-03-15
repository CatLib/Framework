using System;

namespace CatLib.AutoUpdate
{

    public class UpdateFileStartEventArgs : EventArgs
    {

        public string[] UpdateList { get; protected set; }

        public UpdateFileStartEventArgs(string[] list)
        {
            UpdateList = list;
        }

    }

}