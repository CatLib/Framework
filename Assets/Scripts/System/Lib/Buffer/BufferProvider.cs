using CatLib.Contracts.Buffer;

namespace CatLib.Buffer
{

    public class BufferProvider : ServiceProvider
    {
        public override void Register()
        {
            App.Bind<BufferBuilder>().Alias<IBufferBuilder>().Alias("network.package.base");
        }
    }
}