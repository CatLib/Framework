using System;
using System.Collections;
using CatLib;
using CatLib.Network;
using CatLib.Contracts.Network;
using System.Net;

public class NetworkConfig : Configs {

    /// <summary>
    /// 类
    /// </summary>
    public override Type Class
    {
        get
        {
            return typeof(Network);
        }
    }

    /// <summary>
    /// 配置
    /// </summary>
    protected override object[] Field
    {
        get
        {
            return new object[]
            {
                "connector.test"    , new Hashtable(){ 
                                            { "host" , "http://127.0.0.1/testcookie.php" },
                                            { "timeout" , 1000 },
                                    },
                "connector.test.tcp" , new Hashtable(){ 
                                            { "host", "127.0.0.1" },
                                            { "port", 3317 },
                                            { "packing"  , typeof(IPacking) },
                                            { "protocol" , typeof(IProtocol) },
                                        },

                "connector.test.tcp.packing.text" , new Hashtable(){
                                            { "host", "127.0.0.1" },
                                            { "port", 3317 },
                                            { "packing"  , "network.packing.text" },
                                            { "protocol" , "network.protocol.text" },
                                        },
                "test.udp" , new Hashtable(){
                                            { "host", "127.0.0.1" },
                                            { "port", 3300 },
                                            { "packing"  , "network.packing.text" },
                                            { "protocol" , "network.protocol.text" },
                                        },
                "test.udp.noset.default" , new Hashtable(){
                                            { "packing"  , "network.packing.text" },
                                            { "protocol" , "network.protocol.text" },
                                        },
            };
        }
    }

}
