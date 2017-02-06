using System.Collections;
using XLua;

namespace CatLib.Contracts.Network
{

    [LuaCallCSharp]
    public interface IConnector
    {

        IEnumerator StartServer();

        void Disconnect();
        
    }

}
