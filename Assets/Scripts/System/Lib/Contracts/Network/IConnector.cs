using UnityEngine;
using System.Collections;

namespace CatLib.Contracts.Network
{

    public interface IConnector
    {

        IEnumerator StartServer();

        void Disconnect();
        
    }

}
