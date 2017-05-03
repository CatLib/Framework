using UnityEngine;
using CatLib.API.Lua;
using CatLib.Demo.Lua;
using XLua;

[LuaCallCSharp]
public class TestLuaBehaviour : LuaMonoComponent {

    public override void Init()
    {
        //设置代码路径
        luaPath = "scripts/test/LuaTestScript.lua.txt";
        //父类方法执行Lua脚本
        base.Init();
    }
}
