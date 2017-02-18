
using CatLib.Contracts.Buffer;

namespace CatLib.Contracts.Network
{

    /// <summary>
    /// 数据渲染流
    /// </summary>
    public interface IRender
    {

        void Decode(IBufferBuilder bufferBuilder);

        void Encode(IBufferBuilder bufferBuilder);

    }

}