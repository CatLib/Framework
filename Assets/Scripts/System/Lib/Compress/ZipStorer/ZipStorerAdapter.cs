
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace CatLib.Compress{

	public class ZipStorerAdapter : ICompressAdapter {

		public byte[] Compress(byte[] bytes){
		
			Stream stream = new MemoryStream();
			
			ZipStorer zip = ZipStorer.Create(stream, string.Empty);
			
			Stream zipData = new MemoryStream(bytes);
			zip.AddStream(ZipStorer.Compression.Deflate ,"catlib", zipData, DateTime.Now, string.Empty);

			bytes = zip.Close();
			return bytes;

		}

		public byte[] Expand(byte[] bytes){

			using(Stream stream = new MemoryStream(bytes)){

				using(ZipStorer zip = ZipStorer.Open(stream, FileAccess.Read)){

					List<ZipStorer.ZipFileEntry> dir = zip.ReadCentralDir();
					for(int i = 0 ; i <dir.Count ; i++)
					{
						if(System.IO.Path.GetFileName(dir[i].FilenameInZip) == "catlib"){

							zip.ExtractFile(dir[i], out bytes);
							return bytes;
						}
					}

				}

			}
			
			return null;

		}
	}

}