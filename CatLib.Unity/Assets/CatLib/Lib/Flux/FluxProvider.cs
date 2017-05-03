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

using CatLib.API.Flux;

namespace CatLib.Flux
{
    /// <summary>
    /// Flux服务提供商
    /// </summary>
    public sealed class FluxProvider : ServiceProvider
    {
        /// <summary>
        /// 注册Flux服务
        /// </summary>
        public override void Register()
        {
            App.Singleton<FluxDispatcher>((app, param) => new FluxDispatcher()).Alias<IFluxDispatcher>();

            App.Bind<FluxAction>((app, param) =>
            {
                if (param == null || param.Length <= 0)
                {
                    return new FluxAction("undefined");
                }
                return param.Length <= 1 ? new FluxAction(param[0].ToString()) : new FluxAction(param[0].ToString(), param[1]);
            }).Alias<IAction>();
        }
    }
}