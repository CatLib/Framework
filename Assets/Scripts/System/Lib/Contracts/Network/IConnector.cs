using System.Collections;
using XLua;

namespace CatLib.Contracts.Network
{

    public interface IConnector
    {

        IEnumerator StartServer();

        void Disconnect();
        
    }

}
