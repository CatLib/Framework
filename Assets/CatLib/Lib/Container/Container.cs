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
    /// 依赖注入容器
    /// </summary>
    public class Container : IContainer
    {
        /// <summary>
        /// 服务所绑定的相关数据，记录了服务的关系
        /// </summary>
        private readonly Dictionary<string, BindData> binds;

        ///<summary>
        /// 如果所属服务是静态的那么构建后将会储存在这里
        ///</summary>
        private readonly Dictionary<string, object> instances;

        ///<summary>
        /// 服务的别名(key: 别名 , value: 映射的服务名)
        ///</summary>
        private readonly Dictionary<string, string> aliases;

        /// <summary>
        /// 服务标记，一个标记允许标记多个服务
        /// </summary>
        private readonly Dictionary<string, List<string>> tags;

        /// <summary>
        /// 服务构建时的修饰器
        /// </summary>
        private readonly List<Func<IBindData, object, object>> resolving;

        /// <summary>
        /// 静态服务释放时的修饰器
        /// </summary>
        private readonly List<Action<IBindData, object>> release;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// AOP代理包装器
        /// </summary>
        private readonly IBoundProxy proxy;

        /// <summary>
        /// 注入目标
        /// </summary>
        private readonly Type injectTarget;

        /// <summary>
        /// 编译堆栈
        /// </summary>
        private readonly Stack<string> buildStack;

        /// <summary>
        /// 构造一个容器
        /// </summary>
        public Container()
        {
            tags = new Dictionary<string, List<string>>();
            aliases = new Dictionary<string, string>();
            instances = new Dictionary<string, object>();
            binds = new Dictionary<string, BindData>();
            resolving = new List<Func<IBindData, object, object>>();
            release = new List<Action<IBindData, object>>();
            proxy = new BoundProxy();
            injectTarget = typeof(DependencyAttribute);
            buildStack = new Stack<string>();
        }

        /// <summary>
        /// 为一个及以上的服务定义一个标记
        /// 如果标记已经存在那么服务会被追加进列表
        /// </summary>
        /// <param name="tag">标记名</param>
        /// <param name="service">服务名或者别名</param>
        /// <exception cref="ArgumentNullException"><paramref name="service"/>为<c>null</c>或者<paramref name="service"/>中的元素为<c>null</c>或者空字符串</exception>
        public void Tag(string tag, params string[] service)
        {
            Guard.NotEmptyOrNull(tag, "tag");
            Guard.NotNull(service, "service");
            Guard.CountGreaterZero(service, "service");
            Guard.ElementNotEmptyOrNull(service, "service");

            lock (syncRoot)
            {
                if (!tags.ContainsKey(tag))
                {
                    tags.Add(tag, new List<string>());
                }
                tags[tag].AddRange(service);
            }
        }

        /// <summary>
        /// 根据标记名生成标记所对应的所有服务实例
        /// </summary>
        /// <param name="tag">标记名</param>
        /// <returns>将会返回标记所对应的所有服务实例</returns>
        /// <exception cref="RuntimeException"><paramref name="tag"/>不存在</exception>
        /// <exception cref="ArgumentNullException"><paramref name="tag"/>为<c>null</c>或者空字符串</exception>
        public object[] Tagged(string tag)
        {
            Guard.NotEmptyOrNull(tag, "tag");
            lock (syncRoot)
            {
                if (!tags.ContainsKey(tag))
                {
                    throw new RuntimeException("Tag [" + tag + "] is not exist.");
                }

                var result = new List<object>();

                foreach (var tagService in tags[tag])
                {
                    result.Add(Make(tagService));
                }

                return result.ToArray();
            }
        }

        /// <summary>
        /// 获取服务的绑定数据,如果绑定不存在则返回null（只有进行过bind才视作绑定）
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>服务绑定数据或者null</returns>
        /// <exception cref="service"><paramref name="service"/>为<c>null</c>或者空字符串</exception>
        public IBindData GetBind(string service)
        {
            Guard.NotEmptyOrNull(service, "service");
            lock (syncRoot)
            {
                service = Normalize(service);
                service = GetAlias(service);
                return binds.ContainsKey(service) ? binds[service] : null;
            }
        }

        /// <summary>
        /// 是否已经绑定了服务（只有进行过bind才视作绑定）
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>服务是否被绑定</returns>
        public bool HasBind(string service)
        {
            return GetBind(service) != null;
        }

        /// <summary>
        /// 服务是否是静态化的,如果服务不存在也将返回false
        /// </summary>
        /// <param name="service">服务名或者别名</param>
        /// <returns>是否是静态化的</returns>
        public bool IsStatic(string service)
        {
            var bind = GetBind(service);
            return bind != null && bind.IsStatic;
        }

        /// <summary>
        /// 为服务设定一个别名
        /// </summary>
        /// <param name="alias">别名</param>
        /// <param name="service">映射到的服务名</param>
        /// <returns>当前容器对象</returns>
        /// <exception cref="RuntimeException"><paramref name="alias"/>别名冲突或者<paramref name="service"/>的绑定与实例都不存在</exception>
        /// <exception cref="ArgumentNullException"><paramref name="alias"/>,<paramref name="service"/>为<c>null</c>或者空字符串</exception>
        public IContainer Alias(string alias, string service)
        {
            Guard.NotEmptyOrNull(alias, "alias");
            Guard.NotEmptyOrNull(service, "service");

            alias = Normalize(alias);
            service = Normalize(service);

            lock (syncRoot)
            {
                if (aliases.ContainsKey(alias))
                {
                    throw new RuntimeException("[" + alias + "] in Alias is already exists.");
                }
                if (!binds.ContainsKey(service) && !instances.ContainsKey(service))
                {
                    throw new RuntimeException("[" + service + "] must be Bind or Instance.");
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
            return bind ?? Bind(service, concrete, isStatic);
        }

        /// <summary>
        /// 如果服务不存在那么则绑定服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="isStatic">服务是否是静态的</param>
        /// <returns>服务绑定数据</returns>
        public IBindData BindIf(string service, Type concrete, bool isStatic)
        {
            var bind = GetBind(service);
            return bind ?? Bind(service, concrete, isStatic);
        }

        /// <summary>
        /// 绑定一个服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="isStatic">服务是否静态化</param>
        /// <returns>服务绑定数据</returns>
        /// <exception cref="concrete"><paramref name="concrete"/>为<c>null</c>或者空字符串</exception>
        public IBindData Bind(string service, Type concrete, bool isStatic)
        {
            Guard.NotNull(concrete, "concrete");
            return Bind(service, (c, param) =>
            {
                var container = (Container)c;
                return container.BuildMake(service, concrete, false, param);
            }, isStatic);
        }

        /// <summary>
        /// 绑定一个服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="isStatic">服务是否静态化</param>
        /// <returns>服务绑定数据</returns>
        /// <exception cref="RuntimeException"><paramref name="service"/>绑定冲突</exception>
        /// <exception cref="ArgumentNullException"><paramref name="concrete"/>为<c>null</c></exception>
        public IBindData Bind(string service, Func<IContainer, object[], object> concrete, bool isStatic)
        {
            Guard.NotEmptyOrNull(service, "service");
            Guard.NotNull(concrete, "concrete");
            service = Normalize(service);
            lock (syncRoot)
            {
                if (binds.ContainsKey(service))
                {
                    throw new RuntimeException("[" + service + "] in Bind is already exists.");
                }

                if (instances.ContainsKey(service))
                {
                    throw new RuntimeException("[" + service + "] in Instances is already exists.");
                }

                if (aliases.ContainsKey(service))
                {
                    throw new RuntimeException("[" + service + "] in Aliase is already exists.");
                }

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
        /// <param name="param">附加的参数</param>
        /// <returns>方法返回值</returns>
        /// <exception cref="ArgumentNullException"><paramref name="instance"/>,<paramref name="method"/>为<c>null</c>或者空字符串</exception>
        public object Call(object instance, string method, params object[] param)
        {
            Guard.NotNull(instance, "instance");
            Guard.NotEmptyOrNull(method, "method");

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
        /// <exception cref="ArgumentNullException"><paramref name="instance"/>,<paramref name="methodInfo"/>为<c>null</c></exception>
        public object Call(object instance, MethodInfo methodInfo, params object[] param)
        {
            Guard.NotNull(instance, "instance");
            Guard.NotNull(methodInfo, "methodInfo");

            var type = instance.GetType();
            var parameter = new List<ParameterInfo>(methodInfo.GetParameters());

            lock (syncRoot)
            {
                var bindData = GetBindData(type.ToString());
                param = parameter.Count > 0 ? GetDependencies(bindData, type, parameter, param) : new object[] { };
                return methodInfo.Invoke(instance, param);
            }
        }

        /// <summary>
        /// 构造服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <param name="param">构造参数</param>
        /// <returns>服务实例，如果构造失败那么返回null</returns>
        /// <exception cref="ArgumentNullException"><paramref name="service"/>为<c>null</c>或者空字符串</exception>
        public object Make(string service, params object[] param)
        {
            Guard.NotEmptyOrNull(service, "service");
            lock (syncRoot)
            {
                service = Normalize(service);
                service = GetAlias(service);

                if (buildStack.Contains(service))
                {
                    throw new RuntimeException("Circular dependency detected while for [" + service + "]");
                }

                buildStack.Push(service);
                try
                {
                    return instances.ContainsKey(service) ? instances[service] : BuildMake(service, null, true, param);
                }
                finally
                {
                    buildStack.Pop();
                }
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
        /// 静态化一个服务,实例值会经过解决修饰器
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <param name="instance">服务实例，<c>null</c>也是合法的实例值</param>
        /// <exception cref="ArgumentNullException"><paramref name="service"/>为<c>null</c>或者空字符串</exception>
        /// <exception cref="RuntimeException"><paramref name="service"/>的服务在绑定设置中不是静态的</exception>
        public void Instance(string service, object instance)
        {
            Guard.NotEmptyOrNull(service, "service");
            lock (syncRoot)
            {
                service = Normalize(service);
                service = GetAlias(service);

                var bindData = GetBind(service);
                if (bindData != null)
                {
                    if (!bindData.IsStatic)
                    {
                        throw new RuntimeException("[" + service + "] is not Static bind.");
                    }
                    instance = ((BindData)bindData).ExecDecorator(instance);
                }
                else
                {
                    bindData = MakeEmptyBindData(service);
                }

                Release(service);

                instance = ExecOnResolvingDecorator(bindData, instance);
                instances.Add(service, instance);
            }
        }

        /// <summary>
        /// 释放静态化实例
        /// </summary>
        /// <param name="service">服务名或别名</param>
        public void Release(string service)
        {
            lock (syncRoot)
            {
                service = Normalize(service);
                service = GetAlias(service);

                if (!instances.ContainsKey(service))
                {
                    return;
                }

                var bindData = GetBindData(service);
                ExecOnReleaseDecorator(bindData, instances[service]);
                instances.Remove(service);
            }
        }

        /// <summary>
        /// 当静态服务被释放时
        /// </summary>
        /// <param name="action">处理释放时的回调</param>
        public IContainer OnRelease(Action<IBindData, object> action)
        {
            lock (syncRoot)
            {
                release.Add(action);
            }
            return this;
        }

        /// <summary>
        /// 当服务被解决时，生成的服务会经过注册的回调函数
        /// </summary>
        /// <param name="func">回调函数</param>
        /// <returns>当前容器对象</returns>
        public IContainer OnResolving(Func<IBindData, object, object> func)
        {
            lock (syncRoot)
            {
                resolving.Add(func);
                foreach (var data in instances)
                {
                    var bindData = GetBindData(data.Key);
                    instances[data.Key] = func.Invoke(bindData, data.Value);
                }
            }
            return this;
        }

        /// <summary>
        /// 解除绑定服务
        /// </summary>
        /// <param name="service">服务名或者别名</param>
        internal void UnBind(string service)
        {
            lock (syncRoot)
            {
                service = Normalize(service);
                service = GetAlias(service);

                Release(service);
                aliases.Remove(service);
                binds.Remove(service);
            }
        }

        /// <summary>
        /// 执行全局解决修饰器
        /// </summary>
        /// <param name="bindData">服务绑定数据</param>
        /// <param name="obj">服务实例</param>
        /// <returns>被修饰器修饰后的服务实例</returns>
        private object ExecOnResolvingDecorator(IBindData bindData, object obj)
        {
            foreach (var func in resolving)
            {
                obj = func(bindData, obj);
            }
            return obj;
        }

        /// <summary>
        /// 执行全局释放修饰器
        /// </summary>
        /// <param name="bindData">服务绑定数据</param>
        /// <param name="obj">服务实例</param>
        /// <returns>被修饰器修饰后的服务实例</returns>
        private void ExecOnReleaseDecorator(IBindData bindData, object obj)
        {
            foreach (var action in release)
            {
                action.Invoke(bindData, obj);
            }
        }

        /// <summary>
        /// 构造服务
        /// </summary>
        /// <param name="makeService">服务名</param>
        /// <param name="makeServiceType">服务类型</param>
        /// <param name="isFromMake">是否直接调用自Make函数</param>
        /// <param name="param">构造参数</param>
        /// <returns>服务实例</returns>
        private object BuildMake(string makeService , Type makeServiceType, bool isFromMake, params object[] param)
        {
            var bindData = GetBindData(makeService);
            var objectData = isFromMake ? BuildUseConcrete(bindData, param) : Build(bindData, makeServiceType ?? GetType(bindData.Service), param);

            //只有是来自于make函数的调用时才执行di，包装，以及修饰
            if (!isFromMake)
            {
                return objectData;
            }

            AttrInject(bindData, objectData);

            if (proxy != null)
            {
                objectData = proxy.Bound(objectData, bindData);
            }

            if (bindData.IsStatic)
            {
                Instance(makeService, objectData);
            }

            return objectData;
        }

        /// <summary>
        /// 常规编译一个服务
        /// </summary>
        /// <param name="makeServiceBindData">服务绑定数据</param>
        /// <param name="param">构造参数</param>
        /// <returns>服务实例</returns>
        private object BuildUseConcrete(BindData makeServiceBindData, object[] param)
        {
            if (makeServiceBindData.Concrete != null)
            {
                return makeServiceBindData.Concrete(this, param);
            }
            return BuildMake(makeServiceBindData.Service , null, false, param);
        }

        /// <summary>
        /// 构造服务 - 实现
        /// </summary>
        /// <param name="makeServiceBindData">服务绑定数据</param>
        /// <param name="makeServiceType">服务类型</param>
        /// <param name="param">构造参数</param>
        /// <returns>服务实例</returns>
        private object Build(BindData makeServiceBindData, Type makeServiceType, object[] param)
        {
            param = param ?? new object[] { };

            if (makeServiceType == null)
            {
                return null;
            }

            if (makeServiceType.IsAbstract || makeServiceType.IsInterface)
            {
                return null;
            }

            var constructor = makeServiceType.GetConstructors();
            if (constructor.Length <= 0)
            {
                return Activator.CreateInstance(makeServiceType);
            }

            var parameter = new List<ParameterInfo>(constructor[constructor.Length - 1].GetParameters());
            parameter.RemoveRange(0, param.Length);

            if (parameter.Count > 0)
            {
                param = GetDependencies(makeServiceBindData, makeServiceType, parameter, param);
            }

            return Activator.CreateInstance(makeServiceType, param);
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
        /// <param name="makeSerivceBindData">服务绑定数据</param>
        /// <param name="makeServiceInstance">服务实例</param>
        /// <returns>服务实例</returns>
        /// <exception cref="RuntimeException">属性是必须的或者注入类型和需求类型不一致</exception>
        private void AttrInject(BindData makeSerivceBindData, object makeServiceInstance)
        {
            if (makeServiceInstance == null)
            {
                return;
            }

            foreach (var property in makeServiceInstance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanWrite)
                {
                    continue;
                }

                var propertyAttrs = property.GetCustomAttributes(injectTarget, true);
                if (propertyAttrs.Length <= 0)
                {
                    continue;
                }

                var dependency = (DependencyAttribute)propertyAttrs[0];
                var typeName = string.IsNullOrEmpty(dependency.Alias) ? property.PropertyType.ToString() : GetAlias(dependency.Alias);
                object instance;
                if (property.PropertyType.IsClass || property.PropertyType.IsInterface)
                {
                    instance = ResloveClassAttr(makeSerivceBindData, makeServiceInstance.GetType(), typeName);
                }
                else
                {
                    instance = ResolveNonClassAttr(makeSerivceBindData, makeServiceInstance.GetType(), typeName);
                }

                if (dependency.Required && instance == null)
                {
                    throw new RuntimeException("Attr required ["+ makeSerivceBindData.Service + "] service");
                }

                if (instance != null && instance.GetType() != property.PropertyType)
                {
                    throw new RuntimeException("Attr type is not same,injection type must be [" + property.PropertyType + "],Make Service [" + makeSerivceBindData.Service + "],Attr Target Service [" + typeName + "]");
                }

                property.SetValue(makeServiceInstance, instance, null);
            }
        }

        /// <summary>
        /// 解决非类类型
        /// </summary>
        /// <param name="makeServiceBindData">请求注入操作的服务绑定数据</param>
        /// <param name="makeServiceType">请求注入操作的服务实例的类型</param>
        /// <param name="service">希望构造的服务名或者别名</param>
        /// <returns>解决结果</returns>
        private object ResolveNonClassAttr(BindData makeServiceBindData, Type makeServiceType, string service)
        {
            return null;
        }

        /// <summary>
        /// 解决类类型
        /// </summary>
        /// <param name="makeServiceBindData">请求注入操作的服务绑定数据</param>
        /// <param name="makeServiceType">请求注入操作的服务实例的类型</param>
        /// <param name="service">希望构造的服务名或者别名</param>
        /// <returns>解决结果</returns>
        private object ResloveClassAttr(BindData makeServiceBindData, Type makeServiceType, string service)
        {
            return Make(makeServiceBindData.GetContextual(service)); ;
        }

        /// <summary>
        /// 获取依赖解决结果
        /// </summary>
        /// <param name="makeServiceBindData">服务绑定数据</param>
        /// <param name="makeServiceType">服务实例的类型</param>
        /// <param name="paramInfo">服务实例的参数信息</param>
        /// <param name="param">输入的构造参数列表</param>
        /// <returns>服务所需参数的解决结果</returns>
        private object[] GetDependencies(BindData makeServiceBindData, Type makeServiceType, IList<ParameterInfo> paramInfo, IList<object> param)
        {
            var myParam = new List<object>();

            for (var i = 0; i < paramInfo.Count; i++)
            {
                var info = paramInfo[i];
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
                    myParam.Add(ResloveClass(makeServiceBindData, makeServiceType, info));
                }
                else
                {
                    myParam.Add(ResolveNonClass(makeServiceBindData, makeServiceType, info));
                }
            }

            return myParam.ToArray();
        }

        /// <summary>
        /// 解决非类类型
        /// </summary>
        /// <param name="makeServiceBindData">请求注入操作的服务绑定数据</param>
        /// <param name="makeServiceType">请求注入操作的服务实例的类型</param>
        /// <param name="info">参数信息</param>
        /// <returns>解决结果</returns>
        private object ResolveNonClass(BindData makeServiceBindData, Type makeServiceType, ParameterInfo info)
        {
            return info.DefaultValue;
        }

        /// <summary>
        /// 解决类类型
        /// </summary>
        /// <param name="makeServiceBindData">请求注入操作的服务绑定数据</param>
        /// <param name="makeServiceType">请求注入操作的服务实例的类型</param>
        /// <param name="info">参数信息</param>
        /// <returns>解决结果</returns>
        private object ResloveClass(BindData makeServiceBindData, Type makeServiceType, ParameterInfo info)
        {
            return Make(makeServiceBindData.GetContextual(info.ParameterType.ToString()));
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
        /// 获取服务绑定数据(与GetBind的区别是永远不会为null)
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns>服务绑定数据</returns>
        private BindData GetBindData(string service)
        {
            return !binds.ContainsKey(service) ? MakeEmptyBindData(service) : binds[service];
        }

        /// <summary>
        /// 制作一个空的绑定数据
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns>空绑定数据</returns>
        private BindData MakeEmptyBindData(string service)
        {
            return new BindData(this, service, null, false);
        }

        /// <summary>
        /// 获取类型映射
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns>服务类型</returns>
        private Type GetType(string service)
        {
            return Type.GetType(service);
        }
    }
}