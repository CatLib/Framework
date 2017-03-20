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