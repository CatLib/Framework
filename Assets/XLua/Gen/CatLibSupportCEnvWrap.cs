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
    public class CatLibSupportCEnvWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(CatLib.Support.CEnv), L, translator, 0, 0, 0, 0);
			
			
			
			
			
			Utils.EndObjectRegister(typeof(CatLib.Support.CEnv), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(CatLib.Support.CEnv), L, __CreateInstance, 2, 6, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "PlatformToName", _m_PlatformToName_xlua_st_);
            
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(CatLib.Support.CEnv));
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "StreamingAssetsPath", _g_get_StreamingAssetsPath);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "DataPath", _g_get_DataPath);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "PersistentDataPath", _g_get_PersistentDataPath);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "AssetPath", _g_get_AssetPath);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Platform", _g_get_Platform);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "SwitchPlatform", _g_get_SwitchPlatform);
            
			
			Utils.EndClassRegister(typeof(CatLib.Support.CEnv), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					CatLib.Support.CEnv __cl_gen_ret = new CatLib.Support.CEnv();
					translator.Push(L, __cl_gen_ret);
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to CatLib.Support.CEnv constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlatformToName_xlua_st_(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 1&& translator.Assignable<System.Nullable<UnityEngine.RuntimePlatform>>(L, 1)) 
                {
                    System.Nullable<UnityEngine.RuntimePlatform> platform;translator.Get(L, 1, out platform);
                    
                        string __cl_gen_ret = CatLib.Support.CEnv.PlatformToName( platform );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 0) 
                {
                    
                        string __cl_gen_ret = CatLib.Support.CEnv.PlatformToName(  );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CatLib.Support.CEnv.PlatformToName!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_StreamingAssetsPath(RealStatePtr L)
        {
            
            try {
			    LuaAPI.lua_pushstring(L, CatLib.Support.CEnv.StreamingAssetsPath);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DataPath(RealStatePtr L)
        {
            
            try {
			    LuaAPI.lua_pushstring(L, CatLib.Support.CEnv.DataPath);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PersistentDataPath(RealStatePtr L)
        {
            
            try {
			    LuaAPI.lua_pushstring(L, CatLib.Support.CEnv.PersistentDataPath);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetPath(RealStatePtr L)
        {
            
            try {
			    LuaAPI.lua_pushstring(L, CatLib.Support.CEnv.AssetPath);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Platform(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.Push(L, CatLib.Support.CEnv.Platform);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SwitchPlatform(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.Push(L, CatLib.Support.CEnv.SwitchPlatform);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
