using System;
using UnityEngine;
using System.Collections;
using CatLib.Support;
using System.Collections.Generic;
using System.Reflection;
using CatLib.Base;

namespace CatLib.Container
{
    ///<summary>容器</summary>
    public class CContainer : CComponentBase, IContainer
    {

        /// <summary>绑定数据</summary>
        protected class BindData
        {
            /// <summary>提供服务</summary>
            public Func<IContainer, object[], object> Func { get; protected set; }

            /// <summary>是否是静态服务</summary>
            public bool IsStatic { get; protected set; }

            public BindData(Func<IContainer, object[], object> func , bool isStatic)
            {
                Func = func;
                IsStatic = isStatic;
            }
        }

        ///<summary>绑定的服务</summary>
        protected Dictionary<Type, Dictionary<string, BindData>> binds = new Dictionary<Type, Dictionary<string, BindData>>();

        ///<summary>服务实例的内容</summary>
        protected Dictionary<Type, Dictionary<string, object>> instances = new Dictionary<Type, Dictionary<string, object>>();

        ///<summary>会触发每帧更新的服务实例</summary>
        protected IUpdate[] updates;

        ///<summary>会触发每帧延迟更新的服务实例</summary>
        protected ILateUpdate[] lateUpdates;

        /// <summary>绑定服务</summary>
        /// <param name="from">接口</param>
        /// <param name="to">实例的类</param>
        /// <param name="alias">别名</param>
        /// <param name="isStatic">是否是静态的</param>
        /// <returns></returns>
        public IContainer Bind(Type from, Func<IContainer, object[], object> to, string alias , bool isStatic)
        {

            if (alias == null) { alias = string.Empty; }

            if (binds.ContainsKey(from)) {
                binds[from].Remove(alias);
            } else { binds.Add(from, new Dictionary<string, BindData>()); }

            if (instances.ContainsKey(from)) {
                instances[from].Remove(alias);
            } else { instances.Add(from, new Dictionary<string, object>()); }

            binds[from].Add(alias, new BindData(to, isStatic));

            return this;

		}

        /// <summary>构造服务</summary>
        /// <param name="from">来自于接口</param>
        /// <param name="alias">别名</param>
        /// <param name="param">附带参数</param>
        /// <returns></returns>
        public object Make(Type from , string alias , params object[] param)
        {
            if (alias == null) { alias = string.Empty; }
            if (instances.ContainsKey(from) && instances[from].ContainsKey(alias)) { return instances[from][alias]; }

            Func<IContainer, object[], object> concrete = this.GetConcrete(from, alias);
            object objectData = null;
            if (concrete == null) {

                objectData = Build(from, param);
                DIAttr(objectData);
                this.AddInstances(from , alias, objectData);

            }else
            {
                objectData = this.Build(concrete, param);
                this.DIAttr(objectData);
                this.AddInstances(from, alias, objectData);
            }

            return objectData;
        }

        /// <summary>构造服务</summary>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected object Build(Type type, object[] param)
        {
            ConstructorInfo[] constructor = type.GetConstructors();
            if (constructor.Length <= 0) { return Activator.CreateInstance(type); }

            List<ParameterInfo> parameter = new List<ParameterInfo>(constructor[constructor.Length - 1].GetParameters());
            parameter.RemoveRange(0, param.Length);

            if (parameter.Count > 0) { param = GetDependencies(parameter, param); }

            return Activator.CreateInstance(type, param);
        }

        /// <summary>获取依赖关系</summary>
        /// <param name="paramInfo">参数信息</param>
        /// <param name="param">手动输入的参数</param>
        /// <returns></returns>
        protected object[] GetDependencies(List<ParameterInfo> paramInfo, object[] param)
        {
            List<object> myParam = new List<object>(param);

            foreach (ParameterInfo info in paramInfo)
            {
                if (info.ParameterType.IsClass)
                {
                    myParam.Add(ResloveClass(info));
                }
                else
                {
                    myParam.Add(ResolveNonClass(info));
                }
            }

            return myParam.ToArray();

        }

        /// <summary>解决非类类型</summary>
        /// <param name="info">参数信息</param>
        /// <returns></returns>
        protected object ResolveNonClass(ParameterInfo info)
        {
            return info.DefaultValue;
        }

        /// <summary>解决类类型</summary>
        /// <param name="info">参数信息</param>
        /// <returns></returns>
        protected object ResloveClass(ParameterInfo info)
        {
            object obj = this.Make(info.ParameterType, null);
            if (obj == null) { return info.DefaultValue; }
            return obj;
        }


        /// <summary>构造服务</summary>
        /// <param name="func">闭包</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        protected object Build(Func<IContainer, object[], object> func, object[] param)
        {
            return func(this, param);
        }

        protected void ForceAddInstances(Type from , string alias , object objectData)
        {
            if (alias == null) { alias = string.Empty; }
            if (objectData == null) { return; }
            if (instances.ContainsKey(from))
            {
                instances[from].Remove(alias);
            }
            else { instances.Add(from, new Dictionary<string, object>()); }
            instances[from].Add(alias, objectData);
        }

        /// <summary>添加到实例内容</summary>
        /// <param name="type">类型</param>
        /// <param name="alias">别名</param>
        /// <param name="objectData">实体数据</param>
        protected void AddInstances(Type from, string alias, object objectData)
        {
            if (alias == null) { alias = string.Empty; }
            if (objectData == null) { return; }
            if (!binds.ContainsKey(from) || !binds[from].ContainsKey(alias) || !binds[from][alias].IsStatic) { return; }
            if (instances.ContainsKey(from))
            {
                instances[from].Remove(alias);
            }
            else { instances.Add(from, new Dictionary<string, object>()); }
            instances[from].Add(alias, objectData);
        }

        /// <summary>获取服务闭包</summary>
        /// <param name="from">来自于接口</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
	    protected Func<IContainer, object[], object> GetConcrete(Type from, string alias)
        {
            if (!binds.ContainsKey(from) || !binds[from].ContainsKey(alias)) return null;
            return binds[from][alias].Func;
        }

        /// <summary>属性注入</summary>
        /// <param name="cls"></param>
        protected void DIAttr(object cls)
        {
            if (cls == null) { return; }

            foreach (PropertyInfo property in cls.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanWrite) { continue; }
                object[] propertyAttrs = property.GetCustomAttributes(typeof(CDependency), true);
                if (propertyAttrs.Length <= 0) { continue; }
                CDependency dependency = propertyAttrs[0] as CDependency;
               
                if (property.PropertyType.IsClass)
                {
                    property.SetValue(cls, ResloveClassAttr(property.PropertyType , dependency.Alias), null);
                }
                else
                {
                    property.SetValue(cls, ResolveNonClassAttr(property.PropertyType, dependency.Alias), null);
                }
            }

        }

        /// <summary>解决非类类型</summary>
        /// <param name="type">参数类型</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        protected object ResolveNonClassAttr(Type type, string alias)
        {
            return null;
        }

        /// <summary>解决类类型</summary>
        /// <param name="type">参数类型</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        protected object ResloveClassAttr(Type type, string alias)
        {
            object obj = this.Make(type , alias);
            return obj;
        }

        public object this[Type from]
        {

            get
            {
                return this.Make(from , null);
            }

        }


    }
}