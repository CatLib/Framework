/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
 
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