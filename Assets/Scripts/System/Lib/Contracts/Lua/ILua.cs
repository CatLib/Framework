using UnityEngine;
using System.Collections;
using XLua;

namespace CatLib.Contracts.Lua
{

    [LuaCallCSharp]
    public interface ILua
    {

        LuaEnv LuaEnv{ get; }

    }


}