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
 
using CatLib.API.TimeQueue;

namespace CatLib.TimeQueue
{

    public class TimeQueueProvider : ServiceProvider
    {
        public override void Register()
        {
            App.Bind<TimeQueue>((app , param)=>{
                
                return app.Make<TimeRunner>().CreateQueue();

            }).Alias<ITimeQueue>();
            App.Singleton<TimeRunner>();
        }
    }

}