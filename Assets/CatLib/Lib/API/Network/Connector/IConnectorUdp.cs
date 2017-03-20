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

    public interface IConnectorUdp : IConnectorSocket
    {

        void Send(IPackage package, string host, int port);

        void Send(byte[] data, string host, int port);

    }

}