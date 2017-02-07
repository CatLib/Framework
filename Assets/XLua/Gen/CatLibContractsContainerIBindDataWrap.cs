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
    public class CatLibContractsContainerIBindDataWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(CatLib.Contracts.Container.IBindData), L, translator, 0, 3, 3, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Needs", _m_Needs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Alias", _m_Alias);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Decorator", _m_Decorator);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Service", _g_get_Service);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Concrete", _g_get_Concrete);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsStatic", _g_get_IsStatic);
            
			
			Utils.EndObjectRegister(typeof(CatLib.Contracts.Container.IBindData), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(CatLib.Contracts.Container.IBindData), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(CatLib.Contracts.Container.IBindData));
			
			
			Utils.EndClassRegister(typeof(CatLib.Contracts.Container.IBindData), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "CatLib.Contracts.Container.IBindData does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Needs(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            CatLib.Contracts.Container.IBindData __cl_gen_to_be_invoked = (CatLib.Contracts.Container.IBindData)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string service = LuaAPI.lua_tostring(L, 2);
                    
                        CatLib.Contracts.Container.ITmpData __cl_gen_ret = __cl_gen_to_be_invoked.Needs( service );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Alias(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            CatLib.Contracts.Container.IBindData __cl_gen_to_be_invoked = (CatLib.Contracts.Container.IBindData)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string alias = LuaAPI.lua_tostring(L, 2);
                    
                        CatLib.Contracts.Container.IBindData __cl_gen_ret = __cl_gen_to_be_invoked.Alias( alias );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Decorator(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            CatLib.Contracts.Container.IBindData __cl_gen_to_be_invoked = (CatLib.Contracts.Container.IBindData)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    System.Func<CatLib.Contracts.Container.IContainer, CatLib.Contracts.Container.IBindData, object, object> func = translator.GetDelegate<System.Func<CatLib.Contracts.Container.IContainer, CatLib.Contracts.Container.IBindData, object, object>>(L, 2);
                    
                        CatLib.Contracts.Container.IBindData __cl_gen_ret = __cl_gen_to_be_invoked.Decorator( func );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Service(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                CatLib.Contracts.Container.IBindData __cl_gen_to_be_invoked = (CatLib.Contracts.Container.IBindData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.Service);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Concrete(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                CatLib.Contracts.Container.IBindData __cl_gen_to_be_invoked = (CatLib.Contracts.Container.IBindData)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.Concrete);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsStatic(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                CatLib.Contracts.Container.IBindData __cl_gen_to_be_invoked = (CatLib.Contracts.Container.IBindData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.IsStatic);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
