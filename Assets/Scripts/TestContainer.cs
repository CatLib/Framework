using UnityEngine;
using System.Collections;
using CatLib.Container;
using CatLib.Base;

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

public class Test2 : ICall , IUpdate
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

public class Test3
{
    [CDependency]
    public ICall Cls { get; set; }

    public Test3(Test2 call)
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

        this.Singleton<Test2>().Alias("helloworld");
        this.Bind<Test3>().Needs<ICall>().Given<Test1>();

        Test3 t3 = this.Make<Test3>();
        t3.Call();

        Test2 t2 = this.Make<Test2>();
        t2.Call();

    }
}
