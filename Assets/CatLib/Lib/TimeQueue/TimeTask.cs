using CatLib.API.TimeQueue;
using System;
using System.Collections.Generic;

namespace CatLib.TimeQueue
{

    public class TimeTask : ITimeTask , ITimeTaskHandler
    {

        private TimeQueue queue;

        private List<TimeTaskAction> timeLine = new List<TimeTaskAction>();

        public List<TimeTaskAction> TimeLine{ get{ return timeLine; } }

        public Action TaskCall { get; set; }

        public Action<object> TaskCallWithContext { get; set; }

        public Action OnCompleteTask { get; set; }

        public Action<object> OnCompleteTaskWithContext { get; set; }

        public bool IsComplete { get; set; }

        public int TimeLineIndex{ get; set; }

        public TimeTask(TimeQueue queue)
        {
            this.queue = queue;
            TimeLineIndex = 0;
        }

        public ITimeTask Delay(float time)
        {
            timeLine.Add(new TimeTaskAction(){
                Type = TimeTaskActionTypes.DelayTime,
                FloatArgs = new float[]{ time , 0 }
            });
            return this;
        }

        public ITimeTask Loop(float time)
        {
            timeLine.Add(new TimeTaskAction(){
                Type = TimeTaskActionTypes.LoopTime,
                FloatArgs = new float[]{ time , 0 }
            });
            return this;
        }

        public ITimeTask Loop(Func<bool> loopFunc)
        {
            timeLine.Add(new TimeTaskAction(){
                Type = TimeTaskActionTypes.LoopFunc,
                FuncBoolArg = loopFunc
            });
            return this;
        }

        public ITimeTask OnComplete(Action onComplete)
        {
            OnCompleteTask = onComplete;
            return this;
        }

        public ITimeTask OnComplete(Action<object> onComplete)
        {
            OnCompleteTaskWithContext = onComplete;
            return this;
        }

        public ITimeTask Task(Action task)
        {
            Push();
            return queue.Task(task);
        }

        public ITimeTask Task(Action<object> task)
        {
            Push();
            return queue.Task(task);
        }

        public ITimeTaskHandler Push()
        {
            return queue.Push(this);
        }

        public ITimeQueue Play()
        {
            Push();
            queue.Play();
            return queue;
        }

        public void Cancel()
        {
            queue.Cancel(this);
        }

        public void Reset(){

            for(int i = 0 ; i < TimeLine.Count ; i++){

                switch(TimeLine[i].Type){

                    case TimeTaskActionTypes.DelayTime: 
                    case TimeTaskActionTypes.LoopTime:
                        TimeLine[i].FloatArgs[1] = 0; 
                        break;
                    default: break;
                    

                }

            }

            TimeLineIndex = 0;

        }
    }

}