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
 
using System.IO;
using ICSharpCode.SharpZipLib.GZip;

namespace CatLib.Compress{

	public class ShareZipLibAdapter : ICompressAdapter {

		public byte[] Compress(byte[] bytes , int level){

			using(MemoryStream ms = new MemoryStream()){

				GZipOutputStream gzip = new GZipOutputStream(ms);
				gzip.Write(bytes, 0, bytes.Length);
				gzip.SetLevel(level);
				gzip.Close();

				return ms.ToArray();

			}
		}

		public byte[] UnCompress(byte[] bytes){

			using(MemoryStream ms = new MemoryStream()){

				GZipInputStream gzip = new GZipInputStream(new MemoryStream(bytes));
				int count=0;
				byte[] data = new byte[4096];
				while ((count = gzip.Read(data, 0, data.Length)) != 0)
				{
					ms.Write(data, 0 , count);
				}
				return ms.ToArray();

			}
		}
		
	}

}