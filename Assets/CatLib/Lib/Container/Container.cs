/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using CatLib.API;
using CatLib.API.Container;

namespace CatLib.Container
{
    ///<summary>
    /// 容器
    /// </summary>
    public class Container : IContainer
    {
        /// <summary>
        /// 服务所绑定的相关数据
        /// </summary>
        private Dictionary<string, BindData> binds;

        ///<summary>
        /// 如果所属服务是静态的那么构建后将会储存在这里
        ///</summary>
        private Dictionary<string, object> instances;

        ///<summary>
        /// 服务的别名(key: 别名 , value: 映射的服务名)
        ///</summary>
        private Dictionary<string, string> aliases;

        /// <summary>
        /// 服务标记，一个标记允许标记多个服务
        /// </summary>
        private Dictionary<string, List<string>> tags;

        ///<summary>
        /// 全局类型字典
        ///</summary>
        private Dictionary<string, Type> typeDict;

        /// <summary>
        /// 服务构建时的修饰器
        /// </summary>
        private List<Func<IBindData, object, object>> decorator;

        /// <summary>
        /// locker
        /// </summary>
        private readonly object locker = new object();

        /// <summary>
        /// AOP代理包装器
        /// </summary>
        private IBoundProxy proxy;

        /// <summary>
        /// 构造一个容器
        /// </summary>
        public Container()
        {
            Initialize(); 
        }

        /// <summary>
        /// 为一个及以上的服务定义一个标记
        /// </summary>
        /// <param name="tag">标记名</param>
        /// <param name="service">服务名</param>
        public void Tag(string tag, params string[] service)
        {
            if (service == null)
            {
                return;
            }
            if (service.Length <= 0)
            {
                return;
            }
            if (!tags.ContainsKey(tag))
            {
                tags.Add(tag, new List<string>());
            }
            tags[tag].AddRange(service);
        }

        /// <summary>
        /// 根据标记名生成标记所对应的所有服务实例
        /// </summary>
        /// <param name="tag">标记名</param>
        /// <returns>将会返回标记所对应的所有服务实例</returns>
        public object[] Tagged(string tag)
        {
            if (!tags.ContainsKey(tag))
            {
                return new object[] {};
            }

            var result = new List<object>();

            for (var i = 0; i < tags[tag].Count; ++i)
            {
                result.Add(Make(tags[tag][i]));
            }

            return result.ToArray();
        }

        /// <summary>
        /// 获取服务的绑定数据,如果绑定不存在则返回null
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns>服务绑定数据或者null</returns>
        public IBindData GetBind(string service)
        {
            service = Normalize(service);
            service = GetAlias(service);

            return binds.ContainsKey(service) ? binds[service] : null;
        }

        /// <summary>
        /// 是否已经绑定了服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns>返回一个bool值代表服务是否被绑定</returns>
        public bool HasBind(string service)
        {
            service = Normalize(service);
            return binds.ContainsKey(service) || aliases.ContainsKey(service);
        }

        /// <summary>
        /// 服务是否是静态的
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns>返回一个bool值代表服务是否是静态的,如果服务不存在也将返回false</returns>
        public bool IsStatic(string service)
        {
            if (!HasBind(service))
            {
                return false;
            }

            service = Normalize(service);
            service = GetAlias(service);
            return binds[service].IsStatic;
        }

        /// <summary>
        /// 为服务设定一个别名
        /// </summary>
        /// <param name="alias">别名</param>
        /// <param name="service">映射到的服务名</param>
        /// <returns>当前容器对象</returns>
        public IContainer Alias(string alias, string service)
        {
            lock (locker)
            {
                alias = Normalize(alias);
                service = Normalize(service);
                if (aliases.ContainsKey(alias))
                {
                    throw new CatLibException("alias [" + alias + "] is already exists!");
                }
                if (!binds.ContainsKey(service) && !instances.ContainsKey(service))
                {
                    throw new CatLibException("bind [" + service + "] is not exists!");
                }
                aliases.Add(alias, service);
            }
            return this;
        }

        /// <summary>
        /// 如果服务不存在那么则绑定服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="isStatic">服务是否是静态的</param>
        /// <returns>服务绑定数据</returns>
        public IBindData BindIf(string service, Func<IContainer, object[], object> concrete, bool isStatic)
        {
            var bind = GetBind(service);
            if (bind != null)
            {
                return bind;
            }
            return Bind(service, concrete, isStatic);
        }

        /// <summary>
        /// 如果服务不存在那么则绑定服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="isStatic">服务是否是静态的</param>
        /// <returns>服务绑定数据</returns>
        public IBindData BindIf(string service, string concrete, bool isStatic)
        {
            var bind = GetBind(service);
            if (bind != null)
            {
                return bind;
            }
            return Bind(service, concrete, isStatic);
        }

        /// <summary>
        /// 绑定一个服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="isStatic">服务是否静态化</param>
        /// <returns>服务绑定数据</returns>
        public IBindData Bind(string service, string concrete, bool isStatic)
        {
            service = Normalize(service);
            concrete = Normalize(concrete);
            return Bind(service, (c, param) =>
            {
                var container = c as Container;
                return container == null ? null : container.NormalMake(concrete, false, param);
            }, isStatic);
        }

        /// <summary>
        /// 绑定一个服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实体</param>
        /// <param name="isStatic">服务是否静态化</param>
        /// <returns>服务绑定数据</returns>
        public IBindData Bind(string service, Func<IContainer, object[], object> concrete, bool isStatic)
        {
            lock (locker)
            {
                service = Normalize(service);

                if (binds.ContainsKey(service))
                {
                    throw new CatLibException("bind service [" + service + "] is already exists!");
                }

                instances.Remove(service);
                aliases.Remove(service);

                var bindData = new BindData(this, service, concrete, isStatic);

                binds.Add(service, bindData);

                return bindData;
            }
        }

        /// <summary>
        /// 以依赖注入形式调用一个方法
        /// </summary>
        /// <param name="instance">方法对象</param>
        /// <param name="method">方法名</param>
        /// <param name="param">方法参数</param>
        /// <returns>方法返回值</returns>
        public object Call(object instance, string method, params object[] param)
        {
            if (instance == null)
            {
                throw new RuntimeException("call instance is null");
            }

            var methodInfo = instance.GetType().GetMethod(method);

            return Call(instance, methodInfo, param);
        }

        /// <summary>
        /// 以依赖注入形式调用一个方法
        /// </summary>
        /// <param name="instance">方法对象</param>
        /// <param name="methodInfo">方法信息</param>
        /// <param name="param">方法参数</param>
        /// <returns>方法返回值</returns>
        public object Call(object instance, MethodInfo methodInfo, params object[] param)
        {
            if (instance == null)
            {
                throw new RuntimeException("call instance is null");
            }

            var type = instance.GetType();

            if (methodInfo == null)
            {
                throw new RuntimeException("call method is null");
            }

            var parameter = new List<ParameterInfo>(methodInfo.GetParameters());

            var bindData = GetBindData(type.ToString());
            param = parameter.Count > 0 ? GetDependencies(bindData, type, parameter, param) : new object[] { };

            return methodInfo.Invoke(instance, param);
        }

        /// <summary>
        /// 构造服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <param name="param">构造参数</param>
        /// <returns>服务实例，如果构造失败那么返回null</returns>
        public object Make(string service, params object[] param)
        {
            lock (locker)
            {
                service = Normalize(service);
                service = GetAlias(service);
                return NormalMake(service, true, param);
            }
        }

        /// <summary>
        /// 构造服务
        /// </summary>
        /// <param name="service">服务名或者别名</param>
        /// <returns>服务实例，如果构造失败那么返回null</returns>
        public object this[string service]
        {
            get { return Make(service); }
        }

        /// <summary>
        /// 构造服务
        /// </summary>
        /// <param name="service">服务类型</param>
        /// <returns>服务实例，如果构造失败那么返回null</returns>
        public object this[Type service]
        {
            get { return Make(service.ToString()); }
        }

        /// <summary>
        /// 静态化一个服务
        /// </summary>
        /// <param name="service">服务名或者别名</param>
        /// <param name="objectData">服务实例</param>
        public void Instance(string service, object objectData)
        {
            lock (locker)
            {
                service = Normalize(service);
                service = GetAlias(service);

                if (objectData == null)
                {
                    instances.Remove(service);
                    return;
                }

                if (instances.ContainsKey(service))
                {
                    instances.Remove(service);
                }

                instances.Add(service, objectData);
            }
        }

        /// <summary>
        /// 当服务被解决时触发的事件
        /// </summary>
        /// <param name="func">回调函数</param>
        /// <returns>当前容器对象</returns>
        public IContainer OnResolving(Func<IBindData, object, object> func)
        {
            lock (locker)
            {
                if (decorator == null)
                {
                    decorator = new List<Func<IBindData, object, object>>();
                }
                decorator.Add(func);
                foreach (var data in instances)
                {
                    var bindData = GetBindData(data.Key);
                    instances[data.Key] = func(bindData, data.Value);
                }
                return this;
            }
        }

        /// <summary>
        /// 执行全局修饰器
        /// </summary>
        /// <param name="bindData">服务绑定数据</param>
        /// <param name="obj">服务实例</param>
        /// <returns>被修饰器修饰后的服务实例</returns>
        private object ExecDecorator(BindData bindData, object obj)
        {
            if (decorator != null)
            {
                foreach (var func in decorator)
                {
                    obj = func(bindData, obj);
                }
            }
            return obj;
        }

        /// <summary>
        /// 构造服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="isFromMake">是否直接调用自Make函数</param>
        /// <param name="param">构造参数</param>
        /// <returns>服务实例</returns>
        private object NormalMake(string service, bool isFromMake, params object[] param)
        {
            if (instances.ContainsKey(service))
            {
                return instances[service];
            }

            var bindData = GetBindData(service);
            var objectData = isFromMake ? NormalBuild(bindData, param) : Build(bindData, service, param);

            //只有是来自于make函数的调用时才执行di
            if (!isFromMake)
            {
                return objectData;
            }

            DIAttr(bindData, objectData);

            if (proxy != null)
            {
                objectData = proxy.Bound(objectData, bindData);
            }

            objectData = ExecDecorator(bindData, bindData.ExecDecorator(objectData));

            if (bindData.IsStatic)
            {
                Instance(service, objectData);
            }

            return objectData;
        }

        /// <summary>
        /// 常规编译一个服务
        /// </summary>
        /// <param name="bindData">服务绑定数据</param>
        /// <param name="param">构造参数</param>
        /// <returns>服务实例</returns>
        private object NormalBuild(BindData bindData, object[] param)
        {
            if (bindData.Concrete != null)
            {
                return bindData.Concrete(this, param);
            }
            return NormalMake(bindData.Service, false, param); //Build(bindData , bindData.Service, param); 这句语句之前导致了没有正确给定注入实体。但是我们先保留一段时间后再删除
        }

        /// <summary>
        /// 构造服务 - 实现
        /// </summary>
        /// <param name="bindData">服务绑定数据</param>
        /// <param name="service">服务名</param>
        /// <param name="param">构造参数</param>
        /// <returns>服务实例</returns>
        private object Build(BindData bindData, string service, object[] param)
        {
            if (param == null)
            {
                param = new object[] {};
            }

            var type = GetType(bindData.Service);

            if (type == null)
            {
                return null;
            }

            if (type.IsAbstract || type.IsInterface)
            {
                if (service != bindData.Service)
                {
                    type = Type.GetType(service);
                    if (type == null)
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            var constructor = type.GetConstructors();
            if (constructor.Length <= 0)
            {
                return Activator.CreateInstance(type);
            }

            var parameter = new List<ParameterInfo>(constructor[constructor.Length - 1].GetParameters());
            parameter.RemoveRange(0, param.Length);

            if (parameter.Count > 0)
            {
                param = GetDependencies(bindData, type, parameter, param);
            }

            return Activator.CreateInstance(type, param);
        }

        /// <summary>
        /// 标准化服务名
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns>标准化后的服务名</returns>
        private string Normalize(string service)
        {
            return service.Trim();
        }

        /// <summary>
        /// 属性注入
        /// </summary>
        /// <param name="bindData">服务绑定数据</param>
        /// <param name="obj">服务实例</param>
        /// <returns>服务实例</returns>
        private void DIAttr(BindData bindData, object obj)
        {
            if (obj == null)
            {
                return;
            }

            string typeName;
            foreach (var property in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanWrite)
                {
                    continue;
                }
                var propertyAttrs = property.GetCustomAttributes(typeof(DependencyAttribute), true);
                if (propertyAttrs.Length <= 0)
                {
                    continue;
                }

                var dependency = propertyAttrs[0] as DependencyAttribute;

                typeName = string.IsNullOrEmpty(dependency.Alias) ? property.PropertyType.ToString() : GetAlias(dependency.Alias);

                if (property.PropertyType.IsClass || property.PropertyType.IsInterface)
                {
                    property.SetValue(obj, ResloveClassAttr(bindData, obj.GetType(), typeName), null);
                }
                else
                {
                    property.SetValue(obj, ResolveNonClassAttr(bindData, obj.GetType(), typeName), null);
                }
            }
        }

        /// <summary>
        /// 解决非类类型
        /// </summary>
        /// <param name="bindData">请求注入操作的服务绑定数据</param>
        /// <param name="parent">请求注入操作的服务实例的类型</param>
        /// <param name="service">希望构造的服务名或者别名</param>
        /// <returns>解决结果</returns>
        private object ResolveNonClassAttr(BindData bindData, Type parent, string service)
        {
            return null;
        }

        /// <summary>
        /// 解决类类型
        /// </summary>
        /// <param name="bindData">请求注入操作的服务绑定数据</param>
        /// <param name="parent">请求注入操作的服务实例的类型</param>
        /// <param name="service">希望构造的服务名或者别名</param>
        /// <returns>解决结果</returns>
        private object ResloveClassAttr(BindData bindData, Type parent, string service)
        {
            return Make(bindData.GetContextual(service)); ;
        }

        /// <summary>
        /// 获取依赖解决结果
        /// </summary>
        /// <param name="bindData">服务绑定数据</param>
        /// <param name="type">服务实例的类型</param>
        /// <param name="paramInfo">服务实例的参数信息</param>
        /// <param name="param">输入的构造参数列表</param>
        /// <returns>服务所需参数的解决结果</returns>
        private object[] GetDependencies(BindData bindData, Type type, IList<ParameterInfo> paramInfo, IList<object> param)
        {
            var  myParam = new List<object>();

            ParameterInfo info;
            for (var i = 0; i < paramInfo.Count; i++)
            {
                info = paramInfo[i];
                if (param != null && i < param.Count)
                {
                    if (param[i] == null || info.ParameterType.IsInstanceOfType(param[i]))
                    {
                        myParam.Add(param[i]);
                        continue;
                    }
                }

                if (info.ParameterType.IsClass || info.ParameterType.IsInterface)
                {
                    myParam.Add(ResloveClass(bindData, type, info));
                }
                else
                {
                    myParam.Add(ResolveNonClass(bindData, type, info));
                }
            }

            return myParam.ToArray();
        }

        /// <summary>
        /// 解决非类类型
        /// </summary>
        /// <param name="bindData">请求注入操作的服务绑定数据</param>
        /// <param name="parent">请求注入操作的服务实例的类型</param>
        /// <param name="info">参数信息</param>
        /// <returns>解决结果</returns>
        private object ResolveNonClass(BindData bindData, Type parent, ParameterInfo info)
        {
            return info.DefaultValue;
        }

        /// <summary>
        /// 解决类类型
        /// </summary>
        /// <param name="bindData">请求注入操作的服务绑定数据</param>
        /// <param name="parent">请求注入操作的服务实例的类型</param>
        /// <param name="info">参数信息</param>
        /// <returns>解决结果</returns>
        private object ResloveClass(BindData bindData, Type parent, ParameterInfo info)
        {
            return Make(bindData.GetContextual(info.ParameterType.ToString()), null);
        }

        /// <summary>
        /// 获取别名最终对应的服务名
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>最终映射的服务名</returns>
        private string GetAlias(string service)
        {
            return aliases.ContainsKey(service) ? aliases[service] : service;
        }

        /// <summary>
        /// 获取服务绑定数据
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns>服务绑定数据</returns>
        private BindData GetBindData(string service)
        {
            return !binds.ContainsKey(service) ? new BindData(this, service, null, false) : binds[service];
        }

        /// <summary>
        /// 获取类型映射
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns>服务类型</returns>
        private Type GetType(string service)
        {
            return typeDict.ContainsKey(service) ? typeDict[service] : Type.GetType(service);
        }

        /// <summary>
        /// 初始化容器
        /// </summary>
        private void Initialize()
        {
            tags = new Dictionary<string, List<string>>();
            aliases = new Dictionary<string, string>();
            typeDict = new Dictionary<string, Type>();
            instances = new Dictionary<string, object>();
            binds = new Dictionary<string, BindData>();
            decorator = new List<Func<IBindData, object, object>>();
            proxy = new BoundProxy();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!typeDict.ContainsKey(type.ToString()))
                    {
                        typeDict.Add(type.ToString(), type);
                    }
                }
            }
        }
    }
}