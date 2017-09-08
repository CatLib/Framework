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

namespace CatLib
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
        /// 可以通过服务的真实名字来查找别名
        /// </summary>
        private readonly Dictionary<string, List<string>> aliasesReverse;

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
        /// 类型查询回调
        /// 当类型无法被解决时会尝试去开发者提供的查询器中查询类型
        /// </summary>
        private readonly SortSet<Func<string, Type> , int> findType;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot = new object();

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
            aliasesReverse = new Dictionary<string, List<string>>();
            instances = new Dictionary<string, object>();
            binds = new Dictionary<string, BindData>();
            resolving = new List<Func<IBindData, object, object>>();
            release = new List<Action<IBindData, object>>();
            findType = new SortSet<Func<string, Type>, int>();
            injectTarget = typeof(InjectAttribute);
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
                List<string> list;
                if (tags.TryGetValue(tag, out list))
                {
                    list.AddRange(service);
                }
                else
                {
                    list = new List<string>(service);
                    tags.Add(tag, list);
                }
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
                List<string> serviceList;
                if (!tags.TryGetValue(tag, out serviceList))
                {
                    throw new RuntimeException("Tag [" + tag + "] is not exist.");
                }

                var result = new List<object>();

                foreach (var tagService in serviceList)
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
        /// <exception cref="ArgumentNullException"><paramref name="service"/>为<c>null</c>或者空字符串</exception>
        public IBindData GetBind(string service)
        {
            Guard.NotEmptyOrNull(service, "service");
            lock (syncRoot)
            {
                service = Normalize(service);
                service = AliasToService(service);
                BindData bindData;
                return binds.TryGetValue(service, out bindData) ? bindData : null;
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

            if (alias == service)
            {
                throw new RuntimeException("Alias is Same as Service Name: [" + alias + "].");
            }

            alias = Normalize(alias);
            service = Normalize(service);

            lock (syncRoot)
            {
                if (aliases.ContainsKey(alias))
                {
                    throw new RuntimeException("Alias [" + alias + "] is already exists.");
                }
                if (!binds.ContainsKey(service) && !instances.ContainsKey(service))
                {
                    throw new RuntimeException("You must Bind() or Instance() serivce before you can call Alias().");
                }

                aliases.Add(alias, service);

                List<string> serviceList;
                if (aliasesReverse.TryGetValue(service, out serviceList))
                {
                    serviceList.Add(alias);
                }
                else
                {
                    aliasesReverse.Add(service, new List<string> { alias });
                }
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
        /// <exception cref="ArgumentNullException"><paramref name="concrete"/>为<c>null</c>或者空字符串</exception>
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
                    throw new RuntimeException("Bind [" + service + "] already exists.");
                }

                if (instances.ContainsKey(service))
                {
                    throw new RuntimeException("Instances [" + service + "] is already exists.");
                }

                if (aliases.ContainsKey(service))
                {
                    throw new RuntimeException("Aliase [" + service + "] is already exists.");
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
                var bindData = GetBindData(Type2Service(type));
                param = parameter.Count > 0 ? GetDependencies(bindData, parameter, param) : new object[] { };
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
        /// <exception cref="RuntimeException">出现循环依赖</exception>
        /// <returns>服务实例，如果构造失败那么返回null</returns>
        public object MakeWith(string service, params object[] param)
        {
            Guard.NotEmptyOrNull(service, "service");
            lock (syncRoot)
            {
                service = Normalize(service);
                service = AliasToService(service);

                object instance;
                if (instances.TryGetValue(service, out instance))
                {
                    return instance;
                }

                if (buildStack.Contains(service))
                {
                    throw new RuntimeException("Circular dependency detected while for [" + service + "].");
                }

                buildStack.Push(service);
                try
                {
                    return BuildMake(service, null, true, param);
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
        /// <param name="service">服务名或别名</param>
        /// <exception cref="ArgumentNullException"><paramref name="service"/>为<c>null</c>或者空字符串</exception>
        /// <exception cref="RuntimeException">出现循环依赖</exception>
        /// <returns>服务实例，如果构造失败那么返回null</returns>
        public object Make(string service)
        {
            return MakeWith(service);
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
                service = AliasToService(service);

                var bindData = GetBind(service);
                if (bindData != null)
                {
                    if (!bindData.IsStatic)
                    {
                        throw new RuntimeException("Service [" + service + "] is not Singleton(Static) Bind.");
                    }
                    instance = ((BindData)bindData).ExecResolvingDecorator(instance);
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
            Guard.NotEmptyOrNull(service, "service");
            lock (syncRoot)
            {
                service = Normalize(service);
                service = AliasToService(service);

                object instance;
                if (!instances.TryGetValue(service, out instance))
                {
                    return;
                }

                var bindData = GetBindData(service);
                bindData.ExecReleaseDecorator(instance);
                ExecOnReleaseDecorator(bindData, instance);
                instances.Remove(service);
            }
        }

        /// <summary>
        /// 清空容器的所有实例，绑定，别名，标签，解决器
        /// </summary>
        public void Flush()
        {
            lock (syncRoot)
            {
                var releaseList = new string[instances.Count];
                var i = 0;
                foreach (var instance in instances)
                {
                    releaseList[i++] = instance.Key;
                }
                foreach (var service in releaseList)
                {
                    Release(service);
                }

                binds.Clear();
                instances.Clear();
                aliases.Clear();
                aliasesReverse.Clear();
                tags.Clear();
                resolving.Clear();
                release.Clear();
                findType.Clear();
            }
        }

        /// <summary>
        /// 当查找类型无法找到时会尝试去调用开发者提供的查找类型函数
        /// </summary>
        /// <param name="finder">查找类型的回调</param>
        /// <param name="priority">查询优先级(值越小越优先)</param>
        /// <returns>当前容器实例</returns>
        public IContainer OnFindType(Func<string, Type> finder , int priority = int.MaxValue)
        {
            Guard.NotNull(finder, "finder");
            lock (syncRoot)
            {
                findType.Add(finder, priority);
            }
            return this;
        }

        /// <summary>
        /// 当静态服务被释放时
        /// </summary>
        /// <param name="action">处理释放时的回调</param>
        /// <returns>当前容器实例</returns>
        public IContainer OnRelease(Action<IBindData, object> action)
        {
            Guard.NotNull(action, "action");
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
            Guard.NotNull(func, "func");
            lock (syncRoot)
            {
                resolving.Add(func);

                var result = new Dictionary<string, object>();
                foreach (var data in instances)
                {
                    var bindData = GetBindData(data.Key);
                    result[data.Key] = func.Invoke(bindData, data.Value);
                }
                foreach (var data in result)
                {
                    instances[data.Key] = data.Value;
                }
                result.Clear();
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
                service = AliasToService(service);

                Release(service);
                List<string> serviceList;
                if (aliasesReverse.TryGetValue(service, out serviceList))
                {
                    foreach (var alias in serviceList)
                    {
                        aliases.Remove(alias);
                    }
                    aliasesReverse.Remove(service);
                }
                binds.Remove(service);
            }
        }

        /// <summary>
        /// 将类型转为服务名
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>服务名</returns>
        public string Type2Service(Type type)
        {
            return type.ToString();
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
        private object BuildMake(string makeService, Type makeServiceType, bool isFromMake, params object[] param)
        {
            var bindData = GetBindData(makeService);
            var buildInstance = isFromMake ? BuildUseConcrete(bindData, makeServiceType, param) : Build(bindData, makeServiceType ?? GetType(bindData.Service), param);

            //只有是来自于make函数的调用时才执行di，包装，以及修饰
            if (!isFromMake)
            {
                return buildInstance;
            }

            AttrInject(bindData, buildInstance);

            if (bindData.IsStatic)
            {
                Instance(makeService, buildInstance);
            }
            else
            {
                buildInstance = ExecOnResolvingDecorator(bindData, bindData.ExecResolvingDecorator(buildInstance));
            }

            return buildInstance;
        }

        /// <summary>
        /// 常规编译一个服务
        /// </summary>
        /// <param name="makeServiceBindData">服务绑定数据</param>
        /// <param name="makeServiceType">服务类型</param>
        /// <param name="param">构造参数</param>
        /// <returns>服务实例</returns>
        private object BuildUseConcrete(BindData makeServiceBindData, Type makeServiceType, object[] param)
        {
            if (makeServiceBindData.Concrete != null)
            {
                return makeServiceBindData.Concrete(this, param);
            }
            return BuildMake(makeServiceBindData.Service, makeServiceType, false, param);
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

            if (parameter.Count > 0)
            {
                param = GetDependencies(makeServiceBindData, parameter, param);
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
        /// <param name="makeServiceBindData">服务绑定数据</param>
        /// <param name="makeServiceInstance">服务实例</param>
        /// <returns>服务实例</returns>
        /// <exception cref="RuntimeException">属性是必须的或者注入类型和需求类型不一致</exception>
        private void AttrInject(BindData makeServiceBindData, object makeServiceInstance)
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

                if (!property.IsDefined(injectTarget, false))
                {
                    continue;
                }

                var injectAttr = (InjectAttribute)property.GetCustomAttributes(injectTarget, false)[0];
                var needService = string.IsNullOrEmpty(injectAttr.Alias)
                    ? Type2Service(property.PropertyType)
                    : injectAttr.Alias;
                object instance;
                if (property.PropertyType.IsClass || property.PropertyType.IsInterface)
                {
                    instance = ResloveClassAttr(makeServiceBindData, needService);
                }
                else
                {
                    instance = ResolveNonClassAttr(makeServiceBindData, needService);
                }

                if (injectAttr.Required && instance == null)
                {
                    throw new RuntimeException("[" + makeServiceBindData.Service + "] Attr [" + property.PropertyType + "] Required [" + needService + "] Service.");
                }

                if (instance != null && !property.PropertyType.IsInstanceOfType(instance))
                {
                    throw new RuntimeException("[" + makeServiceBindData.Service + "] Attr inject type must be [" + property.PropertyType + "] , But instance is [" + instance.GetType() + "] , Make service is [" + needService + "].");
                }

                property.SetValue(makeServiceInstance, instance, null);
            }
        }

        /// <summary>
        /// 解决非类类型
        /// </summary>
        /// <param name="makeServiceBindData">请求注入操作的服务绑定数据</param>
        /// <param name="service">希望构造的服务名或者别名</param>
        /// <returns>解决结果</returns>
        private object ResolveNonClassAttr(BindData makeServiceBindData, string service)
        {
            return Make(makeServiceBindData.GetContextual(service));
        }

        /// <summary>
        /// 解决类类型
        /// </summary>
        /// <param name="makeServiceBindData">请求注入操作的服务绑定数据</param>
        /// <param name="service">希望构造的服务名或者别名</param>
        /// <returns>解决结果</returns>
        private object ResloveClassAttr(BindData makeServiceBindData, string service)
        {
            return Make(makeServiceBindData.GetContextual(service));
        }

        /// <summary>
        /// 获取依赖解决结果
        /// </summary>
        /// <param name="makeServiceBindData">服务绑定数据</param>
        /// <param name="paramInfo">服务实例的参数信息</param>
        /// <param name="param">输入的构造参数列表</param>
        /// <returns>服务所需参数的解决结果</returns>
        /// <exception cref="RuntimeException">生成的实例类型和需求类型不一致</exception>
        private object[] GetDependencies(BindData makeServiceBindData, IList<ParameterInfo> paramInfo, IList<object> param)
        {
            var myParam = new List<object>();

            for (var i = 0; i < paramInfo.Count; i++)
            {
                var info = paramInfo[i];
                if (param != null && i < param.Count)
                {
                    if (info.ParameterType.IsInstanceOfType(param[i]))
                    {
                        myParam.Add(param[i]);
                        continue;
                    }
                }

                var needService = Type2Service(info.ParameterType);
                InjectAttribute injectAttr = null;
                if (info.IsDefined(injectTarget, false))
                {
                    var propertyAttrs = info.GetCustomAttributes(injectTarget, false);
                    if (propertyAttrs.Length > 0)
                    {
                        injectAttr = (InjectAttribute)propertyAttrs[0];
                        if (!string.IsNullOrEmpty(injectAttr.Alias))
                        {
                            needService = injectAttr.Alias;
                        }
                    }
                }

                object instance;
                if (info.ParameterType.IsClass || info.ParameterType.IsInterface)
                {
                    instance = ResloveClass(makeServiceBindData, needService);
                }
                else
                {
                    instance = ResolveNonClass(makeServiceBindData, needService);
                }

                if (injectAttr != null && injectAttr.Required && instance == null)
                {
                    throw new RuntimeException("[" + makeServiceBindData.Service + "] Required [" + info.ParameterType + "] Service.");
                }

                if (instance != null && !info.ParameterType.IsInstanceOfType(instance))
                {
                    throw new RuntimeException("[" + makeServiceBindData.Service + "] Attr inject type must be [" + info.ParameterType + "] , But instance is [" + instance.GetType() + "] Make service is [" + needService + "].");
                }

                myParam.Add(instance);
            }

            return myParam.ToArray();
        }

        /// <summary>
        /// 解决非类类型
        /// </summary>
        /// <param name="makeServiceBindData">请求注入操作的服务绑定数据</param>
        /// <param name="service">希望解决的服务名或者别名</param>
        /// <returns>解决结果</returns>
        private object ResolveNonClass(BindData makeServiceBindData, string service)
        {
            return Make(makeServiceBindData.GetContextual(service));
        }

        /// <summary>
        /// 解决类类型
        /// </summary>
        /// <param name="makeServiceBindData">请求注入操作的服务绑定数据</param>
        /// <param name="service">希望解决的服务名或者别名</param>
        /// <returns>解决结果</returns>
        private object ResloveClass(BindData makeServiceBindData, string service)
        {
            return Make(makeServiceBindData.GetContextual(service));
        }

        /// <summary>
        /// 获取别名最终对应的服务名
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>最终映射的服务名</returns>
        private string AliasToService(string service)
        {
            string alias;
            return aliases.TryGetValue(service, out alias) ? alias : service;
        }

        /// <summary>
        /// 获取服务绑定数据(与GetBind的区别是永远不会为null)
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns>服务绑定数据</returns>
        private BindData GetBindData(string service)
        {
            BindData bindData;
            return binds.TryGetValue(service, out bindData) ? bindData : MakeEmptyBindData(service);
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
            foreach (var finder in findType)
            {
                var type = finder.Invoke(service);
                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }
    }
}