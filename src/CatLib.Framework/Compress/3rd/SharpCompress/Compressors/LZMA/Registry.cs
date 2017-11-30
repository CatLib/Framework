using System;
using System.IO;
using System.Linq;
using CatLib._3rd.SharpCompress.Common.SevenZip;
using CatLib._3rd.SharpCompress.Compressors.LZMA.Utilites;

namespace CatLib._3rd.SharpCompress.Compressors.LZMA
{
    internal static class DecoderRegistry
    {
        private const uint k_Copy = 0x0;
        private const uint k_LZMA2 = 0x21;
        private const uint k_LZMA = 0x030101;
        private const uint k_BCJ2 = 0x0303011B;

        internal static Stream CreateDecoderStream(CMethodId id, Stream[] inStreams, byte[] info, IPasswordProvider pass,
                                                   long limit)
        {
            switch (id.Id)
            {
                case k_Copy:
                    if (info != null)
                    {
                        throw new NotSupportedException();
                    }
                    return inStreams.Single();
                case k_LZMA:
                case k_LZMA2:
                    return new LzmaStream(info, inStreams.Single(), -1, limit);
#if !NO_CRYPTO
                case CMethodId.kAESId:
                    return new AesDecoderStream(inStreams.Single(), info, pass, limit);
#endif
                case k_BCJ2:
                    return new Bcj2DecoderStream(inStreams, info, limit);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}