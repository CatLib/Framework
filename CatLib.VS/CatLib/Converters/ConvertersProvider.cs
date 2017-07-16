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

using CatLib.API;
using CatLib.API.Converters;
using CatLib.Converters.Plan;

namespace CatLib.Converters
{
    /// <summary>
    /// 转换器服务
    /// </summary>
    public sealed class ConvertersProvider : ServiceProvider
    {
        /// <summary>
        /// 注册文件系统服务
        /// </summary>
        public override void Register()
        {
            RegisterManager();
        }

        /// <summary>
        /// 注册管理器
        /// </summary>
        private void RegisterManager()
        {
            App.Singleton<ConvertersManager>()
                .Alias<IConvertersManager>()
                .Alias("catlib.converters.manager")
                .OnResolving((binder, obj) =>
                {
                    var manager = obj as ConvertersManager;
                    manager.Extend(LoadDefaultConverters);
                    return obj;
                });
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
                new StringBoolConverter(),
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
