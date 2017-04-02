
using CatLib.Flux;

public class DemoStore : Store {


    private int count;


    DemoStore(string storeName) : base(storeName)
    {
        count = 0;
    }

    protected override void OnDispatch(INotification notification)
    {
        switch(notification.Name)
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
