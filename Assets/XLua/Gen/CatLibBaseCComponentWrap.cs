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
    using Utils = XLua.Utils;
    public class CatLibBaseCComponentWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(CatLib.Base.CComponent), L, translator, 0, 0, 4, 0);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Application", _g_get_Application);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Event", _g_get_Event);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Guid", _g_get_Guid);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TypeGuid", _g_get_TypeGuid);
            
			
			Utils.EndObjectRegister(typeof(CatLib.Base.CComponent), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(CatLib.Base.CComponent), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(CatLib.Base.CComponent));
			
			
			Utils.EndClassRegister(typeof(CatLib.Base.CComponent), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					CatLib.Base.CComponent __cl_gen_ret = new CatLib.Base.CComponent();
					translator.Push(L, __cl_gen_ret);
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to CatLib.Base.CComponent constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Application(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                CatLib.Base.CComponent __cl_gen_to_be_invoked = (CatLib.Base.CComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.Application);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Event(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                CatLib.Base.CComponent __cl_gen_to_be_invoked = (CatLib.Base.CComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.Event);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Guid(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                CatLib.Base.CComponent __cl_gen_to_be_invoked = (CatLib.Base.CComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushint64(L, __cl_gen_to_be_invoked.Guid);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TypeGuid(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                CatLib.Base.CComponent __cl_gen_to_be_invoked = (CatLib.Base.CComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.TypeGuid);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
