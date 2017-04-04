using CatLib.Flux;
using FairyGUI;
using CatLib.API.FairyGUI;

namespace CatLib.Demo.FairyGUI
{

    public class DemoView : View
    {

        public void Awake()
        {
            IPackage package = App.Instance.Make<IPackage>();
            package.AddPackage("UI/DemoFairy");
            GComponent comp = UIPackage.CreateObject("DemoFairy", "DemoComponent").asCom;
            GRoot.inst.AddChild(comp);
        }

    }


}