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
using CatLib.API.Event;

namespace CatLib
{

    public class Component : CatLibObject , IEvent , IGuid
    {

        public IApplication App { get { return CatLib.App.Instance; } }


        private IEventAchieve cevent = null;
        public IEventAchieve Event
        {
            get
            {
                if (cevent == null) { cevent = CatLib.App.Instance.Make<IEventAchieve>(); }
                return cevent;
            }
        }

        private long guid;

        public long Guid
        {
            get
            {

                if (guid <= 0)
                {
                    guid = App.GetGuid();
                }
                return guid;
            }
        }
        public string TypeGuid
        {
            get
            {
                return GetType().ToString() + "-" + Guid;
            }
        }

    }

}
