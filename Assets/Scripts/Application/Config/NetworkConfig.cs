using System;
using System.Collections;
using CatLib;
using CatLib.Network;
using CatLib.Contracts.Network;
using CatLib.Base;

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

                //注意测试地址 3300 端口只能走 text 协议 (UDP协议)
                //注意测试地址 3301 端口只能走 frame 协议 (UDP协议)

                //注意测试地址 3302 端口只能走 text 协议 (TCP协议)
                //注意测试地址 3303 端口只能走 frame 协议 (TCP协议)

                "tcp.frame" , new Hashtable(){ 
                                            { "host", "pvp.gift" },
                                            { "port", 3303 },
                                            { "packing"  , typeof(IPacking) },
                                            { "protocol" , typeof(IProtocol) },
                                            { "trigger" , new Hashtable() {
                                                { SocketRequestEvents.ON_MESSAGE , TriggerLevel.SELF }
                                            } }
                                        },

                "tcp.text" , new Hashtable(){
                                            { "host", "pvp.gift" },
                                            { "port", 3302 },
                                            { "packing"  , "network.packing.text" },
                                            { "protocol" , typeof(IProtocol) },
                                            { "trigger" , new Hashtable() {
                                                { SocketRequestEvents.ON_MESSAGE , TriggerLevel.SELF | TriggerLevel.TYPE }
                                            } }
                                        },
                "udp.bind.host.text" , new Hashtable(){
                                            { "host", "pvp.gift" },
                                            { "port", 3300 },
                                            { "packing"  , "network.packing.text" },
                                            { "protocol" , "network.protocol.text" },
                                        },
                "udp.bind.host.frame" , new Hashtable(){
                                            { "host", "pvp.gift" },
                                            { "port", 3301 },
                                            { "packing"  , "network.packing.frame" },
                                            { "protocol" , "network.protocol.text" },
                                        },

                "udp.unbind.host.frame" , new Hashtable(){
                                            { "packing"  , "network.packing.frame" },
                                            { "protocol" , "network.protocol.text" },
                                        },

            };
        }
    }

}
