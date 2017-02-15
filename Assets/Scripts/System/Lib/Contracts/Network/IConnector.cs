using System.Collections;
using XLua;

namespace CatLib.Contracts.Network
{

    public interface IConnector
    {

        string Alias { get; set; }

        IEnumerator StartServer();

        void Disconnect();
        
    }

}
