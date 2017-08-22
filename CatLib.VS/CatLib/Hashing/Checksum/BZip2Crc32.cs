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

using System;

namespace CatLib.Hashing.Checksum
{
    /// <summary>
    /// BZip数据的Crc32校验
    /// </summary>
    public sealed class BZip2Crc32 : IChecksum
    {
        /// <summary>
        /// Crc初始化
        /// </summary>
        private const uint CrcInit = 0xFFFFFFFF;

        /// <summary>
        /// 校验表
        /// </summary>
        private static readonly uint[] table =
        {
            0X00000000, 0X04C11DB7, 0X09823B6E, 0X0D4326D9,
            0X130476DC, 0X17C56B6B, 0X1A864DB2, 0X1E475005,
            0X2608EDB8, 0X22C9F00F, 0X2F8AD6D6, 0X2B4BCB61,
            0X350C9B64, 0X31CD86D3, 0X3C8EA00A, 0X384FBDBD,
            0X4C11DB70, 0X48D0C6C7, 0X4593E01E, 0X4152FDA9,
            0X5F15ADAC, 0X5BD4B01B, 0X569796C2, 0X52568B75,
            0X6A1936C8, 0X6ED82B7F, 0X639B0DA6, 0X675A1011,
            0X791D4014, 0X7DDC5DA3, 0X709F7B7A, 0X745E66CD,
            0X9823B6E0, 0X9CE2AB57, 0X91A18D8E, 0X95609039,
            0X8B27C03C, 0X8FE6DD8B, 0X82A5FB52, 0X8664E6E5,
            0XBE2B5B58, 0XBAEA46EF, 0XB7A96036, 0XB3687D81,
            0XAD2F2D84, 0XA9EE3033, 0XA4AD16EA, 0XA06C0B5D,
            0XD4326D90, 0XD0F37027, 0XDDB056FE, 0XD9714B49,
            0XC7361B4C, 0XC3F706FB, 0XCEB42022, 0XCA753D95,
            0XF23A8028, 0XF6FB9D9F, 0XFBB8BB46, 0XFF79A6F1,
            0XE13EF6F4, 0XE5FFEB43, 0XE8BCCD9A, 0XEC7DD02D,
            0X34867077, 0X30476DC0, 0X3D044B19, 0X39C556AE,
            0X278206AB, 0X23431B1C, 0X2E003DC5, 0X2AC12072,
            0X128E9DCF, 0X164F8078, 0X1B0CA6A1, 0X1FCDBB16,
            0X018AEB13, 0X054BF6A4, 0X0808D07D, 0X0CC9CDCA,
            0X7897AB07, 0X7C56B6B0, 0X71159069, 0X75D48DDE,
            0X6B93DDDB, 0X6F52C06C, 0X6211E6B5, 0X66D0FB02,
            0X5E9F46BF, 0X5A5E5B08, 0X571D7DD1, 0X53DC6066,
            0X4D9B3063, 0X495A2DD4, 0X44190B0D, 0X40D816BA,
            0XACA5C697, 0XA864DB20, 0XA527FDF9, 0XA1E6E04E,
            0XBFA1B04B, 0XBB60ADFC, 0XB6238B25, 0XB2E29692,
            0X8AAD2B2F, 0X8E6C3698, 0X832F1041, 0X87EE0DF6,
            0X99A95DF3, 0X9D684044, 0X902B669D, 0X94EA7B2A,
            0XE0B41DE7, 0XE4750050, 0XE9362689, 0XEDF73B3E,
            0XF3B06B3B, 0XF771768C, 0XFA325055, 0XFEF34DE2,
            0XC6BCF05F, 0XC27DEDE8, 0XCF3ECB31, 0XCBFFD686,
            0XD5B88683, 0XD1799B34, 0XDC3ABDED, 0XD8FBA05A,
            0X690CE0EE, 0X6DCDFD59, 0X608EDB80, 0X644FC637,
            0X7A089632, 0X7EC98B85, 0X738AAD5C, 0X774BB0EB,
            0X4F040D56, 0X4BC510E1, 0X46863638, 0X42472B8F,
            0X5C007B8A, 0X58C1663D, 0X558240E4, 0X51435D53,
            0X251D3B9E, 0X21DC2629, 0X2C9F00F0, 0X285E1D47,
            0X36194D42, 0X32D850F5, 0X3F9B762C, 0X3B5A6B9B,
            0X0315D626, 0X07D4CB91, 0X0A97ED48, 0X0E56F0FF,
            0X1011A0FA, 0X14D0BD4D, 0X19939B94, 0X1D528623,
            0XF12F560E, 0XF5EE4BB9, 0XF8AD6D60, 0XFC6C70D7,
            0XE22B20D2, 0XE6EA3D65, 0XEBA91BBC, 0XEF68060B,
            0XD727BBB6, 0XD3E6A601, 0XDEA580D8, 0XDA649D6F,
            0XC423CD6A, 0XC0E2D0DD, 0XCDA1F604, 0XC960EBB3,
            0XBD3E8D7E, 0XB9FF90C9, 0XB4BCB610, 0XB07DABA7,
            0XAE3AFBA2, 0XAAFBE615, 0XA7B8C0CC, 0XA379DD7B,
            0X9B3660C6, 0X9FF77D71, 0X92B45BA8, 0X9675461F,
            0X8832161A, 0X8CF30BAD, 0X81B02D74, 0X857130C3,
            0X5D8A9099, 0X594B8D2E, 0X5408ABF7, 0X50C9B640,
            0X4E8EE645, 0X4A4FFBF2, 0X470CDD2B, 0X43CDC09C,
            0X7B827D21, 0X7F436096, 0X7200464F, 0X76C15BF8,
            0X68860BFD, 0X6C47164A, 0X61043093, 0X65C52D24,
            0X119B4BE9, 0X155A565E, 0X18197087, 0X1CD86D30,
            0X029F3D35, 0X065E2082, 0X0B1D065B, 0X0FDC1BEC,
            0X3793A651, 0X3352BBE6, 0X3E119D3F, 0X3AD08088,
            0X2497D08D, 0X2056CD3A, 0X2D15EBE3, 0X29D4F654,
            0XC5A92679, 0XC1683BCE, 0XCC2B1D17, 0XC8EA00A0,
            0XD6AD50A5, 0XD26C4D12, 0XDF2F6BCB, 0XDBEE767C,
            0XE3A1CBC1, 0XE760D676, 0XEA23F0AF, 0XEEE2ED18,
            0XF0A5BD1D, 0XF464A0AA, 0XF9278673, 0XFDE69BC4,
            0X89B8FD09, 0X8D79E0BE, 0X803AC667, 0X84FBDBD0,
            0X9ABC8BD5, 0X9E7D9662, 0X933EB0BB, 0X97FFAD0C,
            0XAFB010B1, 0XAB710D06, 0XA6322BDF, 0XA2F33668,
            0XBCB4666D, 0XB8757BDA, 0XB5365D03, 0XB1F740B4
        };

        /// <summary>
        /// 校验和
        /// </summary>
        private uint checkValue;

        /// <summary>
        /// 校验和
        /// </summary>
        public long Value
        {
            get
            {
                return ~checkValue;
            }
        }

        /// <summary>
        /// 构建一个BZip2Crc32
        /// </summary>
        public BZip2Crc32()
        {
            Reset();
        }

        /// <summary>
        /// 重置数据校验，恢复到初始状态
        /// </summary>
        public void Reset()
        {
            checkValue = CrcInit;
        }

        /// <summary>
        /// 添加数据进行校验
        /// </summary>
        /// <param name = "bval">要添加的数据，int的高字节被忽略</param>
        public void Update(int bval)
        {
            checkValue = unchecked(table[(byte)(((checkValue >> 24) & 0xFF) ^ bval)] ^ (checkValue << 8));
        }

        /// <summary>
        /// 使用传入的字节数组更新数据校验和
        /// </summary>
        /// <param name="buffer">字节数组</param>
        public void Update(byte[] buffer)
        {
            Update(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 对字节数组进行校验
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <param name="offset">偏移量</param>
        /// <param name="count">长度</param>
        public void Update(byte[] buffer, int offset, int count)
        {
            Guard.Requires<ArgumentNullException>(buffer != null);
            Guard.Requires<ArgumentOutOfRangeException>(offset >= 0);
            Guard.Requires<ArgumentOutOfRangeException>(offset < buffer.Length);
            Guard.Requires<ArgumentOutOfRangeException>(count >= 0);
            Guard.Requires<ArgumentOutOfRangeException>(offset + count <= buffer.Length);

            for (var i = 0; i < count; ++i)
            {
                Update(buffer[offset++]);
            }
        }
    }
}
