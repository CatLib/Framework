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
using System.Reflection;
using System.Collections.Generic;
using CatLib.API.Container;
using CatLib.API;

namespace CatLib.Container
{
    ///<summary>容器</summary>
    public class Container : MonoComponent, IContainer
    {

        /// <summary>
        /// 绑定数据
        /// </summary>
        private Dictionary<string, BindData> binds = new Dictionary<string, BindData>();

        ///<summary>
        /// 静态化内容
        ///</summary>
        private Dictionary<string, object> instances = new Dictionary<string, object>();

        ///<summary>
        /// 别名(key: 别名 , value: 服务名)
        ///</summary>
        private Dictionary<string, string> alias = new Dictionary<string, string>();

        /// <summary>
        /// 标记
        /// </summary>
        private Dictionary<string, List<string>> tags = new Dictionary<string, List<string>>();

        ///<summary>
        /// 类型字典
        ///</summary>
        private Dictionary<string, Type> typeDict = new Dictionary<string, Type>();

        /// <summary>
        /// 修饰器
        /// </summary>
        private List<Func<IContainer, IBindData, object, object>> decorator = new List<Func<IContainer, IBindData, object, object>>();

        /// <summary>
        /// 锁定器
        /// </summary>
        private readonly object locker = new object();

        public Container()
        {
            //获取已加载到当前应用程序域的执行上下文中的程序集 列表 长度
            for (int i = 0; i < AppDomain.CurrentDomain.GetAssemblies().Length; i++)
            {
                // 获取已经加载到当前应用程序域的单个程序集
                var assembly = AppDomain.CurrentDomain.GetAssemblies()[i];
                //  获取程序集中的类型列表
                Type[] types = assembly.GetTypes();
                for (int index = 0; index < types.Length; index++)
                {
                    // 获取单个类型
                    var type = types[index];
                    if (!typeDict.ContainsKey(type.ToString()))
                    {
                        // 添加类型到类型字典
                        typeDict.Add(type.ToString(), type);
                    }
                }
            }
        }

        /// <summary>
        /// 为一个及以上的服务定义一个标记
        /// </summary>
        /// <param name="tag">标记名</param>
        /// <param name="service">服务名</param>
        public void Tag(string tag, params string[] service)
        {
            // 判断可变参数不为空
            if (service != null)
            {
                // 长度小于0
                if (service.Length <= 0)
                {
                    return;
                }
                // 检查是否有这个Key
                if (tags.ContainsKey(tag))
                {
                    // 存在Key 
                    // 检测Values 是否为空,
                    List<string> tempList = tags[tag];
                    if (tempList == null)
                    {
                        // Value 为空则构建新的值
                        tags[tag] = new List<string>();
                    }
                    // 添加值到Value 集合
                    tags[tag].AddRange(service);
                }
                else
                {
                    tags.Add(tag, new List<string>());
                }
            }
        }

        /// <summary>
        /// 根据标记名生成对应的所有服务
        /// </summary>
        /// <param name="tag">标记名</param>
        /// <returns></returns>
        public object[] Tagged(string tag)
        {
            if (!tags.ContainsKey(tag))
            {
                return new object[] { };
            }

            List<object> result = new List<object>();

            for (int i = 0; i < tags[tag].Count; ++i)
            {
                result.Add(Make(tags[tag][i]));
            }

            return result.ToArray();

        }

        /// <summary>
        /// 获取服务的绑定数据(如果绑定不存在则返回null)
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns></returns>
        public IBindData GetBind(string service)
        {
            service = Normalize(service);
            service = GetAlias(service);

            if (binds.ContainsKey(service))
            {
                return binds[service];
            }
            return null;
        }

        /// <summary>
        /// 是否已经绑定了给定名字的服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns></returns>
        public bool HasBind(string serviceName)
        {
            serviceName = Normalize(serviceName);
            if (binds.ContainsKey(serviceName) || alias.ContainsKey(serviceName))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 给定的服务是否是一个静态服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns></returns>
        public bool IsStatic(string serviceName)
        {
            if (!HasBind(serviceName))
            {
                return false;
            }

            serviceName = Normalize(serviceName);
            serviceName = GetAlias(serviceName);
            return binds[serviceName].IsStatic;
        }

        /// <summary>
        /// 设定一个别名
        /// </summary>
        /// <param name="aliasName">别名</param>
        /// <param name="serviceName">指向的服务</param>
        /// <returns></returns>
        public IContainer Alias(string aliasName, string serviceName)
        {
            lock (locker)
            {
                aliasName = Normalize(aliasName);
                serviceName = Normalize(serviceName);
                if (this.alias.ContainsKey(aliasName))
                {
                    this.alias.Remove(aliasName);
                }

                this.alias.Add(aliasName, serviceName);
            }
            return this;
        }

        /// <summary>
        /// 如果服务不存在那么绑定
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <param name="concrete">服务实体</param>
        /// <param name="isStatic">服务是否是静态的</param>
        /// <returns></returns>
        public IBindData BindIf(string serviceName, Func<IContainer, object[], object> concrete, bool isStatic)
        {
            var bind = GetBind(serviceName);
            if (bind != null)
            {
                return bind;
            }
            return Bind(serviceName, concrete, isStatic);
        }

        /// <summary>
        /// 如果服务不存在那么绑定
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <param name="concrete">服务实体</param>
        /// <param name="isStatic">服务是否是静态的</param>
        /// <returns></returns>
        public IBindData BindIf(string serviceName, string concrete, bool isStatic)
        {
            var bind = GetBind(serviceName);
            if (bind != null)
            {
                return bind;
            }
            return Bind(serviceName, concrete, isStatic);
        }

        /// <summary>
        /// 绑定一个服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <param name="concrete">服务实体</param>
        /// <param name="isStatic">服务是否静态化</param>
        /// <returns></returns>
        public IBindData Bind(string serviceName, string concrete, bool isStatic)
        {
            serviceName = Normalize(serviceName);
            concrete = Normalize(concrete);
            return Bind(serviceName, (c, param) =>
            {
                Container container = c as Container;
                return container.NormalMake(concrete, false, param);
            }, isStatic);
        }

        /// <summary>
        /// 绑定一个服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <param name="concrete">服务实体</param>
        /// <param name="isStatic">服务是否静态化</param>
        /// <returns></returns>
        public IBindData Bind(string serviceName, Func<IContainer, object[], object> concrete, bool isStatic)
        {
            lock (locker)
            {
                serviceName = Normalize(serviceName);

                instances.Remove(serviceName);
                alias = alias.RemoveValue(serviceName);
                alias.Remove(serviceName);

                BindData bindData = new BindData(this, serviceName, concrete, isStatic);


                binds.Add(serviceName, bindData);

                return bindData;
            }
        }

        /// <summary>
        /// 以依赖注入形式调用一个方法
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="methodName"></param>
        /// <param name="param"></param>
        public object Call(object instance, string methodName, params object[] param)
        {

            if (instance == null)
            {
                throw new RuntimeException("call instance is null");
            }

            MethodInfo methodInfo = instance.GetType().GetMethod(methodName);

            return Call(instance, methodInfo, param);

        }

        /// <summary>
        /// 以依赖注入形式调用一个方法
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="methodInfo"></param>
        /// <param name="param"></param>
        public object Call(object instance, MethodInfo methodInfo, params object[] param)
        {

            if (instance == null)
            {
                throw new RuntimeException("call instance is null");
            }

            Type type = instance.GetType();

            if (methodInfo == null)
            {
                throw new RuntimeException("can not find instance [" + type.ToString() + "] 's function :" + methodInfo.Name);
            }

            List<ParameterInfo> parameter = new List<ParameterInfo>(methodInfo.GetParameters());

            var bindData = GetBindData(type.ToString());
            if (parameter.Count > 0)
            {
                param = GetDependencies(bindData, type, parameter, param);
            }
            else
            {
                param = new object[] { };
            }

            return methodInfo.Invoke(instance, param);

        }

        /// <summary>
        /// 构造服务
        /// </summary>
        /// <param name="serviceName">服务名或别名</param>
        /// <param name="param">附带参数</param>
        /// <returns></returns>
        public object Make(string serviceName, params object[] param)
        {
            lock (locker)
            {
                serviceName = Normalize(serviceName);
                serviceName = GetAlias(serviceName);
                return NormalMake(serviceName, true, param);
            }
        }

        /// <summary>
        /// 构造服务
        /// </summary>
        public object this[string serviceName] { get { return Make(serviceName); } }

        /// <summary>添加到静态内容</summary>
        /// <param name="serviceName">服务名</param>
        /// <param name="objectData">实体数据</param>
        public void Instance(string serviceName, object objectData)
        {
            if (objectData == null)
            {
                return;
            }

            lock (locker)
            {

                serviceName = Normalize(serviceName);
                serviceName = GetAlias(serviceName);

                if (instances.ContainsKey(serviceName))
                {
                    instances.Remove(serviceName);
                }

                var bindData = GetBindData(serviceName);
                objectData = ExecDecorator(bindData, bindData.ExecDecorator(objectData));

                instances.Add(serviceName, objectData);

            }
        }

        /// <summary>
        /// 当解决类型时触发的事件
        /// </summary>
        /// <param name="func"></param>
        public IContainer Resolving(Func<IContainer, IBindData, object, object> func)
        {
            lock (locker)
            {
                if (decorator == null)
                {
                    decorator = new List<Func<IContainer, IBindData, object, object>>();
                }
                decorator.Add(func);
                foreach (KeyValuePair<string, object> data in instances)
                {
                    var bindData = GetBindData(data.Key);
                    instances[data.Key] = func(this, bindData, data.Value);
                }
                return this;
            }
        }

        private object ExecDecorator(BindData bindData, object obj)
        {
            if (decorator != null)
            {
                foreach (Func<IContainer, IBindData, object, object> func in decorator)
                {
                    obj = func(this, bindData, obj);
                }
            }
            return obj;
        }

        /// <summary>
        /// 构造服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <param name="withConcrete">是否允许从Concrete获取</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        private object NormalMake(string serviceName, bool withConcrete, params object[] param)
        {
            if (instances.ContainsKey(serviceName))
            {
                return instances[serviceName];
            }

            var bindData = GetBindData(serviceName);
            object objectData;
            if (withConcrete)
            {
                objectData = NormalBuild(bindData, param);
            }
            else
            {
                objectData = Build(bindData, serviceName, param);
            }

            if ((!withConcrete) || (withConcrete && (bindData.Concrete != null)))
            {
                DIAttr(bindData, objectData);

                if (bindData.IsStatic)
                {
                    Instance(serviceName, objectData);
                }
            }

            return objectData;
        }

        /// <summary>
        /// 常规编译
        /// </summary>
        /// <param name="bindData"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private object NormalBuild(BindData bindData, object[] param)
        {
            if (bindData.Concrete != null)
            {
                return bindData.Concrete(this, param);
            }
            return NormalMake(bindData.Service, false, param); //Build(bindData , bindData.Service, param);
        }

        /// <summary>构造服务</summary>
        /// <param name="serviceName">服务名</param
        /// <param name="bindData"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private object Build(BindData bindData, string serviceName, object[] param)
        {
            if (param == null)
            {
                param = new object[] { };
            }
            Type type = GetType(bindData.Service);
            if (type == null)
            {
                return null;
            }

            if (type.IsAbstract || type.IsInterface)
            {
                if (serviceName != bindData.Service)
                {
                    type = Type.GetType(serviceName);
                }
                else
                {
                    return null;
                }
            }
            ConstructorInfo[] constructor = type.GetConstructors();
            if (constructor.Length <= 0)
            {
                return Activator.CreateInstance(type);
            }

            List<ParameterInfo> parameter = new List<ParameterInfo>(constructor[constructor.Length - 1].GetParameters());
            parameter.RemoveRange(0, param.Length);

            if (parameter.Count > 0)
            {
                param = GetDependencies(bindData, type, parameter, param);
            }

            return Activator.CreateInstance(type, param);
        }

        /// <summary>标准化服务名</summary>
        /// <param name="serviceName">服务名</param>
        /// <returns></returns>
        private string Normalize(string serviceName)
        {
            return serviceName;
        }

        /// <summary>属性注入</summary>
        /// <param name="bindData"></param>
        /// <param name="cls"></param>
        private void DIAttr(BindData bindData, object cls)
        {
            if (cls == null) { return; }

            string typeName;
            PropertyInfo[] propertyInfos = cls.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (int index = 0; index < propertyInfos.Length; index++)
            {
                PropertyInfo property = propertyInfos[index];
                if (!property.CanWrite)
                {
                    continue;
                }
                object[] propertyAttrs = property.GetCustomAttributes(typeof(Dependency), true);
                if (propertyAttrs.Length <= 0)
                {
                    continue;
                }

                Dependency dependency = propertyAttrs[0] as Dependency;
                if (string.IsNullOrEmpty(dependency.Alias))
                {
                    typeName = property.PropertyType.ToString();
                }
                else
                {
                    typeName = dependency.Alias;
                }

                if (property.PropertyType.IsClass || property.PropertyType.IsInterface)
                {
                    property.SetValue(cls, ResloveClassAttr(bindData, cls.GetType(), typeName), null);
                }
                else
                {
                    property.SetValue(cls, ResolveNonClassAttr(bindData, cls.GetType(), typeName), null);
                }
            }
        }

        /// <summary>解决非类类型</summary>
        /// <param name="type">参数类型</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        private object ResolveNonClassAttr(BindData bindData, Type parent, string cls)
        {
            return null;
        }

        /// <summary>解决类类型</summary>
        /// <returns></returns>
        private object ResloveClassAttr(BindData bindData, Type parent, string cls)
        {
            return Make(bindData.GetContextual(cls)); ;
        }

        /// <summary>获取依赖关系</summary>
        /// <param name="type">类型</param>
        /// <param name="paramInfo">参数信息</param>
        /// <param name="param">手动输入的参数</param>
        /// <returns></returns>
        private object[] GetDependencies(BindData bindData, Type type, List<ParameterInfo> paramInfo, object[] param)
        {
            List<object> myParam = new List<object>();

            ParameterInfo info;
            for (int i = 0; i < paramInfo.Count; i++)
            {
                info = paramInfo[i];
                if (param != null && i < param.Length)
                {
                    if (param[i] == null || info.ParameterType.IsAssignableFrom(param[i].GetType()))
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

        /// <summary>解决非类类型</summary>
        /// <param name="info">参数信息</param>
        /// <returns></returns>
        private object ResolveNonClass(BindData bindData, Type parent, ParameterInfo info)
        {
            return info.DefaultValue;
        }

        /// <summary>解决类类型</summary>
        /// <param name="bindData"></param>
        /// <param name="parent"></param>
        /// <param name="info">参数信息</param>
        /// <returns></returns>
        private object ResloveClass(BindData bindData, Type parent, ParameterInfo info)
        {
            return Make(bindData.GetContextual(info.ParameterType.ToString()), null);
        }


        /// <summary>
        /// 获取别名最终对应的服务名
        /// </summary>
        /// <param name="serviceName">服务名 </param>
        /// <returns></returns>
        private string GetAlias(string serviceName)
        {
            if (alias.ContainsKey(serviceName))
            {
                return alias[serviceName];
            }
            return serviceName;
        }


        /// <summary>获取服务绑定数据</summary>
        /// <param name="serviceName">服务名</param>
        /// <returns></returns>
        private BindData GetBindData(string serviceName)
        {
            if (!binds.ContainsKey(serviceName))
            {
                return new BindData(this, serviceName, null, false);
            }
            return binds[serviceName];
        }


        /// <summary>获取类型映射</summary>
        /// <param name="serviceName">服务名</param>
        /// <returns></returns>
        private Type GetType(string serviceName)
        {
            if (typeDict.ContainsKey(serviceName))
            {
                return typeDict[serviceName];
            }
            return Type.GetType(serviceName);
        }
    }
}