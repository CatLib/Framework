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
 
namespace CatLib.API
{
    public class ApplicationEvents
    {

        public static readonly string ON_INITING = "application.initing";

        public static readonly string ON_INITED = "application.inited";

        public static readonly string ON_PROVIDER_PROCESSING = "application.provider.processing";

        public static readonly string ON_PROVIDER_PROCESSED = "application.provider.processed";

        public static readonly string ON_APPLICATION_START_COMPLETE = "application.start.complete";

    }

}