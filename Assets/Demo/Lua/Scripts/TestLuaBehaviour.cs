using UnityEngine;
using CatLib.API.Lua;
using CatLib;
using XLua;

[LuaCallCSharp]
public class TestLuaBehaviour : LuaMonoComponent {

    public override void Init()
    {
        //设置代码路径

        //父类方法执行Lua脚本
        base.Init();
    }
}
