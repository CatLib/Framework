using UnityEngine;
using System.Collections;
using XLua;

namespace CatLib.Contracts.Lua
{

    public interface ILua
    {

        LuaEnv LuaEnv{ get; }

    }


}