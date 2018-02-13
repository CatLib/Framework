using System;
using System.Diagnostics;
using System.IO;

namespace CatLib._3rd.SharpCompress.Compressors.LZMA.Utilites
{
    internal class CrcCheckStream : Stream
    {
        private readonly uint mExpectedCRC;
        private uint mCurrentCRC;
        private bool mClosed;

        private readonly long[] mBytes = new long[256];
        private long mLength;

        public CrcCheckStream(uint crc)
        {
            mExpectedCRC = crc;
            mCurrentCRC = CRC.kInitCRC;
        }

        protected override void Dispose(bool disposing)
        {
            if (mCurrentCRC != mExpectedCRC)
            {
                throw new InvalidOperationException();
            }
            try
            {
                if (disposing && !mClosed)
                {
                    mClosed = true;
                    mCurrentCRC = CRC.Finish(mCurrentCRC);
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            mLength += count;
            for (int i = 0; i < count; i++)
            {
                mBytes[buffer[offset + i]]++;
            }

            mCurrentCRC = CRC.Update(mCurrentCRC, buffer, offset, count);
        }
    }
}