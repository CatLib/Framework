using CatLib.Flux;
using FairyGUI;
using CatLib.API.FairyGUI;

namespace CatLib.Demo.FairyGUI
{
    public class DemoView : View
    {
        public void Awake()
        {
            var package = App.Instance.Make<IPackage>();
            //package.AddPackage("UI/DemoPackage");
            //package.AddPackage("package/DemoPackage");
            //var comp = UIPackage.CreateObject("DemoPackage", "DemoUI").asCom;
            //GRoot.inst.AddChild(comp);

            package.AddPackageAsync("package", (uipackage) =>
            {
                var comp = UIPackage.CreateObject("DemoPackage", "DemoUI").asCom;
                GRoot.inst.AddChild(comp);
            });
        }
    }
}