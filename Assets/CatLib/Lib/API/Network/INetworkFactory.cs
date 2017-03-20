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
 
namespace CatLib.API.Network
{

    /// <summary>
    /// 网络服务
    /// </summary>
    public interface INetworkFactory
    {
        T Create<T>(string name) where T : IConnector;

        void Destroy(string name);

        T Get<T>(string name) where T : IConnector;
    }
}
