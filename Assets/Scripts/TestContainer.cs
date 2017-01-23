using UnityEngine;
using System.Collections;
using CatLib.Container;
using CatLib.Base;
using CatLib.Contracts.Base;

public interface ICall
{
    void Call();
}

public class Test1 : ICall
{
    public void Call()
    {
        Debug.Log("this is test1");
    }
}

public class Test2 : ICall
{

    private int i = 0;

    public void Call()
    {
        Debug.Log("this is test2 " + i);
        i++;
    }

    public void Update()
    {

        Debug.Log("in update" + Time.deltaTime);
    }
}

public interface T3 { 
    
void Call();
}
public class Test3 : T3
{
    [CDependency]
    public ICall Cls { get; set; }

    public Test3(ICall call)
    {
        call.Call();
    }

    public void Call()
    {
        Cls.Call();
    }
}

public class TestContainer : CApplication {

    private void Awake()
    {

        this.Singleton<Test2>().Alias("helloworld").Alias<ICall>();
        this.Bind<Test3>().Alias<T3>().Needs<ICall>().Given<Test1>();

        ICall t3 = this.Make<ICall>();
        t3.Call();

        /*
        Test2 t2 = this.Make<Test2>();
        t2.Call();
        */
    }
}
