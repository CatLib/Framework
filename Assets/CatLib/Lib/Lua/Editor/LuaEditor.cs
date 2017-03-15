using UnityEngine;

namespace CatLib.Lua
{

    public static class LuaEditor
    {
        [CSObjectWrapEditor.GenPath]
        public static string GenPath = UnityEngine.Application.dataPath + "/CatLib/Lib/Lua/XLua/Gen/";
    }

}