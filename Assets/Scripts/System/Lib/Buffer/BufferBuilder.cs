using CatLib.Contracts.Buffer;
using System;

namespace CatLib.Buffer{

	public class BufferBuilder : IBufferBuilder
    {

		private byte[] buffer = new byte[0];

		public static implicit operator byte[](BufferBuilder buffer){

			return buffer.buffer;

		}

		public static implicit operator BufferBuilder(byte[] buffer){

			var obj = new BufferBuilder();
			obj.Push(buffer);
			return obj;

		}

        public byte[] Byte
        {
            get
            {
                return buffer;
            }
            set
            {
                if(value == null)
                {
                    buffer = new byte[0];
                    return;
                }
                buffer = value;
            }
        }

		public int IndexOf(params byte[] data){

			if(data.Length <= 0){ return -1; }

			bool isFinded;
			for(int i = 0, n; i < buffer.Length; i++){

                if(buffer[i] == data[0])
                {
                    isFinded = true;
                    for (n = 0; n < data.Length; n++)
                    {
                        if(buffer[i + n] != data[n])
                        {
                            isFinded = false;
                            break;
                        }
                    }
                    if (isFinded) { return i; }
                }
                				
			}

			return -1;

		}

		public void Push(byte[] data){

			var newBuffer = new byte[buffer.Length + data.Length];
			System.Buffer.BlockCopy(buffer, 0, newBuffer, 0, buffer.Length);
            System.Buffer.BlockCopy(data, 0, newBuffer, buffer.Length, data.Length);
			buffer = newBuffer;

		}

		public byte[] Pop(int count = 1){

			count = Math.Max(1, count);
			if(count > buffer.Length){ throw new ArgumentOutOfRangeException("count"); }

			byte[] returnBuffer = new byte[count];
            System.Buffer.BlockCopy(buffer, buffer.Length - returnBuffer.Length ,returnBuffer, 0, returnBuffer.Length);

			byte[] newBuffer = new byte[buffer.Length - returnBuffer.Length];
            System.Buffer.BlockCopy(buffer, 0 ,newBuffer, 0, newBuffer.Length);

			return returnBuffer;

		}

		public void Unshift(byte[] data){

			var newBuffer = new byte[buffer.Length + data.Length];
            System.Buffer.BlockCopy(data, 0, newBuffer, 0, data.Length);
            System.Buffer.BlockCopy(buffer, 0, newBuffer, data.Length, buffer.Length);
			buffer = newBuffer;

		}

		public byte[] Shift(int count = 1){

            count = Math.Max(1, count);
            byte[] returnBuffer = Peek(count);
			byte[] newBuffer = new byte[buffer.Length - count];
            System.Buffer.BlockCopy(buffer, count ,newBuffer, 0, newBuffer.Length);
			buffer = newBuffer;
			return returnBuffer;

		}

		public byte[] Peek(int count = 1){

			count = Math.Max(1, count);
			if(count > buffer.Length){ throw new ArgumentOutOfRangeException("count"); }

			byte[] newBuffer = new byte[count];
            System.Buffer.BlockCopy(buffer, 0 ,newBuffer, 0, newBuffer.Length);
			return newBuffer;

		}

        public void Clear()
        {
            buffer = new byte[0];
        }

		public int Length{

			get{
				
				return buffer.Length;
			
			}

		}
		


	}

}