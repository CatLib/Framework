/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
 
using CatLib.API.Buffer;
using CatLib.API.Network;
using System.Collections;
using CatLib.API.Config;

namespace CatLib.Network
{

    /// <summary>
    /// 网络服务提供商
    /// </summary>
    public class NetworkProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Singleton<Network>().Alias<INetworkFactory>().Resolving((app , bind , obj)=>{

                IConfigStore config = app.Make<IConfigStore>();
                Network network = obj as Network;

                network.SetQuery((name) => config.Get<Hashtable>(typeof(Network) , name , null));

                return obj;

            });
            App.Bind<HttpWebRequest>().Alias<IConnectorHttp>().Alias("network.hwr");
            App.Bind<WebRequest>().Alias("network.uwr");
            App.Bind<TcpRequest>().Alias<IConnectorTcp>().Alias("network.tcp");
            App.Bind<UdpRequest>().Alias<IConnectorUdp>().Alias("network.udp");
        }


    }

}