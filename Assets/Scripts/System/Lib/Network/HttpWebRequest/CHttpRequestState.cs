using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

namespace CatLib.Network
{

    public class CHttpRequestState
    {

        public const int BUFFER_SIZE = 1024;
        public byte[] BufferRead;

        public System.Net.HttpWebRequest Request;
        public HttpWebResponse Response;
        public Stream StreamResponse;

        public CHttpRequestState()
        {
            BufferRead     = new byte[BUFFER_SIZE];
            Request        = null;
            StreamResponse = null;
        }
    }

}