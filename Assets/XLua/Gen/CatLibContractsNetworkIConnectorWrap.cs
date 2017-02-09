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
    public class CatLibContractsNetworkIConnectorWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(CatLib.Contracts.Network.IConnector), L, translator, 0, 2, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StartServer", _m_StartServer);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Disconnect", _m_Disconnect);
			
			
			
			
			Utils.EndObjectRegister(typeof(CatLib.Contracts.Network.IConnector), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(CatLib.Contracts.Network.IConnector), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(CatLib.Contracts.Network.IConnector));
			
			
			Utils.EndClassRegister(typeof(CatLib.Contracts.Network.IConnector), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "CatLib.Contracts.Network.IConnector does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartServer(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            CatLib.Contracts.Network.IConnector __cl_gen_to_be_invoked = (CatLib.Contracts.Network.IConnector)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                        System.Collections.IEnumerator __cl_gen_ret = __cl_gen_to_be_invoked.StartServer(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Disconnect(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            CatLib.Contracts.Network.IConnector __cl_gen_to_be_invoked = (CatLib.Contracts.Network.IConnector)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.Disconnect(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
