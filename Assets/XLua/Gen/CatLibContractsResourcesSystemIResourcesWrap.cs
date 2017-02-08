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
    public class CatLibContractsResourcesSystemIResourcesWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(CatLib.Contracts.ResourcesSystem.IResources), L, translator, 0, 5, 1, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Load", _m_Load);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadAll", _m_LoadAll);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadAsyn", _m_LoadAsyn);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadAllAsyn", _m_LoadAllAsyn);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Unload", _m_Unload);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Variant", _g_get_Variant);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Variant", _s_set_Variant);
            
			Utils.EndObjectRegister(typeof(CatLib.Contracts.ResourcesSystem.IResources), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(CatLib.Contracts.ResourcesSystem.IResources), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(CatLib.Contracts.ResourcesSystem.IResources));
			
			
			Utils.EndClassRegister(typeof(CatLib.Contracts.ResourcesSystem.IResources), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "CatLib.Contracts.ResourcesSystem.IResources does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Load(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            CatLib.Contracts.ResourcesSystem.IResources __cl_gen_to_be_invoked = (CatLib.Contracts.ResourcesSystem.IResources)translator.FastGetCSObj(L, 1);
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string path = LuaAPI.lua_tostring(L, 2);
                    
                        UnityEngine.Object __cl_gen_ret = __cl_gen_to_be_invoked.Load( path );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Type>(L, 3)) 
                {
                    string path = LuaAPI.lua_tostring(L, 2);
                    System.Type type = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    
                        UnityEngine.Object __cl_gen_ret = __cl_gen_to_be_invoked.Load( path, type );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CatLib.Contracts.ResourcesSystem.IResources.Load!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAll(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            CatLib.Contracts.ResourcesSystem.IResources __cl_gen_to_be_invoked = (CatLib.Contracts.ResourcesSystem.IResources)translator.FastGetCSObj(L, 1);
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string path = LuaAPI.lua_tostring(L, 2);
                    
                        UnityEngine.Object[] __cl_gen_ret = __cl_gen_to_be_invoked.LoadAll( path );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Type>(L, 3)) 
                {
                    string path = LuaAPI.lua_tostring(L, 2);
                    System.Type type = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    
                        UnityEngine.Object[] __cl_gen_ret = __cl_gen_to_be_invoked.LoadAll( path, type );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CatLib.Contracts.ResourcesSystem.IResources.LoadAll!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAsyn(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            CatLib.Contracts.ResourcesSystem.IResources __cl_gen_to_be_invoked = (CatLib.Contracts.ResourcesSystem.IResources)translator.FastGetCSObj(L, 1);
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<UnityEngine.Object>>(L, 3)) 
                {
                    string path = LuaAPI.lua_tostring(L, 2);
                    System.Action<UnityEngine.Object> callback = translator.GetDelegate<System.Action<UnityEngine.Object>>(L, 3);
                    
                        UnityEngine.Coroutine __cl_gen_ret = __cl_gen_to_be_invoked.LoadAsyn( path, callback );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Type>(L, 3)&& translator.Assignable<System.Action<UnityEngine.Object>>(L, 4)) 
                {
                    string path = LuaAPI.lua_tostring(L, 2);
                    System.Type type = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    System.Action<UnityEngine.Object> callback = translator.GetDelegate<System.Action<UnityEngine.Object>>(L, 4);
                    
                        UnityEngine.Coroutine __cl_gen_ret = __cl_gen_to_be_invoked.LoadAsyn( path, type, callback );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CatLib.Contracts.ResourcesSystem.IResources.LoadAsyn!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAllAsyn(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            CatLib.Contracts.ResourcesSystem.IResources __cl_gen_to_be_invoked = (CatLib.Contracts.ResourcesSystem.IResources)translator.FastGetCSObj(L, 1);
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<UnityEngine.Object[]>>(L, 3)) 
                {
                    string path = LuaAPI.lua_tostring(L, 2);
                    System.Action<UnityEngine.Object[]> callback = translator.GetDelegate<System.Action<UnityEngine.Object[]>>(L, 3);
                    
                        UnityEngine.Coroutine __cl_gen_ret = __cl_gen_to_be_invoked.LoadAllAsyn( path, callback );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Type>(L, 3)&& translator.Assignable<System.Action<UnityEngine.Object[]>>(L, 4)) 
                {
                    string path = LuaAPI.lua_tostring(L, 2);
                    System.Type type = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    System.Action<UnityEngine.Object[]> callback = translator.GetDelegate<System.Action<UnityEngine.Object[]>>(L, 4);
                    
                        UnityEngine.Coroutine __cl_gen_ret = __cl_gen_to_be_invoked.LoadAllAsyn( path, type, callback );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CatLib.Contracts.ResourcesSystem.IResources.LoadAllAsyn!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unload(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            CatLib.Contracts.ResourcesSystem.IResources __cl_gen_to_be_invoked = (CatLib.Contracts.ResourcesSystem.IResources)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    bool unloadAllLoadedObjects = LuaAPI.lua_toboolean(L, 2);
                    
                    __cl_gen_to_be_invoked.Unload( unloadAllLoadedObjects );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Variant(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                CatLib.Contracts.ResourcesSystem.IResources __cl_gen_to_be_invoked = (CatLib.Contracts.ResourcesSystem.IResources)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.Variant);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Variant(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                CatLib.Contracts.ResourcesSystem.IResources __cl_gen_to_be_invoked = (CatLib.Contracts.ResourcesSystem.IResources)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.Variant = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
