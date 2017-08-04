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

#if CATLIB
using CatLib.API.Converters;
using CatLib.Converters.Plan;

namespace CatLib.Converters
{
    /// <summary>
    /// 转换器服务
    /// </summary>
    public sealed class ConvertersProvider : IServiceProvider
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// 注册文件系统服务
        /// </summary>
        public void Register()
        {
            RegisterManager();
            RegisterDefaultConverter();
        }

        /// <summary>
        /// 注册管理器
        /// </summary>
        private void RegisterManager()
        {
            App.Singleton<ConvertersManager>()
                .Alias<IConvertersManager>()
                .OnResolving((_, obj) =>
                {
                    var manager = (ConvertersManager)obj;
                    manager.Extend(LoadDefaultConverters);
                    return obj;
                });
        }

        /// <summary>
        /// 注册默认的转换器
        /// </summary>
        private void RegisterDefaultConverter()
        {
            App.Bind<IConverters>((continer, _) => continer.Make<IConvertersManager>().Default);
        }

        /// <summary>
        /// 加载默认的转换器
        /// </summary>
        /// <returns>默认的转换器</returns>
        private IConverters LoadDefaultConverters()
        {
            var loadConverters = new ITypeConverter[]
            {
                new BoolStringConverter(),
                new ByteStringConverter(), 
                new CharStringConverter(), 
                new DateTimeStringConverter(), 
                new DecimalStringConverter(), 
                new DoubleStringConverter(), 
                new EnumStringConverter(), 
                new Int16StringConverter(), 
                new Int32StringConverter(), 
                new Int64StringConverter(), 
                new SByteStringConverter(),
                new SingleStringConverter(), 
                new StringBoolConverter(),
                new StringByteConverter(), 
                new StringCharConverter(), 
                new StringDateTimeConverter(), 
                new StringDecimalConverter(), 
                new StringDoubleConverter(), 
                new StringEnumConverter(), 
                new StringInt16Converter(), 
                new StringInt32Converter(), 
                new StringInt64Converter(), 
                new StringSByteConverter(), 
                new StringSingleConverter(), 
                new StringStringConverter(), 
                new StringUInt16Converter(), 
                new StringUInt32Converter(), 
                new StringUInt64Converter(), 
                new UInt16StringConverter(), 
                new UInt32StringConverter(), 
                new UInt64StringConverter(), 
            };

            var converters = new Converters();

            foreach (var tmp in loadConverters)
            {
                converters.AddConverter(tmp);
            }

            return converters;
        }
    }
}
#endif