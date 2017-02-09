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
    public class UnityEngineAudioClipWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(UnityEngine.AudioClip), L, translator, 0, 4, 8, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadAudioData", _m_LoadAudioData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnloadAudioData", _m_UnloadAudioData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetData", _m_GetData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetData", _m_SetData);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "length", _g_get_length);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "samples", _g_get_samples);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "channels", _g_get_channels);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "frequency", _g_get_frequency);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "loadType", _g_get_loadType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "preloadAudioData", _g_get_preloadAudioData);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "loadState", _g_get_loadState);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "loadInBackground", _g_get_loadInBackground);
            
			
			Utils.EndObjectRegister(typeof(UnityEngine.AudioClip), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(UnityEngine.AudioClip), L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Create", _m_Create_xlua_st_);
            
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(UnityEngine.AudioClip));
			
			
			Utils.EndClassRegister(typeof(UnityEngine.AudioClip), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					UnityEngine.AudioClip __cl_gen_ret = new UnityEngine.AudioClip();
					translator.Push(L, __cl_gen_ret);
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.AudioClip constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAudioData(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            UnityEngine.AudioClip __cl_gen_to_be_invoked = (UnityEngine.AudioClip)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.LoadAudioData(  );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadAudioData(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            UnityEngine.AudioClip __cl_gen_to_be_invoked = (UnityEngine.AudioClip)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.UnloadAudioData(  );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetData(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            UnityEngine.AudioClip __cl_gen_to_be_invoked = (UnityEngine.AudioClip)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    float[] data = (float[])translator.GetObject(L, 2, typeof(float[]));
                    int offsetSamples = LuaAPI.xlua_tointeger(L, 3);
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.GetData( data, offsetSamples );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetData(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            UnityEngine.AudioClip __cl_gen_to_be_invoked = (UnityEngine.AudioClip)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    float[] data = (float[])translator.GetObject(L, 2, typeof(float[]));
                    int offsetSamples = LuaAPI.xlua_tointeger(L, 3);
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.SetData( data, offsetSamples );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Create_xlua_st_(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 5&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    string name = LuaAPI.lua_tostring(L, 1);
                    int lengthSamples = LuaAPI.xlua_tointeger(L, 2);
                    int channels = LuaAPI.xlua_tointeger(L, 3);
                    int frequency = LuaAPI.xlua_tointeger(L, 4);
                    bool stream = LuaAPI.lua_toboolean(L, 5);
                    
                        UnityEngine.AudioClip __cl_gen_ret = UnityEngine.AudioClip.Create( name, lengthSamples, channels, frequency, stream );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 6&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)&& translator.Assignable<UnityEngine.AudioClip.PCMReaderCallback>(L, 6)) 
                {
                    string name = LuaAPI.lua_tostring(L, 1);
                    int lengthSamples = LuaAPI.xlua_tointeger(L, 2);
                    int channels = LuaAPI.xlua_tointeger(L, 3);
                    int frequency = LuaAPI.xlua_tointeger(L, 4);
                    bool stream = LuaAPI.lua_toboolean(L, 5);
                    UnityEngine.AudioClip.PCMReaderCallback pcmreadercallback = translator.GetDelegate<UnityEngine.AudioClip.PCMReaderCallback>(L, 6);
                    
                        UnityEngine.AudioClip __cl_gen_ret = UnityEngine.AudioClip.Create( name, lengthSamples, channels, frequency, stream, pcmreadercallback );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 7&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)&& translator.Assignable<UnityEngine.AudioClip.PCMReaderCallback>(L, 6)&& translator.Assignable<UnityEngine.AudioClip.PCMSetPositionCallback>(L, 7)) 
                {
                    string name = LuaAPI.lua_tostring(L, 1);
                    int lengthSamples = LuaAPI.xlua_tointeger(L, 2);
                    int channels = LuaAPI.xlua_tointeger(L, 3);
                    int frequency = LuaAPI.xlua_tointeger(L, 4);
                    bool stream = LuaAPI.lua_toboolean(L, 5);
                    UnityEngine.AudioClip.PCMReaderCallback pcmreadercallback = translator.GetDelegate<UnityEngine.AudioClip.PCMReaderCallback>(L, 6);
                    UnityEngine.AudioClip.PCMSetPositionCallback pcmsetpositioncallback = translator.GetDelegate<UnityEngine.AudioClip.PCMSetPositionCallback>(L, 7);
                    
                        UnityEngine.AudioClip __cl_gen_ret = UnityEngine.AudioClip.Create( name, lengthSamples, channels, frequency, stream, pcmreadercallback, pcmsetpositioncallback );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.AudioClip.Create!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_length(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                UnityEngine.AudioClip __cl_gen_to_be_invoked = (UnityEngine.AudioClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.length);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_samples(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                UnityEngine.AudioClip __cl_gen_to_be_invoked = (UnityEngine.AudioClip)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.samples);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_channels(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                UnityEngine.AudioClip __cl_gen_to_be_invoked = (UnityEngine.AudioClip)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.channels);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_frequency(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                UnityEngine.AudioClip __cl_gen_to_be_invoked = (UnityEngine.AudioClip)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.frequency);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_loadType(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                UnityEngine.AudioClip __cl_gen_to_be_invoked = (UnityEngine.AudioClip)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.loadType);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_preloadAudioData(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                UnityEngine.AudioClip __cl_gen_to_be_invoked = (UnityEngine.AudioClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.preloadAudioData);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_loadState(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                UnityEngine.AudioClip __cl_gen_to_be_invoked = (UnityEngine.AudioClip)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.loadState);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_loadInBackground(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                UnityEngine.AudioClip __cl_gen_to_be_invoked = (UnityEngine.AudioClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.loadInBackground);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
