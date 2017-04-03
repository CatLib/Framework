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

using CatLib.Flux;
using CatLib.API.Flux;

public class DemoStore : Store {

    public const string ADD = "add";

    private static DemoStore instance;

    public static DemoStore Get(IFluxDispatcher dispatcher)
    {
        if (instance == null)
        {
            instance = new DemoStore(dispatcher);
        }
        return instance;
    }

    private int count;

    public DemoStore(IFluxDispatcher dispatcher) : base(dispatcher)
    {
        count = 0;
    }

    protected override void OnDispatch(INotification notification)
    {
        switch(notification.Action) 
        {
            case ADD:
                Add();
                Change();
                break;
        }
    }

    public int GetCount()
    {
        return count;
    }

    protected void Add()
    {
        count++;
    }

}
