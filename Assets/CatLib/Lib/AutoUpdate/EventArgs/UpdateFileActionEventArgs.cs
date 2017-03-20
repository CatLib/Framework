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