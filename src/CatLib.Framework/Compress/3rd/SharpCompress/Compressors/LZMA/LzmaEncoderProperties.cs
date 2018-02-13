namespace CatLib._3rd.SharpCompress.Compressors.LZMA
{
    /// <summary>
    /// 
    /// </summary>
    public class LzmaEncoderProperties
    {
        internal CoderPropID[] propIDs;
        internal object[] properties;

        /// <summary>
        /// 
        /// </summary>
        public LzmaEncoderProperties()
            : this(false)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eos"></param>
        public LzmaEncoderProperties(bool eos)
            : this(eos, 1 << 20)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eos"></param>
        /// <param name="dictionary"></param>
        public LzmaEncoderProperties(bool eos, int dictionary)
            : this(eos, dictionary, 32)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eos"></param>
        /// <param name="dictionary"></param>
        /// <param name="numFastBytes"></param>
        public LzmaEncoderProperties(bool eos, int dictionary, int numFastBytes)
        {
            int posStateBits = 2;
            int litContextBits = 3;
            int litPosBits = 0;
            int algorithm = 2;
            string mf = "bt4";

            propIDs = new[]
                      {
                          CoderPropID.DictionarySize,
                          CoderPropID.PosStateBits,
                          CoderPropID.LitContextBits,
                          CoderPropID.LitPosBits,
                          CoderPropID.Algorithm,
                          CoderPropID.NumFastBytes,
                          CoderPropID.MatchFinder,
                          CoderPropID.EndMarker
                      };
            properties = new object[]
                         {
                             dictionary,
                             posStateBits,
                             litContextBits,
                             litPosBits,
                             algorithm,
                             numFastBytes,
                             mf,
                             eos
                         };
        }
    }
}