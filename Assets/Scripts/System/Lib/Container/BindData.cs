using System;
using System.Collections.Generic;
using CatLib.Contracts.Container;

namespace CatLib.Container {

    /// <summary>
    /// 绑定关系
    /// </summary>
    public class BindData : IBindData
    {

        /// <summary>
        /// 绑定关系临时数据
        /// </summary>
        public class GivenData : IGivenData
        {

            protected BindData bindData;

            protected string needs;

            public GivenData(BindData bindData , string needs)
            {
                this.bindData = bindData;
                this.needs = needs;
            }

            public IBindData Given(string service)
            {
                return bindData.AddContextual(needs, service);
            }

            /// <summary>
            /// 给与什么服务
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public IBindData Given<T>()
            {
                return Given(typeof(T).ToString());
            }

        }

        /// <summary>
        /// 服务名
        /// </summary>
        public string Service { get; protected set; }

        /// <summary>
        /// 服务实体
        /// </summary>
        public Func<IContainer, object[], object> Concrete { get; protected set; }

        /// <summary>
        /// 是否是静态服务
        /// </summary>
        public bool IsStatic { get; protected set; }

        /// <summary>
        /// 上下文
        /// </summary>
        private Dictionary<string, string> contextual;

        /// <summary>
        /// 容器
        /// </summary>
        private IContainer container;

        /// <summary>
        /// 修饰器
        /// </summary>
        private List<Func<IContainer , IBindData, object, object>> decorator;

        public BindData(IContainer container, string service , Func<IContainer, object[], object> concrete, bool isStatic)
        {
            this.container = container;
            Service = service;
            Concrete = concrete;
            IsStatic = isStatic;
        }

        /// <summary>
        /// 需求某个服务                                                                                                                                                                                                                                                                                                                                                                                      
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public IGivenData Needs(string service)
        {
            return new GivenData(this, service);
        }

        /// <summary>
        /// 需求某个服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IGivenData Needs<T>()
        {
            return Needs(typeof(T).ToString());
        }

        /// <summary>
        /// 别名
        /// </summary>
        /// <typeparam name="T">别名</typeparam>
        /// <returns></returns>
        public IBindData Alias<T>()
        {
            return Alias(typeof(T).ToString());
        }

        /// <summary>
        /// 别名
        /// </summary>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        public IBindData Alias(string alias)
        {
            container.Alias(alias, Service);
            return this;
        }

        /// <summary>
        /// 获取上下文的需求关系
        /// </summary>
        /// <param name="needs">需求的服务</param>
        /// <returns></returns>
        public string GetContextual(string needs)
        {
            if (contextual == null) { return needs; }
            if (contextual.ContainsKey(needs)) { return contextual[needs]; }
            return needs;
        }

        /// <summary>
        /// 修饰器
        /// </summary>
        /// <param name="func"></param>
        public IBindData Decorator(Func<IContainer , IBindData, object, object> func)
        {
            if (decorator == null) { decorator = new List<Func<IContainer , IBindData, object, object>>(); }
            decorator.Add(func);
            return this;
        }

        /// <summary>
        /// 执行修饰器
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public object ExecDecorator(object obj)
        {
            if (decorator == null) { return obj; }
            foreach(Func<IContainer , IBindData, object, object> func in decorator)
            {
                obj = func(container , this , obj);
            }
            return obj;
        }

        /// <summary>
        /// 增加上下文
        /// </summary>
        /// <param name="needs">需求</param>
        /// <param name="given">给与</param>
        protected BindData AddContextual(string needs , string given)
        {
            if (contextual == null) { contextual = new Dictionary<string, string>(); }
            contextual.Add(needs, given);
            return this;
        }
    }

}