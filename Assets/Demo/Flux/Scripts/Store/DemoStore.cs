
using CatLib.Flux;
using CatLib.API.Flux;

public class DemoStore : Store {

    private static DemoStore instance;

    public static DemoStore get(IFluxDispatcher dispatcher)
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
            case "add":
                Add();
                break;
        }
        Change();
    }

    public int GetCount()
    {
        return count;
    }

    public void Add()
    {
        count++;
    }

}
