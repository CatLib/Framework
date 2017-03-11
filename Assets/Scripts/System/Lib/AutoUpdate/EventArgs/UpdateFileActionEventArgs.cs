using System;
using UnityEngine.Networking;

namespace CatLib.AutoUpdate
{

    public class UpdateFileActionEventArgs : EventArgs
    {

        public UnityWebRequest Request { get; protected set; }

        public string FilePath { get; protected set; }

        public UpdateFileActionEventArgs(string filePath , UnityWebRequest request)
        {
            Request = request;
            FilePath = filePath;
        }

    }

}