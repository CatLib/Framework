using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatLib.Network
{

	public class CTcpRequestEvents{

		public static readonly string ON_CONNECT = "network.tcp.connector.connect.";

		public static readonly string ON_CLOSE   = "network.tcp.connector.close.";

		public static readonly string ON_ERROR   = "network.tcp.connector.error.";

		public static readonly string ON_MESSAGE = "network.tcp.connector.message.";

	}

	
}