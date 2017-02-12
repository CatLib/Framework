
using System.Runtime.Serialization;

namespace CatLib.Base
{

	public class CException : System.Exception , IErrorCode 
	{
		public CException() { }
		public CException( string message ) : base( message ) { }
		public CException( string message, System.Exception inner ) : base( message, inner ) { }
		protected CException( SerializationInfo info, StreamingContext context ) : base( info, context ) { }
		public CErrorCode ErrorCode{ get{ return CErrorCode.SYSTEM_EXCEPTION; } }

	}

}