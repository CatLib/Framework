using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatLib.Lua
{

    public static class CLuaEditor
    {
        [CSObjectWrapEditor.GenPath]
        public static string GenPath = Application.dataPath + "/Scripts/System/Lib/Lua/XLua/Gen/";
    }

}