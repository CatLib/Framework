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

namespace CatLib.API.IO
{

    public interface IFile
    {

        string Extension{ get; }

        string FullName{ get; }

        string Name{ get; }

        string NameWithoutExtension{ get; }

        bool Exists{ get; }

        IDirectory Directory{ get; }

        long Length{ get; }

        void Delete();

        IFile CopyTo(string targetFileName);

        IFile CopyTo(IDirectory targetDirectory);

        void MoveTo(IDirectory targetDirectory);

        void MoveTo(string targetDirectory);

        void Rename(string newName);

        void Create(byte[] array);

        void CreateAsync(byte[] array , Action callback);

        byte[] Read(); 

        void ReadAsync(Action<byte[]> callback);

    }
}
