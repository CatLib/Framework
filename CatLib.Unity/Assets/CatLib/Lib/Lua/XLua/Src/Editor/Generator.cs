﻿/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

#if !XLUA_GENERAL
using UnityEngine;
using UnityEditor;
#endif
using System.Collections.Generic;
using System.IO;
using XLua;
using System;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using Utils = XLua.Utils;

namespace CSObjectWrapEditor
{
    public static class GeneratorConfig
    {
#if XLUA_GENERAL
        public static string common_path = "./Gen/";
#else
        public static string common_path = Application.dataPath + "/XLua/Gen/";
#endif

        static GeneratorConfig()
        {
            foreach(var type in (from type in Utils.GetAllTypes()
            where type.IsAbstract && type.IsSealed
            select type))
            {
                foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                {
                    if (field.FieldType == typeof(string) && field.IsDefined(typeof(GenPathAttribute), false))
                    {
                        common_path = field.GetValue(null) as string;
                        if (!common_path.EndsWith("/"))
                        {
                            common_path = common_path + "/";
                        }
                    }
                }

                foreach (var prop in type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                {
                    if (prop.PropertyType == typeof(string) && prop.IsDefined(typeof(GenPathAttribute), false))
                    {
                        common_path = prop.GetValue(null, null) as string;
                        if (!common_path.EndsWith("/"))
                        {
                            common_path = common_path + "/";
                        }
                    }
                }
            }
        }
    }

    public struct CustomGenTask
    {
        public LuaTable Data;
        public TextWriter Output;
    }

    public struct UserConfig
    {
        public IEnumerable<Type> LuaCallCSharp;
        public IEnumerable<Type> CSharpCallLua;
        public IEnumerable<Type> ReflectionUse;
    }

    public class GenCodeMenuAttribute : Attribute
    {

    }

    public class GenPathAttribute : Attribute
    {

    }

    public struct XLuaTemplate
    {
        public string name;
        public string text;
    }

    public struct XLuaTemplates
    {
        public XLuaTemplate LuaClassWrap;
        public XLuaTemplate LuaDelegateBridge;
        public XLuaTemplate LuaDelegateWrap;
        public XLuaTemplate LuaEnumWrap;
        public XLuaTemplate LuaInterfaceBridge;
        public XLuaTemplate LuaRegister;
        public XLuaTemplate LuaWrapPusher;
        public XLuaTemplate PackUnpack;
        public XLuaTemplate TemplateCommon;
    }

    public static class Generator
    {
        static LuaEnv luaenv = new LuaEnv();
        static List<string> OpMethodNames = new List<string>() { "op_Addition", "op_Subtraction", "op_Multiply", "op_Division", "op_Equality", "op_UnaryNegation", "op_LessThan", "op_LessThanOrEqual", "op_Modulus" };
        private static XLuaTemplates templateRef;

        static Generator()
        {
#if !XLUA_GENERAL
            TemplateRef template_ref = ScriptableObject.CreateInstance<TemplateRef>();

            templateRef = new XLuaTemplates()
            {
                LuaClassWrap = { name = template_ref.LuaClassWrap.name, text = template_ref.LuaClassWrap.text },
                LuaDelegateBridge = { name = template_ref.LuaDelegateBridge.name, text = template_ref.LuaDelegateBridge.text },
                LuaDelegateWrap = { name = template_ref.LuaDelegateWrap.name, text = template_ref.LuaDelegateWrap.text },
                LuaEnumWrap = { name = template_ref.LuaEnumWrap.name, text = template_ref.LuaEnumWrap.text },
                LuaInterfaceBridge = { name = template_ref.LuaInterfaceBridge.name, text = template_ref.LuaInterfaceBridge.text },
                LuaRegister = { name = template_ref.LuaRegister.name, text = template_ref.LuaRegister.text },
                LuaWrapPusher = { name = template_ref.LuaWrapPusher.name, text = template_ref.LuaWrapPusher.text },
                PackUnpack = { name = template_ref.PackUnpack.name, text = template_ref.PackUnpack.text },
                TemplateCommon = { name = template_ref.TemplateCommon.name, text = template_ref.TemplateCommon.text },
            };
#endif
            luaenv.AddLoader((ref string filepath) =>
            {
                if (filepath == "TemplateCommon")
                {
                    return Encoding.UTF8.GetBytes(templateRef.TemplateCommon.text);
                }
                else
                {
                    return null;
                }
            });
        }

        static int OverloadCosting(MethodBase mi)
        {
            int costing = 0;

            if (!mi.IsStatic)
            {
                costing++;
            }

            foreach (var paraminfo in mi.GetParameters())
            {
                if ((!paraminfo.ParameterType.IsPrimitive ) && (paraminfo.IsIn || !paraminfo.IsOut))
                {
                    costing++;
                }
            }
            costing = costing * 10000 + (mi.GetParameters().Length + (mi.IsStatic ? 0 : 1));
            return costing;
        }

        static IEnumerable<Type> type_has_extension_methods = null;

        static IEnumerable<MethodInfo> GetExtensionMethods(Type extendedType)
        {
            if (type_has_extension_methods == null)
            {
                var gen_types = LuaCallCSharp;

                type_has_extension_methods = from type in gen_types
                                             where type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                                                    .Any(method => Utils.IsSupportedExtensionMethod(method))
                                             select type;
            }
            return from type in type_has_extension_methods
                   where type.IsSealed && !type.IsGenericType && !type.IsNested
                        from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                        where IsSupportedExtensionMethod(method, extendedType)
                        select method;
        }

        public static bool IsSupportedExtensionMethod(MethodBase method, Type extendedType)
        {
            if (!method.IsDefined(typeof(ExtensionAttribute), false))
                return false;
            var methodParameters = method.GetParameters();
            if (methodParameters.Length < 1)
                return false;

            var hasValidGenericParameter = false;
            for (var i = 0; i < methodParameters.Length; i++)
            {
                var parameterType = methodParameters[i].ParameterType;
                if (i == 0)
                {
                    if (parameterType.IsGenericParameter)
                    {
                        var parameterConstraints = parameterType.GetGenericParameterConstraints();
                        if (parameterConstraints.Length == 0 || !parameterConstraints[0].IsAssignableFrom(extendedType))
                            return false;
                        hasValidGenericParameter = true;
                    }
                    else if (parameterType != extendedType)
                        return false;
                }
                else if (parameterType.IsGenericParameter)
                {
                    var parameterConstraints = parameterType.GetGenericParameterConstraints();
                    if (parameterConstraints.Length == 0 || !parameterConstraints[0].IsClass())
                        return false;
                    hasValidGenericParameter = true;
                }
            }
            return hasValidGenericParameter || !method.ContainsGenericParameters;
        }

        static void getClassInfo(Type type, LuaTable parameters)
        {
            parameters.Set("type", type);

            var constructors = new List<MethodBase>();
            var constructor_def_vals = new List<int>();
            if (!type.IsAbstract)
            {
                foreach (var con in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase).Cast<MethodBase>()
                    .Where(constructor => !isObsolete(constructor)))
                {
                    int def_count = 0;
                    constructors.Add(con);
                    constructor_def_vals.Add(def_count);

                    var ps = con.GetParameters();
                    for (int i = ps.Length - 1; i >= 0; i--)
                    {
                        if (ps[i].IsOptional ||
                            (ps[i].IsDefined(typeof(ParamArrayAttribute), false) && i > 0 && ps[i - 1].IsOptional))
                        {
                            def_count++;
                            constructors.Add(con);
                            constructor_def_vals.Add(def_count);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            parameters.Set("constructors", constructors);
            parameters.Set("constructor_def_vals", constructor_def_vals);

            var getters = type.GetProperties().Where(prop => prop.CanRead);
            var setters = type.GetProperties().Where(prop => prop.CanWrite);

            var methodNames = type.GetMethods(BindingFlags.Public | BindingFlags.Instance
                | BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly).Select(t=>t.Name).Distinct().ToDictionary(t=>t);
            foreach (var getter in getters)
            {
                methodNames.Remove("get_" + getter.Name);
            }

            foreach (var setter in setters)
            {
                methodNames.Remove("set_" + setter.Name);
            }
            List<string> extension_methods_namespace = new List<string>();
            var extension_methods = GetExtensionMethods(type).ToArray();
            foreach(var extension_method in extension_methods)
            {
                if (extension_method.DeclaringType.Namespace != null
                    && extension_method.DeclaringType.Namespace != "System.Collections.Generic"
                    && extension_method.DeclaringType.Namespace != "XLua")
                {
                    extension_methods_namespace.Add(extension_method.DeclaringType.Namespace);
                }
            }
            parameters.Set("namespaces", extension_methods_namespace.Distinct().ToList());

            //warnning: filter all method start with "op_"  "add_" "remove_" may  filter some ordinary method
            parameters.Set("methods", type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly)
                .Where(method => !method.IsDefined(typeof (ExtensionAttribute), false) || method.DeclaringType != type)
                .Where(method => methodNames.ContainsKey(method.Name)) //GenericMethod can not be invoke becuase not static info available!
                .Concat(extension_methods)
                .Where(method => !isMethodInBlackList(method) && (!method.IsGenericMethod || extension_methods.Contains(method) || isSupportedGenericMethod(method)) && !isObsolete(method) && !method.Name.StartsWith("op_") && !method.Name.StartsWith("add_") && !method.Name.StartsWith("remove_"))
                .GroupBy(method => (method.Name + ((method.IsStatic && !method.IsDefined(typeof (ExtensionAttribute), false)) ? "_xlua_st_" : "")), (k, v) =>
                {
                    var overloads = new List<MethodBase>();
                    List<int> def_vals = new List<int>();
                    foreach (var overload in v.Cast<MethodBase>().OrderBy(mb => OverloadCosting(mb)))
                    {
                        int def_count = 0;
                        overloads.Add(overload);
                        def_vals.Add(def_count);

                        var ps = overload.GetParameters();
                        for (int i = ps.Length - 1; i >=0; i--)
                        {
                            if(ps[i].IsOptional ||
                            (ps[i].IsDefined(typeof(ParamArrayAttribute), false) && i > 0 && ps[i - 1].IsOptional))
                            {
                                def_count++;
                                overloads.Add(overload);
                                def_vals.Add(def_count);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    return new {
                        Name = k,
                        IsStatic = overloads[0].IsStatic && !overloads[0].IsDefined(typeof(ExtensionAttribute), false),
                        Overloads = overloads,
                        DefaultValues = def_vals
                    };
                }).ToList());

            parameters.Set("getters", type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly)
                
                .Where(prop => prop.CanRead && (prop.GetGetMethod() != null)  && prop.Name != "Item" && !isObsolete(prop) && !isMemberInBlackList(prop)).Select(prop => new { prop.Name, IsStatic = prop.GetGetMethod().IsStatic, ReadOnly = false, Type = prop.PropertyType })
                .Concat(
                    type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly)
                    .Where(field => !isObsolete(field) && !isMemberInBlackList(field))
                    .Select(field => new { field.Name, field.IsStatic, ReadOnly = field.IsInitOnly || field.IsLiteral, Type = field.FieldType })
                )/*.Where(getter => !typeof(Delegate).IsAssignableFrom(getter.Type))*/.ToList());

            parameters.Set("setters", type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly)
                .Where(prop => prop.CanWrite && (prop.GetSetMethod() != null) && prop.Name != "Item" && !isObsolete(prop) && !isMemberInBlackList(prop)).Select(prop => new { prop.Name, IsStatic = prop.GetSetMethod().IsStatic, Type = prop.PropertyType, IsProperty = true })
                .Concat(
                    type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly)
                    .Where(field => !isObsolete(field) && !isMemberInBlackList(field) && !field.IsInitOnly && !field.IsLiteral)
                    .Select(field => new { field.Name, field.IsStatic, Type = field.FieldType, IsProperty = false })
                )/*.Where(setter => !typeof(Delegate).IsAssignableFrom(setter.Type))*/.ToList());

            parameters.Set("operators", type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly)
                .Where(method => OpMethodNames.Contains(method.Name))
                .GroupBy(method => method.Name, (k, v) => new { Name = k, Overloads = v.Cast<MethodBase>().OrderBy(mb => mb.GetParameters().Length).ToList() }).ToList());

            parameters.Set("indexers", type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly)
                .Where(method => method.Name == "get_Item" && method.GetParameters().Length == 1 && !method.GetParameters()[0].ParameterType.IsAssignableFrom(typeof(string)))
                .ToList());

            parameters.Set("newindexers", type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly)
                .Where(method => method.Name == "set_Item" && method.GetParameters().Length == 2)
                .ToList());

            parameters.Set("events", type.GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly).Where(e => !isObsolete(e) && !isMemberInBlackList(e))
                .Where(ev=> ev.GetAddMethod() != null || ev.GetRemoveMethod() != null)
                .Select(ev => new { IsStatic = ev.GetAddMethod() != null? ev.GetAddMethod().IsStatic: ev.GetRemoveMethod().IsStatic, ev.Name,
                    CanSet = false, CanAdd = ev.GetRemoveMethod() != null, CanRemove = ev.GetRemoveMethod() != null, Type = ev.EventHandlerType})
                .ToList());
        }

        static void getInterfaceInfo(Type type, LuaTable parameters)
        {
            parameters.Set("type", type);

            var getters = type.GetProperties().Where(prop => prop.CanRead);
            var setters = type.GetProperties().Where(prop => prop.CanWrite);

            List<string> methodNames = type.GetMethods(BindingFlags.Public | BindingFlags.Instance
                | BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly).Select(method => method.Name).ToList();
            foreach (var getter in getters)
            {
                methodNames.Remove("get_" + getter.Name);
            }

            foreach (var setter in setters)
            {
                methodNames.Remove("set_" + setter.Name);
            }

            parameters.Set("methods", type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly)
                .Where(method => methodNames.Contains(method.Name) && !method.IsGenericMethod && !method.Name.StartsWith("op_") && !method.Name.StartsWith("add_") && !method.Name.StartsWith("remove_")) //GenericMethod can not be invoke becuase not static info available!
                    .ToList());

            parameters.Set("propertys", type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly)
                .Where(prop => (prop.CanRead || prop.CanWrite) && prop.Name != "Item")
                    .ToList());

            parameters.Set("events", type.GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly).ToList());

            parameters.Set("indexers", type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly)
                .Where(prop => (prop.CanRead || prop.CanWrite) && prop.Name == "Item")
                    .ToList());
        }

        static bool isObsolete(MemberInfo mb)
        {
            if (mb == null) return false;
            return mb.IsDefined(typeof(System.ObsoleteAttribute), false);
        }

        static bool isMemberInBlackList(MemberInfo mb)
        {
            if (mb.IsDefined(typeof(BlackListAttribute), false)) return true;

            foreach (var exclude in BlackList)
            {
                if (mb.DeclaringType.FullName == exclude[0] && mb.Name == exclude[1])
                {
                    return true;
                }
            }

            return false;
        }

        static bool isMethodInBlackList(MethodBase mb)
        {
            if (mb.IsDefined(typeof(BlackListAttribute), false)) return true;

            foreach (var exclude in BlackList)
            {
                if (mb.DeclaringType.FullName == exclude[0] && mb.Name == exclude[1])
                {
                    var parameters = mb.GetParameters();
                    if (parameters.Length != exclude.Count - 2)
                    {
                        continue;
                    }
                    bool paramsMatch = true;

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (parameters[i].ParameterType.FullName != exclude[i + 2])
                        {
                            paramsMatch = false;
                            break;
                        }
                    }
                    if (paramsMatch) return true;
                }
            }
            return false;
        }

        static Dictionary<string, LuaFunction> templateCache = new Dictionary<string, LuaFunction>();
        static void GenOne(Type type, Action<Type, LuaTable> type_info_getter, XLuaTemplate templateAsset, StreamWriter textWriter)
        {
            if (isObsolete(type)) return;
            LuaFunction template;
            if (!templateCache.TryGetValue(templateAsset.name, out template))
            {
                template = XLua.TemplateEngine.LuaTemplate.Compile(luaenv, templateAsset.text);
                templateCache[templateAsset.name] = template;
            }

            LuaTable type_info = luaenv.NewTable();
            LuaTable meta = luaenv.NewTable();
            meta.Set("__index", luaenv.Global);
            type_info.SetMetaTable(meta);
            meta.Dispose();

            type_info_getter(type, type_info);

            try
            {
                string genCode = XLua.TemplateEngine.LuaTemplate.Execute(template, type_info);
                //string filePath = save_path + type.ToString().Replace("+", "").Replace(".", "").Replace("`", "").Replace("&", "").Replace("[", "").Replace("]", "").Replace(",", "") + file_suffix + ".cs";
                textWriter.Write(genCode);
                textWriter.Flush();
            }
            catch (Exception e)
            {
#if XLUA_GENERAL
                System.Console.WriteLine("Error: gen wrap file fail! err=" + e.Message + ", stack=" + e.StackTrace);
#else
                Debug.LogError("gen wrap file fail! err=" + e.Message + ", stack=" + e.StackTrace);
#endif
            }
            finally
            {
                type_info.Dispose();
            }
        }

        static void GenEnumWrap(IEnumerable<Type> types, string save_path)
        {
            string filePath = save_path + "EnumWrap.cs";
            StreamWriter textWriter = new StreamWriter(filePath, false, Encoding.UTF8);
            
            GenOne(null, (type, type_info) =>
            {
                type_info.Set("types", types.ToList());
            }, templateRef.LuaEnumWrap, textWriter);

            textWriter.Close();
        }

        static void GenInterfaceBridge(IEnumerable<Type> types, string save_path)
        {
            foreach (var wrap_type in types)
            {
                if (!wrap_type.IsInterface) continue;

                string filePath = save_path + wrap_type.ToString().Replace("+", "").Replace(".", "")
                    .Replace("`", "").Replace("&", "").Replace("[", "").Replace("]", "").Replace(",", "") + "Bridge.cs";
                StreamWriter textWriter = new StreamWriter(filePath, false, Encoding.UTF8);
                GenOne(wrap_type, (type, type_info) =>
                {
                    getInterfaceInfo(type, type_info);
                }, templateRef.LuaInterfaceBridge, textWriter);
                textWriter.Close();
            }
        }

        class ParameterInfoSimulation
        {
            public string Name;
            public bool IsOut;
            public bool IsIn;
            public Type ParameterType;
            public bool IsParamArray;
        }

        class MethodInfoSimulation
        {
            public Type ReturnType;
            public ParameterInfoSimulation[] ParameterInfos;

            public int HashCode;

            public ParameterInfoSimulation[]  GetParameters()
            {
                return ParameterInfos;
            }

            public Type DeclaringType = null;
            public string DeclaringTypeName = null;
        }

        static MethodInfoSimulation makeMethodInfoSimulation(MethodInfo method)
        {
            int hashCode = method.ReturnType.GetHashCode();

            List<ParameterInfoSimulation> paramsExpect = new List<ParameterInfoSimulation>();

            foreach (var param in method.GetParameters())
            {
                if (param.IsOut)
                {
                    hashCode++;
                }
                hashCode += param.ParameterType.GetHashCode();
                paramsExpect.Add(new ParameterInfoSimulation()
                {
                    Name = param.Name,
                    IsOut = param.IsOut,
                    IsIn = param.IsIn,
                    ParameterType = param.ParameterType,
                    IsParamArray = param.IsDefined(typeof(System.ParamArrayAttribute), false)
                });
            }

            return new MethodInfoSimulation()
            {
                ReturnType = method.ReturnType,
                HashCode = hashCode,
                ParameterInfos = paramsExpect.ToArray(),
                DeclaringType = method.DeclaringType
            };
        }

        static bool isNotPublic(Type type)
        {
            if (type.IsByRef || type.IsArray)
            {
                return isNotPublic(type.GetElementType());
            }
            else
            {
                if ((!type.IsNested && !type.IsPublic) || (type.IsNested && !type.IsNestedPublic))
                {
                    return true;
                }
                if (type.IsGenericType)
                {
                    foreach (var ga in type.GetGenericArguments())
                    {
                        if (isNotPublic(ga))
                        {
                            return true;
                        }
                    }
                }
                if (type.IsNested)
                {
                    var parent = type.DeclaringType;
                    while (parent != null)
                    {
                        if ((!parent.IsNested && !parent.IsPublic) || (parent.IsNested && !parent.IsNestedPublic))
                        {
                            return true;
                        }
                        if (parent.IsNested)
                        {
                            parent = parent.DeclaringType;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                return false;
            }
        }

        static bool hasGenericParameter(Type type)
        {
            if (type.IsByRef || type.IsArray)
            {
                return hasGenericParameter(type.GetElementType());
            }
            if (type.IsGenericType)
            {
                foreach (var typeArg in type.GetGenericArguments())
                {
                    if (hasGenericParameter(typeArg))
                    {
                        return true;
                    }
                }
                return false;
            }
            return type.IsGenericParameter;
        }

        static MethodInfoSimulation makeHotfixMethodInfoSimulation(MethodBase hotfixMethod, HotfixFlag hotfixType)
        {
            Type retTypeExpect = (hotfixType == HotfixFlag.Stateful && hotfixMethod.IsConstructor && !hotfixMethod.IsStatic)
                    ? typeof(LuaTable) : (hotfixMethod.IsConstructor ? typeof(void) : (hotfixMethod as MethodInfo).ReturnType);
            int hashCode = retTypeExpect.GetHashCode();
            List<ParameterInfoSimulation> paramsExpect = new List<ParameterInfoSimulation>();
            if (!hotfixMethod.IsStatic) // add self
            {
                if (hotfixType == HotfixFlag.Stateful && !hotfixMethod.IsConstructor)
                {
                    paramsExpect.Add(new ParameterInfoSimulation()
                    {
                        Name = "self",
                        IsOut = false,
                        IsIn = true,
                        ParameterType = typeof(LuaTable),
                        IsParamArray = false
                    });
    
                }
                else
                {
                    paramsExpect.Add(new ParameterInfoSimulation()
                    {
                        Name = "self",
                        IsOut = false,
                        IsIn = true,
                        ParameterType = hotfixMethod.DeclaringType.IsValueType ? hotfixMethod.DeclaringType : typeof(object),
                        IsParamArray = false
                    });
                }
                hashCode += paramsExpect[0].ParameterType.GetHashCode();
            }

            foreach (var param in hotfixMethod.GetParameters())
            {
                var paramExpect = new ParameterInfoSimulation()
                {
                    Name = param.Name,
                    IsOut = param.IsOut,
                    IsIn = param.IsIn,
                    ParameterType = (param.ParameterType.IsByRef || param.ParameterType.IsValueType
                      || param.IsDefined(typeof(System.ParamArrayAttribute), false)) ? param.ParameterType : typeof(object),
                    IsParamArray = param.IsDefined(typeof(System.ParamArrayAttribute), false)
                };
                if (param.IsOut)
                {
                    hashCode++;
                }
                hashCode += paramExpect.ParameterType.GetHashCode();
                paramsExpect.Add(paramExpect);
            }

            return new MethodInfoSimulation()
            {
                HashCode = hashCode,
                ReturnType = retTypeExpect,
                ParameterInfos = paramsExpect.ToArray()
            };
        }

        class MethodInfoSimulationComparer : IEqualityComparer<MethodInfoSimulation>
        {
            public bool Equals(MethodInfoSimulation x, MethodInfoSimulation y)
            {
                if (object.ReferenceEquals(x, y)) return true;
                if (x == null || y == null)
                {
                    return false;
                }
                if (x.ReturnType != y.ReturnType)
                {
                    return false;
                }
                var xParams = x.GetParameters();
                var yParams = y.GetParameters();
                if (xParams.Length != yParams.Length)
                {
                    return false;
                }

                for (int i = 0; i < xParams.Length; i++)
                {
                    if (xParams[i].ParameterType != yParams[i].ParameterType || xParams[i].IsOut != yParams[i].IsOut)
                    {
                        return false;
                    }
                }

                return true;
            }
            public int GetHashCode(MethodInfoSimulation obj)
            {
                return obj.HashCode;
            }
        }

        static bool injectByGeneric(MethodBase method, HotfixFlag hotfixType)
        {
            if (!method.IsConstructor && (isNotPublic((method as MethodInfo).ReturnType) || hasGenericParameter((method as MethodInfo).ReturnType))) return true;

            if (!method.IsStatic &&  (hotfixType == HotfixFlag.Stateless || method.IsConstructor)
                &&((method.DeclaringType.IsValueType && isNotPublic(method.DeclaringType)) || hasGenericParameter(method.DeclaringType)))
            {
                return true;
            }

            foreach (var param in method.GetParameters())
            {
                if (((param.ParameterType.IsValueType || param.ParameterType.IsByRef) && isNotPublic(param.ParameterType)) || hasGenericParameter(param.ParameterType)) return true;
            }
            return false;
        }
        
        static void GenDelegateBridge(IEnumerable<Type> types, string save_path, IEnumerable<Type> hotfix_check_types)
        {
            string filePath = save_path + "DelegatesGensBridge.cs";
            StreamWriter textWriter = new StreamWriter(filePath, false, Encoding.UTF8);
            types = types.Where(type => !type.GetMethod("Invoke").GetParameters().Any(paramInfo => paramInfo.ParameterType.IsGenericParameter));
            var hotfxDelegates = new List<MethodInfoSimulation>();
            var bindingAttrOfMethod = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.NonPublic;
            var bindingAttrOfConstructor = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.NonPublic;
            foreach (var type in (from type in hotfix_check_types where type.IsDefined(typeof(HotfixAttribute), false) select type))
            {
                var hotfixType = ((type.GetCustomAttributes(typeof(HotfixAttribute), false)[0]) as HotfixAttribute).Flag;
                hotfxDelegates.AddRange(type.GetMethods(bindingAttrOfMethod)
                    .Cast<MethodBase>()
                    .Concat(type.GetConstructors(bindingAttrOfConstructor).Cast<MethodBase>())
                    .Where(method => !injectByGeneric(method, hotfixType))
                    .Select(method => makeHotfixMethodInfoSimulation(method, hotfixType)));
            }
            foreach (var kv in HotfixCfg)
            {
                hotfxDelegates.AddRange(kv.Key.GetMethods(bindingAttrOfMethod)
                    .Cast<MethodBase>()
                    .Concat(kv.Key.GetConstructors(bindingAttrOfConstructor).Cast<MethodBase>())
                    .Where(method => !injectByGeneric(method, kv.Value))
                    .Select(method => makeHotfixMethodInfoSimulation(method, kv.Value)));
            }
            var comparer = new MethodInfoSimulationComparer();
            hotfxDelegates = hotfxDelegates.Distinct(comparer).ToList();
            for(int i = 0; i < hotfxDelegates.Count; i++)
            {
                hotfxDelegates[i].DeclaringTypeName = "__Gen_Hotfix_Delegate" + i;
            }
            var delegates_groups = types.Select(delegate_type => makeMethodInfoSimulation(delegate_type.GetMethod("Invoke")))
                .Concat(hotfxDelegates)
                .GroupBy(d => d, comparer).Select((group) => new { Key = group.Key, Value = group.ToList()});
            GenOne(typeof(DelegateBridge), (type, type_info) =>
            {
                type_info.Set("delegates_groups", delegates_groups.ToList());
                type_info.Set("hotfx_delegates", hotfxDelegates);
            }, templateRef.LuaDelegateBridge, textWriter);
            textWriter.Close();
        }

        static void GenWrapPusher(IEnumerable<Type> types, string save_path)
        {
            string filePath = save_path + "WrapPusher.cs";
            StreamWriter textWriter = new StreamWriter(filePath, false, Encoding.UTF8);
            GenOne(typeof(ObjectTranslator), (type, type_info) =>
            {
                type_info.Set("purevaluetypes", types
                     .Where(t => t.IsEnum || (!t.IsPrimitive && SizeOf(t) != -1))
                     .Select(t => new { Type = t, Size = SizeOf(t) }).ToList());
                type_info.Set("tableoptimzetypes", types.Where(t => !t.IsEnum && SizeOf(t) == -1)
                     .Select(t => new { Type = t, Fields = t.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly) })
                     .ToList());
            }, templateRef.LuaWrapPusher, textWriter);
            textWriter.Close();
        }

        static void GenWrap(IEnumerable<Type> types, string save_path)
        {
            types = types.Where(type=>!type.IsEnum);

            var typeMap = types.ToDictionary(type => {
                //Debug.Log("type:" + type);
                return type.ToString();
            });

            foreach (var wrap_type in types)
            {
                string filePath = save_path + wrap_type.ToString().Replace("+", "").Replace(".", "")
                    .Replace("`", "").Replace("&", "").Replace("[", "").Replace("]", "").Replace(",", "") + "Wrap.cs";
                StreamWriter textWriter = new StreamWriter(filePath, false, Encoding.UTF8);
                if (wrap_type.IsEnum)
                {
                    GenOne(wrap_type, (type, type_info) =>
                    {
                        type_info.Set("type", type);
                        type_info.Set("fields", type.GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static)
                            .Where(field => !isObsolete(field))
                            .ToList());
                    }, templateRef.LuaEnumWrap, textWriter);
                }
                else if (typeof(Delegate).IsAssignableFrom(wrap_type))
                {


                    GenOne(wrap_type, (type, type_info) =>
                    {
                        type_info.Set("type", type);
                        type_info.Set("delegate", type.GetMethod("Invoke"));
                    }, templateRef.LuaDelegateWrap, textWriter);

                }
                else
                {
                    GenOne(wrap_type, (type, type_info) =>
                    {
                        if (type.BaseType != null && typeMap.ContainsKey(type.BaseType.ToString()))
                        {
                            type_info.Set("base", type.BaseType);
                        }
                        getClassInfo(type, type_info);
                    }, templateRef.LuaClassWrap, textWriter);
                }
                textWriter.Close();
            }
        }

#if !XLUA_GENERAL
        static void clear(string path)
        {
            System.IO.Directory.Delete(path, true);
            AssetDatabase.DeleteAsset(path.Substring(path.IndexOf("Assets") + "Assets".Length));

            AssetDatabase.Refresh();
        }
#endif

        class DelegateByMethodDecComparer : IEqualityComparer<Type>
        {
            public bool Equals(Type x, Type y)
            {
                return Utils.IsParamsMatch(x.GetMethod("Invoke"), y.GetMethod("Invoke"));
            }
            public int GetHashCode(Type obj)
            {
                int hc = 0;
                var method = obj.GetMethod("Invoke");
                hc += method.ReturnType.GetHashCode();
                foreach (var pi in method.GetParameters())
                {
                    hc += pi.ParameterType.GetHashCode();
                }
                return hc;
            }
        }

        public static void GenDelegateBridges(IEnumerable<Type> hotfix_check_types)
        {
            var delegate_types = CSharpCallLua.Where(type => typeof(Delegate).IsAssignableFrom(type));

            GenDelegateBridge(delegate_types, GeneratorConfig.common_path, hotfix_check_types);
        }

        public static void GenEnumWraps()
        {
            var enum_types = LuaCallCSharp.Where(type => type.IsEnum).Distinct();

#if XLUA_GENERAL
            //TODO: 后续支持位运算重载后，这块可以去掉
            luaenv.Global.Set("g_enum_get_fields_flag", BindingFlags.Static | BindingFlags.Public);
#endif
            GenEnumWrap(enum_types, GeneratorConfig.common_path);
#if XLUA_GENERAL
            luaenv.Global.Set("g_enum_get_fields_flag", (object)null);
#endif
        }

        public static void GenLuaRegister(bool minimum = false)
        {
            var wraps = minimum ? new List<Type>() : LuaCallCSharp;

            var itf_bridges = CSharpCallLua.Where(t => t.IsInterface);

            string filePath = GeneratorConfig.common_path + "XLuaGenAutoRegister.cs";
            StreamWriter textWriter = new StreamWriter(filePath, false, Encoding.UTF8);
            var extension_methods = from t in ReflectionUse
                                    where t.IsDefined(typeof(ExtensionAttribute), false)
                                    from method in t.GetMethods(BindingFlags.Static | BindingFlags.Public)
                                    where !method.ContainsGenericParameters && method.IsDefined(typeof(ExtensionAttribute), false)
                                    select method;
            GenOne(typeof(DelegateBridgeBase), (type, type_info) =>
            {
                type_info.Set("wraps", wraps.ToList());
                type_info.Set("itf_bridges", itf_bridges.ToList());
                type_info.Set("extension_methods", extension_methods.ToList());
            }, templateRef.LuaRegister, textWriter);
            textWriter.Close();
        }

        public static void AllSubStruct(Type type, Action<Type> cb)
        {
            if (!type.IsPrimitive && type != typeof(decimal))
            {
                cb(type);
                foreach(var fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    AllSubStruct(fieldInfo.FieldType, cb);
                }

                foreach(var propInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    if ((AdditionalProperties.ContainsKey(type) && AdditionalProperties[type].Contains(propInfo.Name))
                        || propInfo.IsDefined(typeof(AdditionalPropertiesAttribute), false))
                    {
                        AllSubStruct(propInfo.PropertyType, cb);
                    }
                }
            }
        }

        class XluaFieldInfo
        {
            public string Name;
            public Type Type;
            public bool IsField;
            public int Size;
        }

        public static void GenPackUnpack(IEnumerable<Type> types, string save_path)
        {
            var set = types.ToDictionary(type => type);
            List<Type> all_types = new List<Type>();

            foreach(var type in types)
            {
                AllSubStruct(type, (t) =>
                {
                    all_types.Add(t);
                });
            }

            string filePath = save_path + "PackUnpack.cs";
            StreamWriter textWriter = new StreamWriter(filePath, false, Encoding.UTF8);
            GenOne(typeof(CopyByValue), (type, type_info) =>
            {
                type_info.Set("type_infos", all_types.Distinct().Select(t =>
                {
                    var fs = t.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                        .Select(fi => new XluaFieldInfo { Name = fi.Name, Type = fi.FieldType, IsField = true, Size = SizeOf(fi.FieldType) })
                        .Concat(t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                        .Where(prop => {
                            return (AdditionalProperties.ContainsKey(t) && AdditionalProperties[t].Contains(prop.Name))
                                || prop.IsDefined(typeof(AdditionalPropertiesAttribute), false);
                        })
                        .Select(prop=> new XluaFieldInfo { Name = prop.Name, Type = prop.PropertyType, IsField = false, Size = SizeOf(prop.PropertyType) }));
                    int float_field_count = 0;
                    bool only_float = true;
                    foreach (var f in fs)
                    {
                        if (f.Type == typeof(float))
                        {
                            float_field_count++;
                        }
                        else
                        {
                            only_float = false;
                            break;
                        }
                    }
                    List<List<XluaFieldInfo>> grouped_field = null;
                    if (only_float && float_field_count > 1)
                    {
                        grouped_field = new List<List<XluaFieldInfo>>();
                        List<XluaFieldInfo> group = null;
                        foreach (var f in fs)
                        {
                            if (group == null) group = new List<XluaFieldInfo>();
                            group.Add(f);
                            if (group.Count >= 6)
                            {
                                grouped_field.Add(group);
                                group = null;
                            }
                        }
                        if (group != null) grouped_field.Add(group);
                    }
                    return new { Type = t, FieldInfos = fs.ToList(), FieldGroup = grouped_field, IsRoot = set.ContainsKey(t) };
                }).ToList());
            }, templateRef.PackUnpack, textWriter);
            textWriter.Close();
        }

        //lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
        public static List<Type> LuaCallCSharp = null;

        //C#静态调用Lua的配置（包括事件的原型），仅可以配delegate，interface
        public static List<Type> CSharpCallLua = null;

        //黑名单
        public static List<List<string>> BlackList = null;

        public static List<Type> GCOptimizeList = null;

        public static Dictionary<Type, List<string>> AdditionalProperties = null;

        public static List<Type> ReflectionUse = null;

        public static Dictionary<Type, HotfixFlag> HotfixCfg = null;

        static void AddToList(List<Type> list, Func<object> get)
        {
            object obj = get();
            if (obj is Type)
            {
                list.Add(obj as Type);
            }
            else if (obj is IEnumerable<Type>)
            {
                list.AddRange(obj as IEnumerable<Type>);
            }
        }

        static void MergeCfg(MemberInfo test, Type cfg_type, Func<object> get_cfg)
        {
            if (test.IsDefined(typeof(LuaCallCSharpAttribute), false))
            {
                AddToList(LuaCallCSharp, get_cfg);
                object[] ccla = test.GetCustomAttributes(typeof(LuaCallCSharpAttribute), false);
                if (ccla.Length == 1 && (((ccla[0] as LuaCallCSharpAttribute).Flag & GenFlag.GCOptimize) != 0))
                {
                    AddToList(GCOptimizeList, get_cfg);
                }
            }
            if (test.IsDefined(typeof(CSharpCallLuaAttribute), false))
            {
                AddToList(CSharpCallLua, get_cfg);
            }
            if (test.IsDefined(typeof(GCOptimizeAttribute), false))
            {
                AddToList(GCOptimizeList, get_cfg);
            }
            if (test.IsDefined(typeof(ReflectionUseAttribute), false))
            {
                AddToList(ReflectionUse, get_cfg);
            }
            if (test.IsDefined(typeof(HotfixAttribute), false))
            {
                object cfg = get_cfg();
                if (cfg is IEnumerable<Type>)
                {
                    var hotfixType = ((test.GetCustomAttributes(typeof(HotfixAttribute), false)[0]) as HotfixAttribute).Flag;
                    foreach (var type in cfg as IEnumerable<Type>)
                    {
                        if (!HotfixCfg.ContainsKey(type) && !isObsolete(type) 
                            && !type.IsEnum && !typeof(Delegate).IsAssignableFrom(type)
                            && (!type.IsGenericType || type.IsGenericTypeDefinition) 
                            && (type.Namespace == null || (type.Namespace != "XLua" && !type.Namespace.StartsWith("XLua.")))
                            && (type.Module.Assembly.GetName().Name == "Assembly-CSharp"))
                        {
                            HotfixCfg.Add(type, hotfixType);
                        }
                    }
                }
            }
            if (test.IsDefined(typeof(BlackListAttribute), false)
                        && (typeof(List<List<string>>)).IsAssignableFrom(cfg_type))
            {
                BlackList.AddRange(get_cfg() as List<List<string>>);
            }

            if (test.IsDefined(typeof(AdditionalPropertiesAttribute), false)
                        && (typeof(Dictionary<Type, List<string>>)).IsAssignableFrom(cfg_type))
            {
                var cfg = get_cfg() as Dictionary<Type, List<string>>;
                foreach (var kv in cfg)
                {
                    if (!AdditionalProperties.ContainsKey(kv.Key))
                    {
                        AdditionalProperties.Add(kv.Key, kv.Value);
                    }
                }
            }
        }

        public static void GetGenConfig(IEnumerable<Type> check_types)
        {
            LuaCallCSharp = new List<Type>();

            CSharpCallLua = new List<Type>();

            GCOptimizeList = new List<Type>();

            AdditionalProperties = new Dictionary<Type, List<string>>();

            ReflectionUse = new List<Type>();

            BlackList = new List<List<string>>()
            {
            };

            HotfixCfg = new Dictionary<Type, HotfixFlag>();

            foreach (var t in check_types)
            {
                if(!t.IsInterface && typeof(GenConfig).IsAssignableFrom(t))
                {
                    var cfg = Activator.CreateInstance(t) as GenConfig;
                    if (cfg.LuaCallCSharp != null) LuaCallCSharp.AddRange(cfg.LuaCallCSharp);
                    if (cfg.CSharpCallLua != null) CSharpCallLua.AddRange(cfg.CSharpCallLua);
                    if (cfg.BlackList != null) BlackList.AddRange(cfg.BlackList);
                }
                else if (!t.IsInterface && typeof(GCOptimizeConfig).IsAssignableFrom(t))
                {
                    var cfg = Activator.CreateInstance(t) as GCOptimizeConfig;
                    if (cfg.TypeList != null) GCOptimizeList.AddRange(cfg.TypeList);
                    if (cfg.AdditionalProperties != null)
                    {
                        foreach(var kv in cfg.AdditionalProperties)
                        {
                            if(!AdditionalProperties.ContainsKey(kv.Key))
                            {
                                AdditionalProperties.Add(kv.Key, kv.Value);
                            }
                        }
                    }
                }
                else if (!t.IsInterface && typeof(ReflectionConfig).IsAssignableFrom(t))
                {
                    var cfg = Activator.CreateInstance(t) as ReflectionConfig;
                    ReflectionUse.AddRange(cfg.ReflectionUse);
                }

                MergeCfg(t, null, () => t);

                if (!t.IsAbstract || !t.IsSealed) continue;

                var fields = t.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                for (int i = 0; i < fields.Length; i++)
                {
                    var field = fields[i];
                    MergeCfg(field, field.FieldType, () => field.GetValue(null));
                }

                var props = t.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                for (int i = 0; i < props.Length; i++)
                {
                    var prop = props[i];
                    MergeCfg(prop, prop.PropertyType, () => prop.GetValue(null, null));
                }
            }
            LuaCallCSharp = LuaCallCSharp.Distinct()
                .Where(type=>/*type.IsPublic && */!isObsolete(type) && !type.IsGenericTypeDefinition)
                .Where(type => !typeof(Delegate).IsAssignableFrom(type))
                .ToList();//public的内嵌Enum（其它类型未测试），IsPublic为false，像是mono的bug
            CSharpCallLua = CSharpCallLua.Distinct()
                .Where(type =>/*type.IsPublic && */!isObsolete(type) && !type.IsGenericTypeDefinition)
                .ToList();
            GCOptimizeList = GCOptimizeList.Distinct()
                .Where(type =>/*type.IsPublic && */!isObsolete(type) && !type.IsGenericTypeDefinition)
                .ToList();
            ReflectionUse = ReflectionUse.Distinct()
                .Where(type =>/*type.IsPublic && */!isObsolete(type) && !type.IsGenericTypeDefinition)
                .ToList();
        }

        static Dictionary<Type, int> type_size = new Dictionary<Type, int>()
        {
            { typeof(byte), 1 },
            { typeof(sbyte), 1 },
            { typeof(short), 2 },
            { typeof(ushort), 2 },
            { typeof(int), 4 },
            { typeof(uint), 4 },
            { typeof(long), 8 },
            { typeof(ulong), 8 },
            { typeof(float), 4 },
            { typeof(double), 8 },
            { typeof(decimal), 16 }
        };

        static int SizeOf(Type type)
        {
            if (type_size.ContainsKey(type))
            {
                return type_size[type];
            }

            if (!CopyByValue.IsStruct(type))
            {
                return -1;
            }

            int size = 0;
            foreach(var fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                int t_size = SizeOf(fieldInfo.FieldType);
                if (t_size == -1)
                {
                    size = -1;
                    break;
                }
                size += t_size;
            }
            if (size != -1)
            {
                foreach (var propInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    if ((AdditionalProperties.ContainsKey(type) && AdditionalProperties[type].Contains(propInfo.Name)) || propInfo.IsDefined(typeof(AdditionalPropertiesAttribute), false))
                    {
                        int t_size = SizeOf(propInfo.PropertyType);
                        if (t_size == -1)
                        {
                            size = -1;
                            break;
                        }
                        size += t_size;
                    }
                }
            }

            if (!type_size.ContainsKey(type))
            {
                type_size.Add(type, size);
            }

            return size;
        }

        public static void Gen(IEnumerable<Type> wraps, IEnumerable<Type> gc_optimze_list, IEnumerable<Type> itf_bridges, string save_path)
        {
            templateCache.Clear();
            Directory.CreateDirectory(save_path);
            GenWrap(wraps, save_path);
            GenWrapPusher(gc_optimze_list.Concat(wraps.Where(type=>type.IsEnum)).Distinct(), save_path);
            GenPackUnpack(gc_optimze_list.Where(type => !type.IsPrimitive && SizeOf(type) != -1), save_path);
            GenInterfaceBridge(itf_bridges, save_path);
        }

        public static void GenCodeForClass(bool minimum = false)
        {
            var warp_types = minimum ? new List<Type>() : LuaCallCSharp;
            var itf_bridges_types = CSharpCallLua.Where(type => type.IsInterface).Distinct();

            Gen(warp_types, GCOptimizeList, itf_bridges_types, GeneratorConfig.common_path);
        }

#if XLUA_GENERAL
        public static void GenAll(XLuaTemplates templates, IEnumerable<Type> all_types)
        {
            var start = DateTime.Now;
            Directory.CreateDirectory(GeneratorConfig.common_path);
            templateRef = templates;
            GetGenConfig(all_types.Where(type => !type.IsGenericTypeDefinition));
            luaenv.DoString("require 'TemplateCommon'");
            var gen_push_types_setter = luaenv.Global.Get<LuaFunction>("SetGenPushAndUpdateTypes");
            gen_push_types_setter.Call(GCOptimizeList.Where(t => !t.IsPrimitive && SizeOf(t) != -1).Concat(LuaCallCSharp.Where(t => t.IsEnum)).Distinct().ToList());
            GenDelegateBridges(all_types);
            GenEnumWraps();
            GenCodeForClass();
            GenLuaRegister();
            Console.WriteLine("finished! use " + (DateTime.Now - start).TotalMilliseconds + " ms");
            luaenv.Dispose();
        }
#endif

#if !XLUA_GENERAL
        static void callCustomGen()
        {
            foreach (var method in (from type in Utils.GetAllTypes()
                               from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                               where method.IsDefined(typeof(GenCodeMenuAttribute), false) select method))
            {
                method.Invoke(null, new object[] { });
            }
        }

        [MenuItem("XLua/Generate Code", false, 1)]
        public static void GenAll()
        {
            var start = DateTime.Now;
            Directory.CreateDirectory(GeneratorConfig.common_path);
            GetGenConfig(Utils.GetAllTypes());
            luaenv.DoString("require 'TemplateCommon'");
            var gen_push_types_setter = luaenv.Global.Get<LuaFunction>("SetGenPushAndUpdateTypes");
            gen_push_types_setter.Call(GCOptimizeList.Where(t => !t.IsPrimitive && SizeOf(t) != -1).Concat(LuaCallCSharp.Where(t => t.IsEnum)).Distinct().ToList());
            GenDelegateBridges(Utils.GetAllTypes(false));
            GenEnumWraps();
            GenCodeForClass();
            GenLuaRegister();
            callCustomGen();
            Debug.Log("finished! use " + (DateTime.Now - start).TotalMilliseconds + " ms");
            AssetDatabase.Refresh();
        }

        [MenuItem("XLua/Clear Generated Code", false, 2)]
        public static void ClearAll()
        {
            clear(GeneratorConfig.common_path);
        }

        public delegate IEnumerable<CustomGenTask> GetTasks(LuaEnv lua_env, UserConfig user_cfg);

        public static void CustomGen(string template_src, GetTasks get_tasks)
        {
            GetGenConfig(Utils.GetAllTypes());

            LuaFunction template = XLua.TemplateEngine.LuaTemplate.Compile(luaenv,
                template_src);
            foreach (var gen_task in get_tasks(luaenv, new UserConfig() {
                LuaCallCSharp = LuaCallCSharp,
                CSharpCallLua = CSharpCallLua,
                ReflectionUse = ReflectionUse
            }))
            {
                LuaTable meta = luaenv.NewTable();
                meta.Set("__index", luaenv.Global);
                gen_task.Data.SetMetaTable(meta);
                meta.Dispose();

                try
                {
                    string genCode = XLua.TemplateEngine.LuaTemplate.Execute(template, gen_task.Data);
                    gen_task.Output.Write(genCode);
                    gen_task.Output.Flush();
                }
                catch (Exception e)
                {
                    Debug.LogError("gen file fail! template=" + template_src + ", err=" + e.Message + ", stack=" + e.StackTrace);
                }
            }
        }

#endif

        private static bool isSupportedGenericMethod(MethodInfo method)
        {
            if (!method.ContainsGenericParameters)
                return true;
            var methodParameters = method.GetParameters();
            var hasValidGenericParameter = false;
            for (var i = 0; i < methodParameters.Length; i++)
            {
                var parameterType = methodParameters[i].ParameterType;
                if (parameterType.IsGenericParameter)
                {
                    var parameterConstraints = parameterType.GetGenericParameterConstraints();
                    if (parameterConstraints.Length == 0 || !parameterConstraints[0].IsClass)
                        return false;
                    hasValidGenericParameter = true;
                }
            }
            return hasValidGenericParameter;
        }

#if !XLUA_GENERAL
        [InitializeOnLoad]
        public class Startup
        {

            static Startup()
            {
                EditorApplication.update += Update;
            }


            static void Update()
            {
                EditorApplication.update -= Update;

                if (!System.IO.File.Exists(GeneratorConfig.common_path + "XLuaGenAutoRegister.cs"))
                {
                    UnityEngine.Debug.LogWarning("code has not been genrate, may be not work in phone!");
                }
            }

        }
#endif
    }
}