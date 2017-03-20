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
 
namespace CatLib.API.Buffer
{

    public interface IBufferBuilder
    {

        byte[] Byte { get; set; }

        int IndexOf(params byte[] data);

        void Push(byte[] data);

        byte[] Pop(int count = 1);

        void Unshift(byte[] data);

        byte[] Shift(int count = 1);

        byte[] Peek(int count = 1);

        int Length { get; }

        void Clear();

    }

}