using FairyGUI;
using UnityEngine;

namespace CatLib.FairyGUI
{
    /// <summary>
    /// 加载器
    /// </summary>
    public class Loader : GLoader
    {
        protected override void LoadExternal()
        {
            Debug.Log(url);
            base.LoadExternal();
        }
    }
}