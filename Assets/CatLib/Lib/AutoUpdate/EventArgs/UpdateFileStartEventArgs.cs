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