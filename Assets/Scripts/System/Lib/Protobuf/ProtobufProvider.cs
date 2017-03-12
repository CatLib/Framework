
using CatLib.API.Protobuf;

namespace CatLib.Protobuf
{

    public class ProtobufProvider : ServiceProvider
    {
        public override void Register()
        {
            App.Singleton<Protobuf>().Alias<IProtobuf>().Alias("protobuf");
        }
    }

}