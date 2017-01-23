using UnityEngine;
using System.Collections;

namespace CatLib.Network
{

    public interface IConnector
    {

        IEnumerator StartServer();

        void Disconnect();
        
    }

}
