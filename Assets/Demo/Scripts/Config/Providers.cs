using System;
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
                typeof(INIProvider),
                typeof(TranslationProvider),
                typeof(JSONProvider),
                typeof(CompressProvider),
                typeof(ConfigProvider),
                typeof(ProtobufProvider),
                typeof(CsvParserProvider),
                typeof(CsvStoreProvider),
                typeof(DataTableProvider),

                //以下是测试的提供商,框架本体并不会带有这些提供商
                typeof(Bootstrap),
                typeof(IOCryptProvider),

            };
        }
    }
}
