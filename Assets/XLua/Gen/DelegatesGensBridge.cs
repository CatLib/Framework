#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using System;


namespace XLua
{
    public partial class DelegateBridge : DelegateBridgeBase
    {
		
		public void SystemVoid()
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                
                
                LuaAPI.lua_getref(L, luaReference);
                
                
                int __gen_error = LuaAPI.lua_pcall(L, 0, 0, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                
                LuaAPI.lua_settop(L, err_func - 1);
                
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public void SystemVoid(UnityEngine.Object obj)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.Push(L, obj);
                
                int __gen_error = LuaAPI.lua_pcall(L, 1, 0, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                
                LuaAPI.lua_settop(L, err_func - 1);
                
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public InvokeLua.ICalc InvokeLuaICalc(int mult, string[] arg)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                LuaAPI.xlua_pushinteger(L, mult);
                for (int __gen_i = 0; __gen_i < arg.Length; ++__gen_i) LuaAPI.lua_pushstring(L, arg[__gen_i]);
                
                int __gen_error = LuaAPI.lua_pcall(L, 1 + arg.Length, 1, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                InvokeLua.ICalc __gen_ret = (InvokeLua.ICalc)translator.GetObject(L, err_func + 1, typeof(InvokeLua.ICalc));
                LuaAPI.lua_settop(L, err_func - 1);
                return  __gen_ret;
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public void SystemVoid(bool obj)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                
                
                LuaAPI.lua_getref(L, luaReference);
                
                LuaAPI.lua_pushboolean(L, obj);
                
                int __gen_error = LuaAPI.lua_pcall(L, 1, 0, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                
                LuaAPI.lua_settop(L, err_func - 1);
                
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public double SystemDouble(double arg1, double arg2)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                
                
                LuaAPI.lua_getref(L, luaReference);
                
                LuaAPI.lua_pushnumber(L, arg1);
                LuaAPI.lua_pushnumber(L, arg2);
                
                int __gen_error = LuaAPI.lua_pcall(L, 2, 1, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                double __gen_ret = LuaAPI.lua_tonumber(L, err_func + 1);
                LuaAPI.lua_settop(L, err_func - 1);
                return  __gen_ret;
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public void SystemVoid(string obj)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                
                
                LuaAPI.lua_getref(L, luaReference);
                
                LuaAPI.lua_pushstring(L, obj);
                
                int __gen_error = LuaAPI.lua_pcall(L, 1, 0, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                
                LuaAPI.lua_settop(L, err_func - 1);
                
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public void SystemVoid(double obj)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                
                
                LuaAPI.lua_getref(L, luaReference);
                
                LuaAPI.lua_pushnumber(L, obj);
                
                int __gen_error = LuaAPI.lua_pcall(L, 1, 0, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                
                LuaAPI.lua_settop(L, err_func - 1);
                
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public int SystemInt32(int a, string b, out CSCallLua.DClass c)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                LuaAPI.xlua_pushinteger(L, a);
                LuaAPI.lua_pushstring(L, b);
                
                int __gen_error = LuaAPI.lua_pcall(L, 2, 2, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                c = (CSCallLua.DClass)translator.GetObject(L, err_func + 2, typeof(CSCallLua.DClass));
                
                int __gen_ret = LuaAPI.xlua_tointeger(L, err_func + 1);
                LuaAPI.lua_settop(L, err_func - 1);
                return  __gen_ret;
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public System.Action SystemAction()
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                
                int __gen_error = LuaAPI.lua_pcall(L, 0, 1, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                System.Action __gen_ret = translator.GetDelegate<System.Action>(L, err_func + 1);
                LuaAPI.lua_settop(L, err_func - 1);
                return  __gen_ret;
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public int SystemInt32(int p)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                
                
                LuaAPI.lua_getref(L, luaReference);
                
                LuaAPI.xlua_pushinteger(L, p);
                
                int __gen_error = LuaAPI.lua_pcall(L, 1, 1, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                int __gen_ret = LuaAPI.xlua_tointeger(L, err_func + 1);
                LuaAPI.lua_settop(L, err_func - 1);
                return  __gen_ret;
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public UnityEngine.Vector3 UnityEngineVector3(UnityEngine.Vector3 p)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.PushUnityEngineVector3(L, p);
                
                int __gen_error = LuaAPI.lua_pcall(L, 1, 1, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                UnityEngine.Vector3 __gen_ret;translator.Get(L, err_func + 1, out __gen_ret);
                LuaAPI.lua_settop(L, err_func - 1);
                return  __gen_ret;
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public XLuaTest.MyStruct XLuaTestMyStruct(XLuaTest.MyStruct p)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.PushXLuaTestMyStruct(L, p);
                
                int __gen_error = LuaAPI.lua_pcall(L, 1, 1, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                XLuaTest.MyStruct __gen_ret;translator.Get(L, err_func + 1, out __gen_ret);
                LuaAPI.lua_settop(L, err_func - 1);
                return  __gen_ret;
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public XLuaTest.MyEnum XLuaTestMyEnum(XLuaTest.MyEnum p)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.PushXLuaTestMyEnum(L, p);
                
                int __gen_error = LuaAPI.lua_pcall(L, 1, 1, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                XLuaTest.MyEnum __gen_ret;translator.Get(L, err_func + 1, out __gen_ret);
                LuaAPI.lua_settop(L, err_func - 1);
                return  __gen_ret;
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public decimal SystemDecimal(decimal p)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.PushDecimal(L, p);
                
                int __gen_error = LuaAPI.lua_pcall(L, 1, 1, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                decimal __gen_ret;translator.Get(L, err_func + 1, out __gen_ret);
                LuaAPI.lua_settop(L, err_func - 1);
                return  __gen_ret;
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public void SystemVoid(System.Array arr)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.Push(L, arr);
                
                int __gen_error = LuaAPI.lua_pcall(L, 1, 0, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                
                LuaAPI.lua_settop(L, err_func - 1);
                
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public int SystemInt32(HotfixCalc calc, int a, out double b, ref string c)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.Push(L, calc);
                LuaAPI.xlua_pushinteger(L, a);
                LuaAPI.lua_pushstring(L, c);
                
                int __gen_error = LuaAPI.lua_pcall(L, 3, 3, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                b = LuaAPI.lua_tonumber(L, err_func + 2);
                c = LuaAPI.lua_tostring(L, err_func + 3);
                
                int __gen_ret = LuaAPI.xlua_tointeger(L, err_func + 1);
                LuaAPI.lua_settop(L, err_func - 1);
                return  __gen_ret;
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public void SystemVoid(object self)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.PushAny(L, self);
                
                int __gen_error = LuaAPI.lua_pcall(L, 1, 0, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                
                LuaAPI.lua_settop(L, err_func - 1);
                
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public int SystemInt32(object self, int a, int b)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.PushAny(L, self);
                LuaAPI.xlua_pushinteger(L, a);
                LuaAPI.xlua_pushinteger(L, b);
                
                int __gen_error = LuaAPI.lua_pcall(L, 3, 1, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                int __gen_ret = LuaAPI.xlua_tointeger(L, err_func + 1);
                LuaAPI.lua_settop(L, err_func - 1);
                return  __gen_ret;
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public UnityEngine.Vector3 UnityEngineVector3(object self, UnityEngine.Vector3 a, UnityEngine.Vector3 b)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.PushAny(L, self);
                translator.PushUnityEngineVector3(L, a);
                translator.PushUnityEngineVector3(L, b);
                
                int __gen_error = LuaAPI.lua_pcall(L, 3, 1, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                UnityEngine.Vector3 __gen_ret;translator.Get(L, err_func + 1, out __gen_ret);
                LuaAPI.lua_settop(L, err_func - 1);
                return  __gen_ret;
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public int SystemInt32(object self, int a, out double b, ref string c)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.PushAny(L, self);
                LuaAPI.xlua_pushinteger(L, a);
                LuaAPI.lua_pushstring(L, c);
                
                int __gen_error = LuaAPI.lua_pcall(L, 3, 3, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                b = LuaAPI.lua_tonumber(L, err_func + 2);
                c = LuaAPI.lua_tostring(L, err_func + 3);
                
                int __gen_ret = LuaAPI.xlua_tointeger(L, err_func + 1);
                LuaAPI.lua_settop(L, err_func - 1);
                return  __gen_ret;
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public int SystemInt32(object self, int a, out double b, ref string c, object go)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.PushAny(L, self);
                LuaAPI.xlua_pushinteger(L, a);
                LuaAPI.lua_pushstring(L, c);
                translator.PushAny(L, go);
                
                int __gen_error = LuaAPI.lua_pcall(L, 4, 3, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                b = LuaAPI.lua_tonumber(L, err_func + 2);
                c = LuaAPI.lua_tostring(L, err_func + 3);
                
                int __gen_ret = LuaAPI.xlua_tointeger(L, err_func + 1);
                LuaAPI.lua_settop(L, err_func - 1);
                return  __gen_ret;
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public UnityEngine.GameObject UnityEngineGameObject(StructTest self, int a, object b)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.Push(L, self);
                LuaAPI.xlua_pushinteger(L, a);
                translator.PushAny(L, b);
                
                int __gen_error = LuaAPI.lua_pcall(L, 3, 1, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                UnityEngine.GameObject __gen_ret = (UnityEngine.GameObject)translator.GetObject(L, err_func + 1, typeof(UnityEngine.GameObject));
                LuaAPI.lua_settop(L, err_func - 1);
                return  __gen_ret;
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public void SystemVoid(StructTest self, object go)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.Push(L, self);
                translator.PushAny(L, go);
                
                int __gen_error = LuaAPI.lua_pcall(L, 2, 0, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                
                LuaAPI.lua_settop(L, err_func - 1);
                
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public void SystemVoid(XLua.LuaTable self, object value)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.Push(L, self);
                translator.PushAny(L, value);
                
                int __gen_error = LuaAPI.lua_pcall(L, 2, 0, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                
                LuaAPI.lua_settop(L, err_func - 1);
                
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public int SystemInt32(XLua.LuaTable self)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.Push(L, self);
                
                int __gen_error = LuaAPI.lua_pcall(L, 1, 1, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                int __gen_ret = LuaAPI.xlua_tointeger(L, err_func + 1);
                LuaAPI.lua_settop(L, err_func - 1);
                return  __gen_ret;
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public void SystemVoid(XLua.LuaTable self, int value)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.Push(L, self);
                LuaAPI.xlua_pushinteger(L, value);
                
                int __gen_error = LuaAPI.lua_pcall(L, 2, 0, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                
                LuaAPI.lua_settop(L, err_func - 1);
                
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public int SystemInt32(XLua.LuaTable self, object field)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.Push(L, self);
                translator.PushAny(L, field);
                
                int __gen_error = LuaAPI.lua_pcall(L, 2, 1, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                int __gen_ret = LuaAPI.xlua_tointeger(L, err_func + 1);
                LuaAPI.lua_settop(L, err_func - 1);
                return  __gen_ret;
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public void SystemVoid(XLua.LuaTable self, object field, int value)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.Push(L, self);
                translator.PushAny(L, field);
                LuaAPI.xlua_pushinteger(L, value);
                
                int __gen_error = LuaAPI.lua_pcall(L, 3, 0, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                
                LuaAPI.lua_settop(L, err_func - 1);
                
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public void SystemVoid(XLua.LuaTable self)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.Push(L, self);
                
                int __gen_error = LuaAPI.lua_pcall(L, 1, 0, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                
                LuaAPI.lua_settop(L, err_func - 1);
                
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public void SystemVoid(int a, int b)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                
                
                LuaAPI.lua_getref(L, luaReference);
                
                LuaAPI.xlua_pushinteger(L, a);
                LuaAPI.xlua_pushinteger(L, b);
                
                int __gen_error = LuaAPI.lua_pcall(L, 2, 0, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                
                LuaAPI.lua_settop(L, err_func - 1);
                
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public void SystemVoid(object a, int b, int c)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.PushAny(L, a);
                LuaAPI.xlua_pushinteger(L, b);
                LuaAPI.xlua_pushinteger(L, c);
                
                int __gen_error = LuaAPI.lua_pcall(L, 3, 0, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                
                LuaAPI.lua_settop(L, err_func - 1);
                
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
		public XLua.LuaTable XLuaLuaTable(object self)
		{
#if THREAD_SAFT || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.L;
                int err_func =LuaAPI.load_error_func(L, errorFuncRef);
                ObjectTranslator translator = luaEnv.translator;
                
                LuaAPI.lua_getref(L, luaReference);
                
                translator.PushAny(L, self);
                
                int __gen_error = LuaAPI.lua_pcall(L, 1, 1, err_func);
                if (__gen_error != 0)
                    luaEnv.ThrowExceptionFromError(err_func - 1);
                
                
                XLua.LuaTable __gen_ret = (XLua.LuaTable)translator.GetObject(L, err_func + 1, typeof(XLua.LuaTable));
                LuaAPI.lua_settop(L, err_func - 1);
                return  __gen_ret;
#if THREAD_SAFT || HOTFIX_ENABLE
            }
#endif
		}
        
        
		static DelegateBridge()
		{
		    Gen_Flag = true;
		}
		
		public override Delegate GetDelegateByType(Type type)
		{
		
		    if (type == typeof(System.Action))
			{
			    return new System.Action(SystemVoid);
			}
		
		    if (type == typeof(System.Action<UnityEngine.Object>))
			{
			    return new System.Action<UnityEngine.Object>(SystemVoid);
			}
		
		    if (type == typeof(InvokeLua.CalcNew))
			{
			    return new InvokeLua.CalcNew(InvokeLuaICalc);
			}
		
		    if (type == typeof(System.Action<bool>))
			{
			    return new System.Action<bool>(SystemVoid);
			}
		
		    if (type == typeof(UnityEngine.Events.UnityAction))
			{
			    return new UnityEngine.Events.UnityAction(SystemVoid);
			}
		
		    if (type == typeof(System.Func<double, double, double>))
			{
			    return new System.Func<double, double, double>(SystemDouble);
			}
		
		    if (type == typeof(System.Action<string>))
			{
			    return new System.Action<string>(SystemVoid);
			}
		
		    if (type == typeof(System.Action<double>))
			{
			    return new System.Action<double>(SystemVoid);
			}
		
		    if (type == typeof(CSCallLua.FDelegate))
			{
			    return new CSCallLua.FDelegate(SystemInt32);
			}
		
		    if (type == typeof(CSCallLua.GetE))
			{
			    return new CSCallLua.GetE(SystemAction);
			}
		
		    if (type == typeof(XLuaTest.IntParam))
			{
			    return new XLuaTest.IntParam(SystemInt32);
			}
		
		    if (type == typeof(XLuaTest.Vector3Param))
			{
			    return new XLuaTest.Vector3Param(UnityEngineVector3);
			}
		
		    if (type == typeof(XLuaTest.CustomValueTypeParam))
			{
			    return new XLuaTest.CustomValueTypeParam(XLuaTestMyStruct);
			}
		
		    if (type == typeof(XLuaTest.EnumParam))
			{
			    return new XLuaTest.EnumParam(XLuaTestMyEnum);
			}
		
		    if (type == typeof(XLuaTest.DecimalParam))
			{
			    return new XLuaTest.DecimalParam(SystemDecimal);
			}
		
		    if (type == typeof(XLuaTest.ArrayAccess))
			{
			    return new XLuaTest.ArrayAccess(SystemVoid);
			}
		
		    if (type == typeof(TestOutDelegate))
			{
			    return new TestOutDelegate(SystemInt32);
			}
		
        
            if (type == typeof(__Gen_Hotfix_Delegate0))
			{
                return new __Gen_Hotfix_Delegate0(SystemVoid);
            }
        
            if (type == typeof(__Gen_Hotfix_Delegate1))
			{
                return new __Gen_Hotfix_Delegate1(SystemInt32);
            }
        
            if (type == typeof(__Gen_Hotfix_Delegate2))
			{
                return new __Gen_Hotfix_Delegate2(UnityEngineVector3);
            }
        
            if (type == typeof(__Gen_Hotfix_Delegate3))
			{
                return new __Gen_Hotfix_Delegate3(SystemInt32);
            }
        
            if (type == typeof(__Gen_Hotfix_Delegate4))
			{
                return new __Gen_Hotfix_Delegate4(SystemInt32);
            }
        
            if (type == typeof(__Gen_Hotfix_Delegate5))
			{
                return new __Gen_Hotfix_Delegate5(UnityEngineGameObject);
            }
        
            if (type == typeof(__Gen_Hotfix_Delegate6))
			{
                return new __Gen_Hotfix_Delegate6(SystemVoid);
            }
        
            if (type == typeof(__Gen_Hotfix_Delegate7))
			{
                return new __Gen_Hotfix_Delegate7(SystemVoid);
            }
        
            if (type == typeof(__Gen_Hotfix_Delegate8))
			{
                return new __Gen_Hotfix_Delegate8(SystemInt32);
            }
        
            if (type == typeof(__Gen_Hotfix_Delegate9))
			{
                return new __Gen_Hotfix_Delegate9(SystemVoid);
            }
        
            if (type == typeof(__Gen_Hotfix_Delegate10))
			{
                return new __Gen_Hotfix_Delegate10(SystemInt32);
            }
        
            if (type == typeof(__Gen_Hotfix_Delegate11))
			{
                return new __Gen_Hotfix_Delegate11(SystemVoid);
            }
        
            if (type == typeof(__Gen_Hotfix_Delegate12))
			{
                return new __Gen_Hotfix_Delegate12(SystemVoid);
            }
        
            if (type == typeof(__Gen_Hotfix_Delegate13))
			{
                return new __Gen_Hotfix_Delegate13(SystemVoid);
            }
        
            if (type == typeof(__Gen_Hotfix_Delegate14))
			{
                return new __Gen_Hotfix_Delegate14(SystemVoid);
            }
        
            if (type == typeof(__Gen_Hotfix_Delegate15))
			{
                return new __Gen_Hotfix_Delegate15(XLuaLuaTable);
            }
        
		    throw new InvalidCastException("This delegate must add to CSharpCallLua: " + type);
		}
	}
    
    
    [HotfixDelegate]
    public delegate void __Gen_Hotfix_Delegate0(object p0);
    
    [HotfixDelegate]
    public delegate int __Gen_Hotfix_Delegate1(object p0, int p1, int p2);
    
    [HotfixDelegate]
    public delegate UnityEngine.Vector3 __Gen_Hotfix_Delegate2(object p0, UnityEngine.Vector3 p1, UnityEngine.Vector3 p2);
    
    [HotfixDelegate]
    public delegate int __Gen_Hotfix_Delegate3(object p0, int p1, out double p2, ref string p3);
    
    [HotfixDelegate]
    public delegate int __Gen_Hotfix_Delegate4(object p0, int p1, out double p2, ref string p3, object p4);
    
    [HotfixDelegate]
    public delegate UnityEngine.GameObject __Gen_Hotfix_Delegate5(StructTest p0, int p1, object p2);
    
    [HotfixDelegate]
    public delegate void __Gen_Hotfix_Delegate6(StructTest p0, object p1);
    
    [HotfixDelegate]
    public delegate void __Gen_Hotfix_Delegate7(XLua.LuaTable p0, object p1);
    
    [HotfixDelegate]
    public delegate int __Gen_Hotfix_Delegate8(XLua.LuaTable p0);
    
    [HotfixDelegate]
    public delegate void __Gen_Hotfix_Delegate9(XLua.LuaTable p0, int p1);
    
    [HotfixDelegate]
    public delegate int __Gen_Hotfix_Delegate10(XLua.LuaTable p0, object p1);
    
    [HotfixDelegate]
    public delegate void __Gen_Hotfix_Delegate11(XLua.LuaTable p0, object p1, int p2);
    
    [HotfixDelegate]
    public delegate void __Gen_Hotfix_Delegate12(XLua.LuaTable p0);
    
    [HotfixDelegate]
    public delegate void __Gen_Hotfix_Delegate13(int p0, int p1);
    
    [HotfixDelegate]
    public delegate void __Gen_Hotfix_Delegate14(object p0, int p1, int p2);
    
    [HotfixDelegate]
    public delegate XLua.LuaTable __Gen_Hotfix_Delegate15(object p0);
    
}