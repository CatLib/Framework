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
    public class XLuaTestNoGcWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(XLuaTest.NoGc), L, translator, 0, 5, 5, 5);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FloatParamMethod", _m_FloatParamMethod);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Vector3ParamMethod", _m_Vector3ParamMethod);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StructParamMethod", _m_StructParamMethod);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EnumParamMethod", _m_EnumParamMethod);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DecimalParamMethod", _m_DecimalParamMethod);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "a1", _g_get_a1);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "a2", _g_get_a2);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "a3", _g_get_a3);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "a4", _g_get_a4);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "a5", _g_get_a5);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "a1", _s_set_a1);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "a2", _s_set_a2);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "a3", _s_set_a3);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "a4", _s_set_a4);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "a5", _s_set_a5);
            
			Utils.EndObjectRegister(typeof(XLuaTest.NoGc), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(XLuaTest.NoGc), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(XLuaTest.NoGc));
			
			
			Utils.EndClassRegister(typeof(XLuaTest.NoGc), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					XLuaTest.NoGc __cl_gen_ret = new XLuaTest.NoGc();
					translator.Push(L, __cl_gen_ret);
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to XLuaTest.NoGc constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FloatParamMethod(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            XLuaTest.NoGc __cl_gen_to_be_invoked = (XLuaTest.NoGc)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    float p = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        float __cl_gen_ret = __cl_gen_to_be_invoked.FloatParamMethod( p );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Vector3ParamMethod(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            XLuaTest.NoGc __cl_gen_to_be_invoked = (XLuaTest.NoGc)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    UnityEngine.Vector3 p;translator.Get(L, 2, out p);
                    
                        UnityEngine.Vector3 __cl_gen_ret = __cl_gen_to_be_invoked.Vector3ParamMethod( p );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StructParamMethod(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            XLuaTest.NoGc __cl_gen_to_be_invoked = (XLuaTest.NoGc)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    XLuaTest.MyStruct p;translator.Get(L, 2, out p);
                    
                        XLuaTest.MyStruct __cl_gen_ret = __cl_gen_to_be_invoked.StructParamMethod( p );
                        translator.PushXLuaTestMyStruct(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EnumParamMethod(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            XLuaTest.NoGc __cl_gen_to_be_invoked = (XLuaTest.NoGc)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    XLuaTest.MyEnum p;translator.Get(L, 2, out p);
                    
                        XLuaTest.MyEnum __cl_gen_ret = __cl_gen_to_be_invoked.EnumParamMethod( p );
                        translator.PushXLuaTestMyEnum(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DecimalParamMethod(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            XLuaTest.NoGc __cl_gen_to_be_invoked = (XLuaTest.NoGc)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    decimal p;translator.Get(L, 2, out p);
                    
                        decimal __cl_gen_ret = __cl_gen_to_be_invoked.DecimalParamMethod( p );
                        translator.PushDecimal(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_a1(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                XLuaTest.NoGc __cl_gen_to_be_invoked = (XLuaTest.NoGc)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.a1);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_a2(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                XLuaTest.NoGc __cl_gen_to_be_invoked = (XLuaTest.NoGc)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.a2);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_a3(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                XLuaTest.NoGc __cl_gen_to_be_invoked = (XLuaTest.NoGc)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.a3);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_a4(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                XLuaTest.NoGc __cl_gen_to_be_invoked = (XLuaTest.NoGc)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.a4);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_a5(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                XLuaTest.NoGc __cl_gen_to_be_invoked = (XLuaTest.NoGc)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.a5);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_a1(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                XLuaTest.NoGc __cl_gen_to_be_invoked = (XLuaTest.NoGc)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.a1 = (double[])translator.GetObject(L, 2, typeof(double[]));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_a2(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                XLuaTest.NoGc __cl_gen_to_be_invoked = (XLuaTest.NoGc)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.a2 = (UnityEngine.Vector3[])translator.GetObject(L, 2, typeof(UnityEngine.Vector3[]));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_a3(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                XLuaTest.NoGc __cl_gen_to_be_invoked = (XLuaTest.NoGc)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.a3 = (XLuaTest.MyStruct[])translator.GetObject(L, 2, typeof(XLuaTest.MyStruct[]));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_a4(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                XLuaTest.NoGc __cl_gen_to_be_invoked = (XLuaTest.NoGc)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.a4 = (XLuaTest.MyEnum[])translator.GetObject(L, 2, typeof(XLuaTest.MyEnum[]));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_a5(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                XLuaTest.NoGc __cl_gen_to_be_invoked = (XLuaTest.NoGc)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.a5 = (decimal[])translator.GetObject(L, 2, typeof(decimal[]));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
