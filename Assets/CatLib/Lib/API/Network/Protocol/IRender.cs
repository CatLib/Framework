
using CatLib.API.Buffer;

namespace CatLib.API.Network
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