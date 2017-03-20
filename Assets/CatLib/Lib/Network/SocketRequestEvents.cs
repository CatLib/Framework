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
 
namespace CatLib.Network
{

	public class SocketRequestEvents{

		public static readonly string ON_CONNECT = "network.socket.connector.connect.";

		public static readonly string ON_CLOSE   = "network.socket.connector.close.";

		public static readonly string ON_ERROR   = "network.socket.connector.error.";

		public static readonly string ON_MESSAGE = "network.socket.connector.message.";

	}

	
}