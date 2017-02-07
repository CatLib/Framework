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
    public class CatLibContractsContainerIContainerWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(CatLib.Contracts.Container.IContainer), L, translator, 0, 4, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Bind", _m_Bind);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Make", _m_Make);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Alias", _m_Alias);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Decorator", _m_Decorator);
			
			
			
			
			Utils.EndObjectRegister(typeof(CatLib.Contracts.Container.IContainer), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(CatLib.Contracts.Container.IContainer), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(CatLib.Contracts.Container.IContainer));
			
			
			Utils.EndClassRegister(typeof(CatLib.Contracts.Container.IContainer), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "CatLib.Contracts.Container.IContainer does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Bind(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            CatLib.Contracts.Container.IContainer __cl_gen_to_be_invoked = (CatLib.Contracts.Container.IContainer)translator.FastGetCSObj(L, 1);
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Func<CatLib.Contracts.Container.IContainer, object[], object>>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    string service = LuaAPI.lua_tostring(L, 2);
                    System.Func<CatLib.Contracts.Container.IContainer, object[], object> concrete = translator.GetDelegate<System.Func<CatLib.Contracts.Container.IContainer, object[], object>>(L, 3);
                    bool isStatic = LuaAPI.lua_toboolean(L, 4);
                    
                        CatLib.Contracts.Container.IBindData __cl_gen_ret = __cl_gen_to_be_invoked.Bind( service, concrete, isStatic );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    string service = LuaAPI.lua_tostring(L, 2);
                    string concrete = LuaAPI.lua_tostring(L, 3);
                    bool isStatic = LuaAPI.lua_toboolean(L, 4);
                    
                        CatLib.Contracts.Container.IBindData __cl_gen_ret = __cl_gen_to_be_invoked.Bind( service, concrete, isStatic );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CatLib.Contracts.Container.IContainer.Bind!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Make(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            CatLib.Contracts.Container.IContainer __cl_gen_to_be_invoked = (CatLib.Contracts.Container.IContainer)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string service = LuaAPI.lua_tostring(L, 2);
                    object[] param = translator.GetParams<object>(L, 3);
                    
                        object __cl_gen_ret = __cl_gen_to_be_invoked.Make( service, param );
                        translator.PushAny(L, __cl_gen_ret);
                    
                    
                    
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
            
            
            CatLib.Contracts.Container.IContainer __cl_gen_to_be_invoked = (CatLib.Contracts.Container.IContainer)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string alias = LuaAPI.lua_tostring(L, 2);
                    string service = LuaAPI.lua_tostring(L, 3);
                    
                        CatLib.Contracts.Container.IContainer __cl_gen_ret = __cl_gen_to_be_invoked.Alias( alias, service );
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
            
            
            CatLib.Contracts.Container.IContainer __cl_gen_to_be_invoked = (CatLib.Contracts.Container.IContainer)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    System.Func<CatLib.Contracts.Container.IContainer, CatLib.Contracts.Container.IBindData, object, object> func = translator.GetDelegate<System.Func<CatLib.Contracts.Container.IContainer, CatLib.Contracts.Container.IBindData, object, object>>(L, 2);
                    
                        CatLib.Contracts.Container.IContainer __cl_gen_ret = __cl_gen_to_be_invoked.Decorator( func );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
