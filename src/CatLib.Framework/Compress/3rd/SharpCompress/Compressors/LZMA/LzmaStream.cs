using System;
using System.IO;
using CatLib._3rd.SharpCompress.Compressors.LZMA.LZ;
using CatLib._3rd.SharpCompress.Converters;

namespace CatLib._3rd.SharpCompress.Compressors.LZMA
{
    /// <summary>
    /// 
    /// </summary>
    public class LzmaStream : Stream
    {
        private readonly Stream inputStream;
        private readonly long inputSize;
        private readonly long outputSize;

        private readonly int dictionarySize;
        private readonly OutWindow outWindow = new OutWindow();
        private readonly RangeCoder.Decoder rangeDecoder = new RangeCoder.Decoder();
        private Decoder decoder;

        private long position;
        private bool endReached;
        private long availableBytes;
        private long rangeDecoderLimit;
        private long inputPosition;

        // LZMA2
        private readonly bool isLZMA2;
        private bool uncompressedChunk;
        private bool needDictReset = true;
        private bool needProps = true;

        private readonly Encoder encoder;
        private bool isDisposed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="inputStream"></param>
        public LzmaStream(byte[] properties, Stream inputStream)
            : this(properties, inputStream, -1, -1, null, properties.Length < 5)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="inputStream"></param>
        /// <param name="inputSize"></param>
        public LzmaStream(byte[] properties, Stream inputStream, long inputSize)
            : this(properties, inputStream, inputSize, -1, null, properties.Length < 5)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="inputStream"></param>
        /// <param name="inputSize"></param>
        /// <param name="outputSize"></param>
        public LzmaStream(byte[] properties, Stream inputStream, long inputSize, long outputSize)
            : this(properties, inputStream, inputSize, outputSize, null, properties.Length < 5)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="inputStream"></param>
        /// <param name="inputSize"></param>
        /// <param name="outputSize"></param>
        /// <param name="presetDictionary"></param>
        /// <param name="isLZMA2"></param>
        public LzmaStream(byte[] properties, Stream inputStream, long inputSize, long outputSize,
                          Stream presetDictionary, bool isLZMA2)
        {
            Properties = new byte[5];
            this.inputStream = inputStream;
            this.inputSize = inputSize;
            this.outputSize = outputSize;
            this.isLZMA2 = isLZMA2;

            if (!isLZMA2)
            {
                dictionarySize = DataConverter.LittleEndian.GetInt32(properties, 1);
                outWindow.Create(dictionarySize);
                if (presetDictionary != null)
                {
                    outWindow.Train(presetDictionary);
                }

                rangeDecoder.Init(inputStream);

                decoder = new Decoder();
                decoder.SetDecoderProperties(properties);
                Properties = properties;

                availableBytes = outputSize < 0 ? long.MaxValue : outputSize;
                rangeDecoderLimit = inputSize;
            }
            else
            {
                dictionarySize = 2 | (properties[0] & 1);
                dictionarySize <<= (properties[0] >> 1) + 11;

                outWindow.Create(dictionarySize);
                if (presetDictionary != null)
                {
                    outWindow.Train(presetDictionary);
                    needDictReset = false;
                }

                Properties = new byte[1];
                availableBytes = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="isLZMA2"></param>
        /// <param name="outputStream"></param>
        public LzmaStream(LzmaEncoderProperties properties, bool isLZMA2, Stream outputStream)
            : this(properties, isLZMA2, null, outputStream)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="isLZMA2"></param>
        /// <param name="presetDictionary"></param>
        /// <param name="outputStream"></param>
        public LzmaStream(LzmaEncoderProperties properties, bool isLZMA2, Stream presetDictionary, Stream outputStream)
        {
            Properties = new byte[5];
            this.isLZMA2 = isLZMA2;
            availableBytes = 0;
            endReached = true;

            if (isLZMA2)
            {
                throw new NotImplementedException();
            }

            encoder = new Encoder();
            encoder.SetCoderProperties(properties.propIDs, properties.properties);
            MemoryStream propStream = new MemoryStream(5);
            encoder.WriteCoderProperties(propStream);
            Properties = propStream.ToArray();

            encoder.SetStreams(null, outputStream, -1, -1);
            if (presetDictionary != null)
            {
                encoder.Train(presetDictionary);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool CanRead
        {
            get { return encoder == null; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool CanSeek
        {
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool CanWrite
        {
            get { return encoder != null; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Flush()
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }
            isDisposed = true;
            if (disposing)
            {
                if (encoder != null)
                {
                    position = encoder.Code(null, true);
                }
                if (inputStream != null)
                {
                    inputStream.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 
        /// </summary>
        public override long Length
        {
            get { return position + availableBytes; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override long Position
        {
            get { return position; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (endReached)
            {
                return 0;
            }

            int total = 0;
            while (total < count)
            {
                if (availableBytes == 0)
                {
                    if (isLZMA2)
                    {
                        decodeChunkHeader();
                    }
                    else
                    {
                        endReached = true;
                    }
                    if (endReached)
                    {
                        break;
                    }
                }

                int toProcess = count - total;
                if (toProcess > availableBytes)
                {
                    toProcess = (int)availableBytes;
                }

                outWindow.SetLimit(toProcess);
                if (uncompressedChunk)
                {
                    inputPosition += outWindow.CopyStream(inputStream, toProcess);
                }
                else if (decoder.Code(dictionarySize, outWindow, rangeDecoder)
                         && outputSize < 0)
                {
                    availableBytes = outWindow.AvailableBytes;
                }

                int read = outWindow.Read(buffer, offset, toProcess);
                total += read;
                offset += read;
                position += read;
                availableBytes -= read;

                if (availableBytes == 0 && !uncompressedChunk)
                {
                    rangeDecoder.ReleaseStream();
                    if (!rangeDecoder.IsFinished || (rangeDecoderLimit >= 0 && rangeDecoder.Total != rangeDecoderLimit))
                    {
                        throw new DataErrorException();
                    }
                    inputPosition += rangeDecoder.Total;
                    if (outWindow.HasPending)
                    {
                        throw new DataErrorException();
                    }
                }
            }

            if (endReached)
            {
                if (inputSize >= 0 && inputPosition != inputSize)
                {
                    throw new DataErrorException();
                }
                if (outputSize >= 0 && position != outputSize)
                {
                    throw new DataErrorException();
                }
            }

            return total;
        }

        private void decodeChunkHeader()
        {
            int control = inputStream.ReadByte();
            inputPosition++;

            if (control == 0x00)
            {
                endReached = true;
                return;
            }

            if (control >= 0xE0 || control == 0x01)
            {
                needProps = true;
                needDictReset = false;
                outWindow.Reset();
            }
            else if (needDictReset)
            {
                throw new DataErrorException();
            }

            if (control >= 0x80)
            {
                uncompressedChunk = false;

                availableBytes = (control & 0x1F) << 16;
                availableBytes += (inputStream.ReadByte() << 8) + inputStream.ReadByte() + 1;
                inputPosition += 2;

                rangeDecoderLimit = (inputStream.ReadByte() << 8) + inputStream.ReadByte() + 1;
                inputPosition += 2;

                if (control >= 0xC0)
                {
                    needProps = false;
                    Properties[0] = (byte)inputStream.ReadByte();
                    inputPosition++;

                    decoder = new Decoder();
                    decoder.SetDecoderProperties(Properties);
                }
                else if (needProps)
                {
                    throw new DataErrorException();
                }
                else if (control >= 0xA0)
                {
                    decoder = new Decoder();
                    decoder.SetDecoderProperties(Properties);
                }

                rangeDecoder.Init(inputStream);
            }
            else if (control > 0x02)
            {
                throw new DataErrorException();
            }
            else
            {
                uncompressedChunk = true;
                availableBytes = (inputStream.ReadByte() << 8) + inputStream.ReadByte() + 1;
                inputPosition += 2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (encoder != null)
            {
                position = encoder.Code(new MemoryStream(buffer, offset, count), false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Properties { get; set; }
    }
}