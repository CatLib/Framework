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

using CatLib.API;
using CatLib.API.Thread;
using System.Collections.Generic;
using System.Threading;
using CatLib.API.Time;

namespace CatLib.Thread
{
    /// <summary>
    /// 多线程运行器
    /// </summary>
    public sealed class ThreadRuner : IThread, IUpdate
    {
        /// <summary>
        /// 应用程序
        /// </summary>
        [Inject]
        public IApplication App { get; set; }

        /// <summary>
        /// 时间组件
        /// </summary>
        [Inject]
        public ITime Time { get; set; }

        /// <summary>
        /// 任务列表
        /// </summary>
        private readonly List<ThreadTask> taskRunner = new List<ThreadTask>();

        /// <summary>
        /// 任务读写锁
        /// </summary>
        private readonly ReaderWriterLockSlim taskRunnerLocker = new ReaderWriterLockSlim();

        /// <summary>
        /// 新建一个多线程任务
        /// </summary>
        /// <param name="task">任务内容</param>
        /// <returns>任务</returns>
        public ITask Task(System.Action task)
        {
            return new ThreadTask(this)
            {
                Task = task,
                ReturnResult = false,
            };
        }

        /// <summary>
        /// 新建一个多线程任务允许产生回调
        /// </summary>
        /// <param name="task">任务内容</param>
        /// <returns>任务</returns>
        public ITask Task(System.Func<object> task)
        {
            return new ThreadTask(this)
            {
                TaskWithResult = task,
                ReturnResult = true,
            };
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="task">任务</param>
        /// <returns>任务句柄</returns>
        public ITaskHandler AddTask(ThreadTask task)
        {
            task.StartTime = Time.Time;
            if (task.DelayTime > 0)
            {
                taskRunnerLocker.EnterWriteLock();
                try
                {
                    taskRunner.Add(task);
                }
                finally
                {
                    taskRunnerLocker.ExitWriteLock();
                }
            }
            else
            {
                ThreadPool.QueueUserWorkItem(ThreadExecuter, task);
            }
            return task;
        }

        /// <summary>
        /// 撤销任务
        /// </summary>
        /// <param name="task">任务</param>
        public void Cancel(ThreadTask task)
        {
            taskRunnerLocker.EnterWriteLock();
            try
            {
                taskRunner.Remove(task);
            }
            finally
            {
                taskRunnerLocker.ExitWriteLock();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            taskRunnerLocker.EnterReadLock();
            var handlersToRemove = new bool[taskRunner.Count];

            try
            {
                for (var i = 0; i < taskRunner.Count; ++i)
                {
                    var runner = taskRunner[i];
                    if (!((runner.StartTime + runner.DelayTime) <= Time.Time))
                    {
                        continue;
                    }
                    ThreadPool.QueueUserWorkItem(ThreadExecuter, runner);
                    handlersToRemove[i] = true;
                }
            }
            finally
            {
                taskRunnerLocker.ExitReadLock();
            }

            taskRunnerLocker.EnterWriteLock();
            try
            {
                for (var i = handlersToRemove.Length - 1; i > -1; --i)
                {
                    if (handlersToRemove[i])
                    {
                        taskRunner.RemoveAt(i);
                    }
                }
            }
            finally
            {
                taskRunnerLocker.ExitWriteLock();
            }
        }

        /// <summary>
        /// 线程执行器
        /// </summary>
        /// <param name="state">线程任务</param>
        private void ThreadExecuter(object state)
        {
            try
            {
                if (typeof(ThreadTask) == state.GetType())
                {
                    RunTaskThread((ThreadTask)state);
                }
                else
                {
                    App.TriggerGlobal(ThreadEvents.ON_THREAD_EXECURE_ERROR, this).Trigger(
                                        new ExceptionEventArgs(
                                            new System.Exception(string.Format("type '{0}' not supported!", state.GetType())
                                        )));
                }
            }
            catch (System.Exception exception)
            {
                App.TriggerGlobal(ThreadEvents.ON_THREAD_EXECURE_ERROR, this).Trigger(new ExceptionEventArgs(exception));
            }
        }

        /// <summary>
        /// 运行任务线程
        /// </summary>
        /// <param name="task">任务</param>
        private void RunTaskThread(ThreadTask task)
        {
            try
            {
                object result = null;
                if (task.ReturnResult)
                {
                    result = task.TaskWithResult();
                }
                else
                {
                    task.Task();
                }

                if (task.Complete != null)
                {
                    App.MainThread(() =>
                    {
                        task.Complete();
                    });
                }

                if (task.CompleteWithResult != null)
                {
                    App.MainThread(() =>
                    {
                        task.CompleteWithResult(result);
                    });
                }
            }
            catch (System.Exception exception)
            {
                if (task.Error != null)
                {
                    App.MainThread(() =>
                    {
                        task.Error(exception);
                    });
                }
            }
        }
    }
}