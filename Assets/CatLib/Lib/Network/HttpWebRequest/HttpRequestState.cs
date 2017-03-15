using System.IO;
using System.Net;

namespace CatLib.Network
{

    public class HttpRequestState
    {

        public const int BUFFER_SIZE = 1024;
        public byte[] BufferRead;

        public System.Net.HttpWebRequest Request;
        public HttpWebResponse Response;
        public Stream StreamResponse;

        public HttpRequestState()
        {
            BufferRead     = new byte[BUFFER_SIZE];
            Request        = null;
            StreamResponse = null;
        }
    }

}