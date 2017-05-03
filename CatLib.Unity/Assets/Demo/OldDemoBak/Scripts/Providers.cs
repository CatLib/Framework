﻿using System;
using CatLib;
using CatLib.Event;
using CatLib.Lua;
using CatLib.Network;
using CatLib.NetPackage;
using CatLib.AutoUpdate;
using CatLib.Resources;
using CatLib.Buffer;
using CatLib.Thread;
using CatLib.Time;
using CatLib.IO;
using CatLib.Crypt;
using CatLib.Hash;
using CatLib.TimeQueue;
using CatLib.INI;
using CatLib.Translation;
using CatLib.Json;
using CatLib.Compress;
using CatLib.Config;
using CatLib.Protobuf;
using CatLib.Csv;
using CatLib.CsvStore;
using CatLib.DataTable;
using CatLib.LocalSetting;
using CatLib.FilterChain;
using CatLib.Routing;
using CatLib.LruCache;

public class Providers{

    /// <summary>
    /// 服务提供者
    /// </summary>
	public static Type[] ServiceProviders
    {
        get
        {
            return new Type[] {

                typeof(NetPackageProvider),
                typeof(AutoUpdateProvider),
                typeof(ResourcesProvider),
                typeof(EventProvider),
                typeof(NetworkProvider),
                typeof(LuaProvider),
                typeof(IOProvider),
                typeof(BufferProvider),
                typeof(ThreadProvider),
                typeof(TimeProvider),
                typeof(CryptProvider),
                typeof(HashProvider),
                typeof(CoreProvider),
                typeof(TimeQueueProvider),
                typeof(IniProvider),
                typeof(TranslationProvider),
                typeof(JsonProvider),
                typeof(CompressProvider),
                typeof(ConfigProvider),
                typeof(ProtobufProvider),
                typeof(CsvParserProvider),
                typeof(CsvStoreProvider),
                typeof(DataTableProvider),
                typeof(LocalSettingProvider),
                typeof(FilterChainProvider),
                typeof(RoutingProvider),
                typeof(LruCacheProvider),

                //以下是测试的提供商,框架本体并不会带有这些提供商
                typeof(Bootstrap),
                typeof(IOCryptProvider),

            };
        }
    }
}
