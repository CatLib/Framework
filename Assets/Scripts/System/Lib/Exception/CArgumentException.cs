using System;
using System.Runtime.Serialization;

namespace CatLib.Exception{

	public class CArgumentException : ArgumentException , IErrorCode {

		public CArgumentException() { }
		public CArgumentException( string message ) : base( message ) { }
		public CArgumentException( string message, System.Exception inner ) : base( message, inner ) { }
		protected CArgumentException( SerializationInfo info, StreamingContext context ) : base( info, context ) { }
		public CErrorCode ErrorCode{ get{ return CErrorCode.ARGUMENT_EXCEPTION; } }
	}

}