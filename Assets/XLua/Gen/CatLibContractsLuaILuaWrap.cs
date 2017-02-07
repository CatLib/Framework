#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace CSObjectWrap
{
    public class CatLibContractsLuaILuaWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(CatLib.Contracts.Lua.ILua), L, translator, 0, 0, 1, 0);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaEnv", _g_get_LuaEnv);
            
			
			Utils.EndObjectRegister(typeof(CatLib.Contracts.Lua.ILua), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(CatLib.Contracts.Lua.ILua), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(CatLib.Contracts.Lua.ILua));
			
			
			Utils.EndClassRegister(typeof(CatLib.Contracts.Lua.ILua), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "CatLib.Contracts.Lua.ILua does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaEnv(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                CatLib.Contracts.Lua.ILua __cl_gen_to_be_invoked = (CatLib.Contracts.Lua.ILua)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.LuaEnv);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
