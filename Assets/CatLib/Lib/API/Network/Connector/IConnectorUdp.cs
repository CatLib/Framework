
namespace CatLib.API.Network
{

    public interface IConnectorUdp : IConnectorSocket
    {

        void Send(IPackage package, string host, int port);

        void Send(byte[] data, string host, int port);

    }

}