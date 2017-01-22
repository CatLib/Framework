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
    public class CContainer : CMonoComponent, IContainer
    {

        /// <summary>
        /// 绑定数据
        /// </summary>
        private Dictionary<string , CBindData> binds = new Dictionary<string, CBindData>();

        ///<summary>
        /// 静态化内容
        ///</summary>
        private Dictionary<string, object> instances = new Dictionary<string, object>();

        ///<summary>
        /// 别名(key: 别名 , value: 服务名)
        ///</summary>
        private Dictionary<string, string> alias = new Dictionary<string, string>();

        /// <summary>
        /// 修饰器
        /// </summary>
        private List<Func<IContainer , CBindData, object, object>> decorator = new List<Func<IContainer , CBindData, object , object>>();
         
        /// <summary>
        /// 配置信息
        /// </summary>
        private Dictionary<Type, CConfig> config;

        /// <summary>
        /// 设定一个别名
        /// </summary>
        /// <param name="alias">别名</param>
        /// <param name="service">指向的服务</param>
        /// <returns></returns>
        public IContainer Alias(string alias , string service)
        {
            alias = Normalize(alias);
            service = Normalize(service);
            if (this.alias.ContainsKey(alias)) { this.alias.Remove(alias); }
            this.alias.Add(alias, service);
            return this;
        }

        /// <summary>
        /// 绑定一个服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实体</param>
        /// <param name="isStatic">服务是否静态化</param>
        /// <returns></returns>
        public CBindData Bind(string service , string concrete , bool isStatic)
        {
            service  = Normalize(service);
            concrete = Normalize(concrete);
            return Bind(service, (c, param) => {
                CContainer container = c as CContainer;
                return container.NormalMake(concrete, false, param);
            }, true);
        }

        /// <summary>
        /// 绑定一个服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实体</param>
        /// <param name="isStatic">服务是否静态化</param>
        /// <returns></returns>
        public CBindData Bind(string service , Func<IContainer, object[], object> concrete, bool isStatic)
        {
            service = Normalize(service);

            instances.Remove(service);
            alias = alias.RemoveValue(service);
            alias.Remove(service);

            CBindData bindData = new CBindData(this , service , concrete, isStatic);

            binds.Add(service, bindData);

            return bindData;
        }

        /// <summary>
        /// 构造服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <param name="param">附带参数</param>
        /// <returns></returns>
        public object Make(string service, params object[] param)
        {
            service = Normalize(service);
            return NormalMake(service, true, param);
        }

        /// <summary>添加到静态内容</summary>
        /// <param name="type">类型</param>
        /// <param name="alias">别名</param>
        /// <param name="objectData">实体数据</param>
        public void Instances(string service, object objectData)
        {
            if (objectData == null) { return; }
            service = GetAlias(service);

            if (instances.ContainsKey(service))
            {
                instances.Remove(service);
            }

            var bindData = GetBindData(service);
            objectData = ExecDecorator(bindData, bindData.ExecDecorator(objectData));
            
            instances.Add(service, objectData);
        }

        /// <summary>
        /// 修饰器
        /// </summary>
        /// <param name="func"></param>
        public IContainer Decorator(Func<IContainer , CBindData, object, object> func)
        {
            if (decorator == null) { decorator = new List<Func<IContainer , CBindData, object, object>>(); }
            decorator.Add(func);
            foreach(KeyValuePair<string , object> data in instances)
            {
                var bindData = GetBindData(data.Key);
                instances[data.Key] = func(this , bindData, data.Value);
            }
            return this;
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        protected void InitConfig()
        {
            config = new Dictionary<Type, CConfig>();
            Type[] types = typeof(CConfig).GetChildTypes();
            foreach (Type t in types)
            {
                CConfig conf = Make(t.ToString(), null) as CConfig;
                if (conf == null) { continue; }
                if (!config.ContainsKey(conf.Class))
                {
                    config.Add(conf.Class, conf);
                }
            }
        }

        private object ExecDecorator(CBindData bindData, object obj)
        {
            if (decorator != null)
            {
                foreach (Func<IContainer , CBindData, object, object> func in decorator)
                {
                    obj = func(this , bindData, obj);
                }
            }
            return obj;
        }

        /// <summary>
        /// 构造服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="withConcrete">是否允许从Concrete获取</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        private object NormalMake(string service , bool withConcrete , params object[] param)
        {
            service = GetAlias(service);
            if (instances.ContainsKey(service)) { return instances[service]; }

            var bindData = GetBindData(service);
            object objectData = withConcrete ? NormalBuild(bindData, param) : Build(bindData, param);

            DIAttr(bindData , objectData);

            if (bindData.IsStatic)
            {
                Instances(service, objectData);
            }

            return objectData;
        }

        /// <summary>
        /// 常规编译
        /// </summary>
        /// <param name="bindData"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private object NormalBuild(CBindData bindData, object[] param)
        {
            if (bindData.Concrete != null)
            {
                return bindData.Concrete(this, param);
            }

            return Build(bindData, param);
        }

        /// <summary>构造服务</summary>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private object Build(CBindData bindData, object[] param)
        {
            if (param == null) { param = new object[] { }; }
            Type type = Type.GetType(bindData.Service);
            ConstructorInfo[] constructor = type.GetConstructors();
            if (constructor.Length <= 0)
            {
                if (type.IsAbstract || type.IsInterface)
                {
                    return null;
                }
                return Activator.CreateInstance(type);
            }

            List<ParameterInfo> parameter = new List<ParameterInfo>(constructor[constructor.Length - 1].GetParameters());
            parameter.RemoveRange(0, param.Length);

            if (parameter.Count > 0) { param = GetDependencies(bindData , type, parameter, param); }

            return Activator.CreateInstance(type, param);
        }

        /// <summary>标准化服务名</summary>
        /// <param name="service">服务名</param>
        /// <returns></returns>
        private string Normalize(string service)
        {
            return service;
        }

        /// <summary>属性注入</summary>
        /// <param name="cls"></param>
        private void DIAttr(CBindData bindData , object cls)
        {
            if (cls == null) { return; }

            foreach (PropertyInfo property in cls.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanWrite) { continue; }
                object[] propertyAttrs = property.GetCustomAttributes(typeof(CDependency), true);
                if (propertyAttrs.Length <= 0) { continue; }
                CDependency dependency = propertyAttrs[0] as CDependency;

                if (property.PropertyType.IsClass || property.PropertyType.IsInterface)
                {
                    property.SetValue(cls, ResloveClassAttr(bindData , cls.GetType(), property.PropertyType), null);
                }
                else
                {
                    property.SetValue(cls, ResolveNonClassAttr(bindData , cls.GetType(), property.PropertyType), null);
                }
            }

        }

        /// <summary>解决非类类型</summary>
        /// <param name="type">参数类型</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        private object ResolveNonClassAttr(CBindData bindData, Type parent, Type type)
        {
            return null;
        }

        /// <summary>解决类类型</summary>
        /// <param name="type">参数类型</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        private object ResloveClassAttr(CBindData bindData , Type parent, Type type)
        {
            object obj = null;

            if (type == typeof(CConfig))
            {
                if (config == null) { this.InitConfig(); }
                if (config.ContainsKey(parent))
                {
                    obj = config[parent];
                }
            }
            else
            {
                obj = Make(bindData.GetContextual(type.ToString()));
            }

            return obj;
        }

        /// <summary>获取依赖关系</summary>
        /// <param name="type">类型</param>
        /// <param name="paramInfo">参数信息</param>
        /// <param name="param">手动输入的参数</param>
        /// <returns></returns>
        private object[] GetDependencies(CBindData bindData, Type type, List<ParameterInfo> paramInfo, object[] param)
        {
            List<object> myParam = new List<object>(param);

            foreach (ParameterInfo info in paramInfo)
            {
                if (info.ParameterType.IsClass || info.ParameterType.IsInterface)
                {
                    myParam.Add(ResloveClass(bindData , type, info));
                }
                else
                {
                    myParam.Add(ResolveNonClass(bindData , type , info));
                }
            }

            return myParam.ToArray();

        }

        /// <summary>解决非类类型</summary>
        /// <param name="info">参数信息</param>
        /// <returns></returns>
        private object ResolveNonClass(CBindData bindData, Type parent, ParameterInfo info)
        {
            return info.DefaultValue;
        }

        /// <summary>解决类类型</summary>
        /// <param name="bindData"></param>
        /// <param name="parent"></param>
        /// <param name="info">参数信息</param>
        /// <returns></returns>
        private object ResloveClass(CBindData bindData, Type parent, ParameterInfo info)
        {

            object obj = null;

            if (info.ParameterType == typeof(CConfig))
            {
                if (config == null) { this.InitConfig(); }
                if (config.ContainsKey(parent))
                {
                    obj = config[parent];
                }
            }
            else
            {
                obj = this.Make(bindData.GetContextual(info.ParameterType.ToString()), null);
            }

            if (obj == null) { return info.DefaultValue; }
            return obj;
        }


        /// <summary>
        /// 获取别名最终对应的服务名
        /// </summary>
        /// <param name="name">服务名或别名</param>
        /// <returns></returns>
        private string GetAlias(string service)
        {
            if (alias.ContainsKey(service))
            {
                return alias[service];
            }
            return service;
        }

        /// <summary>获取服务绑定数据</summary>
        /// <param name="service">服务名</param>
        /// <returns></returns>
        private CBindData GetBindData(string service)
        {
            if (!binds.ContainsKey(service))
            {
                return new CBindData(this , service, null, false);
            }
            return binds[service];
        }

    }
}