
using CatLib.API.Protobuf;

namespace CatLib.Protobuf
{

    public class ProtobufProvider : ServiceProvider
    {
        public override void Register()
        {
            RegisterProtobufAdapter();
            App.Singleton<Protobuf>().Alias<IProtobuf>().Alias("protobuf");
        }

        protected void RegisterProtobufAdapter()
        {
            App.Singleton<ProtobufNetAdapter>().Alias<IProtobufAdapter>();
        }
    }

}