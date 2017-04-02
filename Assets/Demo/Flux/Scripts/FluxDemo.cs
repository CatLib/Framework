
using CatLib.API;
namespace CatLib.Demo.Flux
{

    public class FluxDemo : ServiceProvider
    {

        public override void Init()
        {
            App.On(ApplicationEvents.ON_APPLICATION_START_COMPLETE, (sender, e) =>
            {

                UnityEngine.Debug.Log("this is demo");
                //todo:

            });
        }

        public override void Register() { }

    }

}