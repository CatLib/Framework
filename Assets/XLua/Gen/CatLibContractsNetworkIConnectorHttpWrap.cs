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
    public class CatLibContractsNetworkIConnectorHttpWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(CatLib.Contracts.Network.IConnectorHttp), L, translator, 0, 4, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetUrl", _m_SetUrl);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Post", _m_Post);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Put", _m_Put);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Get", _m_Get);
			
			
			
			
			Utils.EndObjectRegister(typeof(CatLib.Contracts.Network.IConnectorHttp), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(CatLib.Contracts.Network.IConnectorHttp), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(CatLib.Contracts.Network.IConnectorHttp));
			
			
			Utils.EndClassRegister(typeof(CatLib.Contracts.Network.IConnectorHttp), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "CatLib.Contracts.Network.IConnectorHttp does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUrl(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            CatLib.Contracts.Network.IConnectorHttp __cl_gen_to_be_invoked = (CatLib.Contracts.Network.IConnectorHttp)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string url = LuaAPI.lua_tostring(L, 2);
                    
                        CatLib.Contracts.Network.IConnectorHttp __cl_gen_ret = __cl_gen_to_be_invoked.SetUrl( url );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Post(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            CatLib.Contracts.Network.IConnectorHttp __cl_gen_to_be_invoked = (CatLib.Contracts.Network.IConnectorHttp)translator.FastGetCSObj(L, 1);
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    byte[] bytes = LuaAPI.lua_tobytes(L, 2);
                    
                    __cl_gen_to_be_invoked.Post( bytes );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Collections.Generic.Dictionary<string, string>>(L, 3)) 
                {
                    string action = LuaAPI.lua_tostring(L, 2);
                    System.Collections.Generic.Dictionary<string, string> fields = (System.Collections.Generic.Dictionary<string, string>)translator.GetObject(L, 3, typeof(System.Collections.Generic.Dictionary<string, string>));
                    
                    __cl_gen_to_be_invoked.Post( action, fields );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string action = LuaAPI.lua_tostring(L, 2);
                    byte[] bytes = LuaAPI.lua_tobytes(L, 3);
                    
                    __cl_gen_to_be_invoked.Post( action, bytes );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.WWWForm>(L, 3)) 
                {
                    string action = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.WWWForm formData = (UnityEngine.WWWForm)translator.GetObject(L, 3, typeof(UnityEngine.WWWForm));
                    
                    __cl_gen_to_be_invoked.Post( action, formData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CatLib.Contracts.Network.IConnectorHttp.Post!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Put(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            CatLib.Contracts.Network.IConnectorHttp __cl_gen_to_be_invoked = (CatLib.Contracts.Network.IConnectorHttp)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string action = LuaAPI.lua_tostring(L, 2);
                    byte[] bodyData = LuaAPI.lua_tobytes(L, 3);
                    
                    __cl_gen_to_be_invoked.Put( action, bodyData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Get(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            CatLib.Contracts.Network.IConnectorHttp __cl_gen_to_be_invoked = (CatLib.Contracts.Network.IConnectorHttp)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string action = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.Get( action );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
