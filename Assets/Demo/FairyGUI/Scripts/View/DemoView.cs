using CatLib.Flux;
using FairyGUI;

namespace CatLib.Demo.FairyGUI
{

    public class DemoView : View
    {

        public void Awake()
        {
            UIPackage.AddPackage("UI/DemoFairy");
            GComponent comp = UIPackage.CreateObject("DemoFairy", "DemoComponent").asCom;
            GRoot.inst.AddChild(comp);
        }

    }


}