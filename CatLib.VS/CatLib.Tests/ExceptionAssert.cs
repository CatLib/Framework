﻿using System;

#if UNITY_EDITOR
using NUnit.Framework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests
{
    /// <summary>
    /// 异常断言
    /// </summary>
    static class ExceptionAssert
    {
        /// <summary>
        /// 行为中需要引发一个异常
        /// </summary>
        /// <param name="action">行为</param>
        /// <returns></returns>
        public static Exception Throws(Action action)
        {
            return Throws(action, null);
        }

        /// <summary>
        /// 行为中需要引发一个异常
        /// </summary>
        /// <param name="action">行为</param>
        /// <param name="message">异常消息</param>
        /// <returns></returns>
        public static Exception Throws(Action action,string message)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                return ex;
            }
            Assert.Fail(message ?? "need throw exception");
            return null;
        }

        /// <summary>
        /// 行为中需要引发一个异常
        /// </summary>
        /// <param name="action">行为</param>
        /// <returns></returns>
        public static Exception Throws<T>(Action action) where T : Exception
        {
            return Throws<T>(action, null);
        }

        /// <summary>
        /// 行为中需要引发一个异常
        /// </summary>
        /// <param name="action">行为</param>
        /// <param name="message">异常消息</param>
        /// <returns></returns>
        public static Exception Throws<T>(Action action, string message) where T : Exception
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                if ((ex as T) != null)
                {
                    return ex;
                }
            }
            Assert.Fail(message ?? "need throw exception");
            return null;
        }

        /// <summary>
        /// 行为中不触发异常
        /// </summary>
        /// <param name="action">行为</param>
        public static void DoesNotThrow(Action action)
        {
            DoesNotThrow(action, null);
        }

        /// <summary>
        /// 行为中不触发异常
        /// </summary>
        /// <param name="action">行为</param>
        /// <param name="message">异常消息</param>
        public static void DoesNotThrow(Action action, string message)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception)
            {
                Assert.Fail(message ?? "need not throw exception");
            }
        }
    }
}
