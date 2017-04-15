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

namespace CatLib.API.Routing
{

    /// <summary>
    /// can not find scheme
    /// </summary>
    public class NotFoundSchemeException : CatLibException
    {
        public NotFoundSchemeException(string message) : base(message) { }
    }

}