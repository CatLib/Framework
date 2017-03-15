using UnityEngine;
using System.Collections;
using XLua;

namespace CatLib.API.Lua
{

    public interface ILua
    {

        LuaEnv LuaEnv{ get; }

    }


}