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

    public class FluxProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Singleton<FluxDispatcher>((app , param) =>
            {
                return new FluxDispatcher();
            }).Alias<IFluxDispatcher>();

            App.Bind<Notification>((app, param) =>
            {
                if (param.Length <= 0) {
                    return new Notification("undefined");
                }else if(param.Length <= 1)
                {
                    return new Notification(param[0].ToString());
                }else
                {
                    return new Notification(param[0].ToString(), param[1]);
                }
            }).Alias<INotification>();
        }

    }

}