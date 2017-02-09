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
    public class UnityEngineInputWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(UnityEngine.Input), L, translator, 0, 0, 0, 0);
			
			
			
			
			
			Utils.EndObjectRegister(typeof(UnityEngine.Input), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(UnityEngine.Input), L, __CreateInstance, 16, 26, 6);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetAxis", _m_GetAxis_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetAxisRaw", _m_GetAxisRaw_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetButton", _m_GetButton_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetButtonDown", _m_GetButtonDown_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetButtonUp", _m_GetButtonUp_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetKey", _m_GetKey_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetKeyDown", _m_GetKeyDown_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetKeyUp", _m_GetKeyUp_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetJoystickNames", _m_GetJoystickNames_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMouseButton", _m_GetMouseButton_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMouseButtonDown", _m_GetMouseButtonDown_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMouseButtonUp", _m_GetMouseButtonUp_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ResetInputAxes", _m_ResetInputAxes_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetAccelerationEvent", _m_GetAccelerationEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetTouch", _m_GetTouch_xlua_st_);
            
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(UnityEngine.Input));
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "compensateSensors", _g_get_compensateSensors);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "gyro", _g_get_gyro);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "mousePosition", _g_get_mousePosition);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "mouseScrollDelta", _g_get_mouseScrollDelta);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "mousePresent", _g_get_mousePresent);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "simulateMouseWithTouches", _g_get_simulateMouseWithTouches);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "anyKey", _g_get_anyKey);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "anyKeyDown", _g_get_anyKeyDown);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "inputString", _g_get_inputString);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "acceleration", _g_get_acceleration);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "accelerationEvents", _g_get_accelerationEvents);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "accelerationEventCount", _g_get_accelerationEventCount);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "touches", _g_get_touches);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "touchCount", _g_get_touchCount);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "touchPressureSupported", _g_get_touchPressureSupported);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "stylusTouchSupported", _g_get_stylusTouchSupported);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "touchSupported", _g_get_touchSupported);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "multiTouchEnabled", _g_get_multiTouchEnabled);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "location", _g_get_location);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "compass", _g_get_compass);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "deviceOrientation", _g_get_deviceOrientation);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "imeCompositionMode", _g_get_imeCompositionMode);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "compositionString", _g_get_compositionString);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "imeIsSelected", _g_get_imeIsSelected);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "compositionCursorPos", _g_get_compositionCursorPos);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "backButtonLeavesApp", _g_get_backButtonLeavesApp);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "compensateSensors", _s_set_compensateSensors);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "simulateMouseWithTouches", _s_set_simulateMouseWithTouches);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "multiTouchEnabled", _s_set_multiTouchEnabled);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "imeCompositionMode", _s_set_imeCompositionMode);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "compositionCursorPos", _s_set_compositionCursorPos);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "backButtonLeavesApp", _s_set_backButtonLeavesApp);
            
			Utils.EndClassRegister(typeof(UnityEngine.Input), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					UnityEngine.Input __cl_gen_ret = new UnityEngine.Input();
					translator.Push(L, __cl_gen_ret);
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Input constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAxis_xlua_st_(RealStatePtr L)
        {
            
            
            
            try {
                
                {
                    string axisName = LuaAPI.lua_tostring(L, 1);
                    
                        float __cl_gen_ret = UnityEngine.Input.GetAxis( axisName );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAxisRaw_xlua_st_(RealStatePtr L)
        {
            
            
            
            try {
                
                {
                    string axisName = LuaAPI.lua_tostring(L, 1);
                    
                        float __cl_gen_ret = UnityEngine.Input.GetAxisRaw( axisName );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetButton_xlua_st_(RealStatePtr L)
        {
            
            
            
            try {
                
                {
                    string buttonName = LuaAPI.lua_tostring(L, 1);
                    
                        bool __cl_gen_ret = UnityEngine.Input.GetButton( buttonName );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetButtonDown_xlua_st_(RealStatePtr L)
        {
            
            
            
            try {
                
                {
                    string buttonName = LuaAPI.lua_tostring(L, 1);
                    
                        bool __cl_gen_ret = UnityEngine.Input.GetButtonDown( buttonName );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetButtonUp_xlua_st_(RealStatePtr L)
        {
            
            
            
            try {
                
                {
                    string buttonName = LuaAPI.lua_tostring(L, 1);
                    
                        bool __cl_gen_ret = UnityEngine.Input.GetButtonUp( buttonName );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetKey_xlua_st_(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string name = LuaAPI.lua_tostring(L, 1);
                    
                        bool __cl_gen_ret = UnityEngine.Input.GetKey( name );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 1&& translator.Assignable<UnityEngine.KeyCode>(L, 1)) 
                {
                    UnityEngine.KeyCode key;translator.Get(L, 1, out key);
                    
                        bool __cl_gen_ret = UnityEngine.Input.GetKey( key );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Input.GetKey!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetKeyDown_xlua_st_(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string name = LuaAPI.lua_tostring(L, 1);
                    
                        bool __cl_gen_ret = UnityEngine.Input.GetKeyDown( name );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 1&& translator.Assignable<UnityEngine.KeyCode>(L, 1)) 
                {
                    UnityEngine.KeyCode key;translator.Get(L, 1, out key);
                    
                        bool __cl_gen_ret = UnityEngine.Input.GetKeyDown( key );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Input.GetKeyDown!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetKeyUp_xlua_st_(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string name = LuaAPI.lua_tostring(L, 1);
                    
                        bool __cl_gen_ret = UnityEngine.Input.GetKeyUp( name );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 1&& translator.Assignable<UnityEngine.KeyCode>(L, 1)) 
                {
                    UnityEngine.KeyCode key;translator.Get(L, 1, out key);
                    
                        bool __cl_gen_ret = UnityEngine.Input.GetKeyUp( key );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Input.GetKeyUp!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetJoystickNames_xlua_st_(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
            try {
                
                {
                    
                        string[] __cl_gen_ret = UnityEngine.Input.GetJoystickNames(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMouseButton_xlua_st_(RealStatePtr L)
        {
            
            
            
            try {
                
                {
                    int button = LuaAPI.xlua_tointeger(L, 1);
                    
                        bool __cl_gen_ret = UnityEngine.Input.GetMouseButton( button );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMouseButtonDown_xlua_st_(RealStatePtr L)
        {
            
            
            
            try {
                
                {
                    int button = LuaAPI.xlua_tointeger(L, 1);
                    
                        bool __cl_gen_ret = UnityEngine.Input.GetMouseButtonDown( button );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMouseButtonUp_xlua_st_(RealStatePtr L)
        {
            
            
            
            try {
                
                {
                    int button = LuaAPI.xlua_tointeger(L, 1);
                    
                        bool __cl_gen_ret = UnityEngine.Input.GetMouseButtonUp( button );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetInputAxes_xlua_st_(RealStatePtr L)
        {
            
            
            
            try {
                
                {
                    
                    UnityEngine.Input.ResetInputAxes(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAccelerationEvent_xlua_st_(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
            try {
                
                {
                    int index = LuaAPI.xlua_tointeger(L, 1);
                    
                        UnityEngine.AccelerationEvent __cl_gen_ret = UnityEngine.Input.GetAccelerationEvent( index );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTouch_xlua_st_(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
            try {
                
                {
                    int index = LuaAPI.xlua_tointeger(L, 1);
                    
                        UnityEngine.Touch __cl_gen_ret = UnityEngine.Input.GetTouch( index );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_compensateSensors(RealStatePtr L)
        {
            
            try {
			    LuaAPI.lua_pushboolean(L, UnityEngine.Input.compensateSensors);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_gyro(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.Push(L, UnityEngine.Input.gyro);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mousePosition(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.PushUnityEngineVector3(L, UnityEngine.Input.mousePosition);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mouseScrollDelta(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.PushUnityEngineVector2(L, UnityEngine.Input.mouseScrollDelta);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mousePresent(RealStatePtr L)
        {
            
            try {
			    LuaAPI.lua_pushboolean(L, UnityEngine.Input.mousePresent);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_simulateMouseWithTouches(RealStatePtr L)
        {
            
            try {
			    LuaAPI.lua_pushboolean(L, UnityEngine.Input.simulateMouseWithTouches);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_anyKey(RealStatePtr L)
        {
            
            try {
			    LuaAPI.lua_pushboolean(L, UnityEngine.Input.anyKey);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_anyKeyDown(RealStatePtr L)
        {
            
            try {
			    LuaAPI.lua_pushboolean(L, UnityEngine.Input.anyKeyDown);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_inputString(RealStatePtr L)
        {
            
            try {
			    LuaAPI.lua_pushstring(L, UnityEngine.Input.inputString);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_acceleration(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.PushUnityEngineVector3(L, UnityEngine.Input.acceleration);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_accelerationEvents(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.Push(L, UnityEngine.Input.accelerationEvents);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_accelerationEventCount(RealStatePtr L)
        {
            
            try {
			    LuaAPI.xlua_pushinteger(L, UnityEngine.Input.accelerationEventCount);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_touches(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.Push(L, UnityEngine.Input.touches);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_touchCount(RealStatePtr L)
        {
            
            try {
			    LuaAPI.xlua_pushinteger(L, UnityEngine.Input.touchCount);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_touchPressureSupported(RealStatePtr L)
        {
            
            try {
			    LuaAPI.lua_pushboolean(L, UnityEngine.Input.touchPressureSupported);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_stylusTouchSupported(RealStatePtr L)
        {
            
            try {
			    LuaAPI.lua_pushboolean(L, UnityEngine.Input.stylusTouchSupported);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_touchSupported(RealStatePtr L)
        {
            
            try {
			    LuaAPI.lua_pushboolean(L, UnityEngine.Input.touchSupported);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_multiTouchEnabled(RealStatePtr L)
        {
            
            try {
			    LuaAPI.lua_pushboolean(L, UnityEngine.Input.multiTouchEnabled);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_location(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.Push(L, UnityEngine.Input.location);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_compass(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.Push(L, UnityEngine.Input.compass);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_deviceOrientation(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.Push(L, UnityEngine.Input.deviceOrientation);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_imeCompositionMode(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.Push(L, UnityEngine.Input.imeCompositionMode);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_compositionString(RealStatePtr L)
        {
            
            try {
			    LuaAPI.lua_pushstring(L, UnityEngine.Input.compositionString);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_imeIsSelected(RealStatePtr L)
        {
            
            try {
			    LuaAPI.lua_pushboolean(L, UnityEngine.Input.imeIsSelected);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_compositionCursorPos(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.PushUnityEngineVector2(L, UnityEngine.Input.compositionCursorPos);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_backButtonLeavesApp(RealStatePtr L)
        {
            
            try {
			    LuaAPI.lua_pushboolean(L, UnityEngine.Input.backButtonLeavesApp);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_compensateSensors(RealStatePtr L)
        {
            
            try {
			    UnityEngine.Input.compensateSensors = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_simulateMouseWithTouches(RealStatePtr L)
        {
            
            try {
			    UnityEngine.Input.simulateMouseWithTouches = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_multiTouchEnabled(RealStatePtr L)
        {
            
            try {
			    UnityEngine.Input.multiTouchEnabled = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_imeCompositionMode(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			UnityEngine.IMECompositionMode __cl_gen_value;translator.Get(L, 1, out __cl_gen_value);
				UnityEngine.Input.imeCompositionMode = __cl_gen_value;
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_compositionCursorPos(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			UnityEngine.Vector2 __cl_gen_value;translator.Get(L, 1, out __cl_gen_value);
				UnityEngine.Input.compositionCursorPos = __cl_gen_value;
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_backButtonLeavesApp(RealStatePtr L)
        {
            
            try {
			    UnityEngine.Input.backButtonLeavesApp = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
