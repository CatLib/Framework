using UnityEngine;

namespace CatLib.Lua
{

    public static class LuaEditor
    {
        [CSObjectWrapEditor.GenPath]
        public static string GenPath = UnityEngine.Application.dataPath + "/Scripts/System/Lib/Lua/XLua/Gen/";
    }

}