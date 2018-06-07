

using System;

using System.Collections;

using System.Threading;

//using NUnit.Framework;



namespace TestTask

{



#region 任务计划接口和一些标准实现

    /// <summary>;

    /// 计划的接口

    /// </summary>;

    public interface ISchedule

    {

        /// <summary>;

        /// 返回最初计划执行时间

        /// </summary>;

        DateTime ExecutionTime { get; set; }



        /// <summary>;

        /// 初始化执行时间于现在时间的时间刻度差

        /// </summary>;

        long DueTime { get; }



        /// <summary>;

        /// 循环的周期

        /// </summary>;

        long Period { get; }

        #endregion



    }


#region 任务实现

/// <summary>;

/// 计划任务基类

/// 启动的任务会在工作工作线程中完成，调用启动方法后会立即返回。

/// 

/// 用法：

/// (1)如果你要创建自己的任务，需要从这个类继承一个新类，然后重载Execute(object param)方法．

/// 实现自己的任务,再把任务加入到任务管理中心来启动和停止。

/// 比如：

/// TaskCenter center = new TaskCenter();

/// Task newTask = new Task( new ImmediateExecution());

/// center.AddTask(newTask);

/// center.StartAllTask();



/// (2)直接把自己的任务写入TimerCallBack委托，然后生成一个Task类的实例，

/// 设置它的Job和JobParam属性，再Start就可以启动该服务了。此时不能够再使用任务管理中心了。

// 比如：

/// Task newTask = new Task( new ImmediateExecution());

/// newTask.Job+= new TimerCallback(newTask.Execute);

/// newTask.JobParam = "Test immedialte task"; //添加自己的参数

/// newTask.Start();

/// 

/// </summary>;

public class Task

{

/// <summary>;

/// 构造函数

/// </summary>;

/// <param name="schedule">;为每个任务制定一个执行计划</param>;

public Task(ISchedule schedule)

{

if(schedule==null)

{

throw (new ArgumentNullException("schedule") );

}



m_schedule =schedule;

}





/// <summary>;

/// 启动任务

/// </summary>;

public void Start()

{

//启动定时器

m_timer = new Timer(m_execTask, m_param, m_schedule.DueTime , m_schedule.Period);

}





/// <summary>;

/// 停止任务

/// </summary>;

public void Stop()

{

//停止定时器

m_timer.Change(Timeout.Infinite,Timeout.Infinite);



}





/// <summary>;

/// 任务内容

/// </summary>;

/// <param name="param">;任务函数参数</param>;

public virtual void Execute(object param)

{

//你需要重载该函数,但是需要在你的新函数中调用base.Execute();

m_lastExecuteTime = DateTime.Now;



if(m_schedule.Period == Timeout.Infinite)

{

m_nextExecuteTime = DateTime.MaxValue; //下次运行的时间不存在

}

else

{

TimeSpan period = new TimeSpan(m_schedule.Period * 1000);



m_nextExecuteTime = m_lastExecuteTime +period;

}



}





/// <summary>;

/// 任务下执行时间

/// </summary>;

public DateTime NextExecuteTime

{

get

{

return m_nextExecuteTime;

}

}



DateTime m_nextExecuteTime;

/// <summary>;

/// 执行任务的计划

/// </summary>;

public ISchedule Shedule

{

get

{

return m_schedule;

} 

}



private ISchedule m_schedule;



/// <summary>;

/// 系统定时器

/// </summary>;

private Timer m_timer;



/// <summary>;

/// 任务内容

/// </summary>;

public TimerCallback Job

{

get

{

return m_execTask;

}

set

{

m_execTask= value;

}

}



private TimerCallback m_execTask;



/// <summary>;

/// 任务参数

/// </summary>;

public object JobParam

{

set

{

m_param = value;

}

}

private object m_param;



/// <summary>;

/// 任务名称

/// </summary>;

public string Name

{

get

{

return m_name;

}

set

{

m_name = value;

}

}

private string m_name;





/// <summary>;

/// 任务描述

/// </summary>;

public string Description

{

get

{

return m_description;

}

set

{

m_description = value;

}

}

private string m_description;



/// <summary>;

/// 该任务最后一次执行的时间

/// </summary>;

public DateTime LastExecuteTime

{

get

{

return m_lastExecuteTime;

}

}

private DateTime m_lastExecut

eTime;





}



#endregion



#region 启动任务



/// <summary>;

/// 任务管理中心

/// 使用它可以管理一个或则多个同时运行的任务

/// </summary>;

public class TaskCenter

{

/// <summary>;

/// 构造函数

/// </summary>;

public TaskCenter()

{

m_scheduleTasks = new ArrayList();

}



/// <summary>;

/// 添加任务

/// </summary>;

/// <param name="newTask">;新任务</param>;

public void AddTask(Task newTask)

{

m_scheduleTasks.Add(newTask);

}



/// <summary>;

/// 删除任务

/// </summary>;

/// <param name="delTask">;将要删除的任务，你可能需要停止掉该任务</param>;

public void DelTask(Task delTask)

{

m_scheduleTasks.Remove(delTask);

}



/// <summary>;

/// 启动所有的任务

/// </summary>;

public void StartAllTask()

{

foreach(Task task in ScheduleTasks)

{

StartTask(task);

}

}



/// <summary>;

/// 启动一个任务

/// </summary>;

/// <param name="task">;</param>;

public void StartTask(Task task)

{

//标准启动方法

if(task.Job == null)

{

task.Job+= new TimerCallback(task.Execute);

}



task.Start();

}



/// <summary>;

/// 终止所有的任务

/// </summary>;

public void TerminateAllTask()

{

foreach(Task task in ScheduleTasks)

{

TerminateTask(task);

}

}



/// <summary>;

/// 终止一个任务

/// </summary>;

/// <param name="task">;</param>;

public void TerminateTask(Task task)

{

task.Stop();

}



/// <summary>;

/// 获得所有的任务

/// </summary>;

ArrayList ScheduleTasks

{

get

{

return m_scheduleTasks;

}

}

private ArrayList m_scheduleTasks;



/// <summary>;

/// 单元测试代码

/// </summary>;

public void TestTaskCenter()

{

TaskCenter center = new TaskCenter();



//Test immedialte task

Task newTask = new Task(new ImmediateExecution());

newTask.Job+= new TimerCallback(newTask.Execute);

newTask.JobParam = "Test immedialte task";



//Test excute once task

DateTime sheduleTime = DateTime.Now.AddSeconds(10);



ScheduleExecutionOnce future = new ScheduleExecutionOnce(sheduleTime);



Task sheduleTask = new Task(future);

sheduleTask.Job+= new TimerCallback(sheduleTask.Execute);

sheduleTask.JobParam = "Test excute once task";



//Test cyc task at once



CycExecution cyc = new CycExecution(new TimeSpan(0, 0, 2));

Task cysTask = new Task(cyc);



cysTask.Job+= new TimerCallback(cysTask.Execute);

cysTask.JobParam = "Test cyc task";



//Test cys task at schedule



CycExecution cycShedule = new CycExecution(DateTime.Now.AddSeconds(8), new TimeSpan(0, 0, 2));



Task cycSheduleTask = new Task(cycShedule);



cycSheduleTask.Job+= new TimerCallback(cysTask.Execute);

cycSheduleTask.JobParam = "Test cyc Shedule task";





center.AddTask(newTask);

center.AddTask(

sheduleTask);

center.AddTask(cysTask);



center.AddTask(cycSheduleTask);



center.StartAllTask();



Console.ReadLine();



Console.WriteLine(newTask.LastExecuteTime);

}

}



#endregion

}






using System;

using System.Threading;





namespace Ibms.Utility.Task.Test

{

/// <summary>;

/// 解释怎么创建自己的任务类，和使用它们

/// </summary>;

class TestTask : Ibms.Utility.Task.Task

{

public TestTask(ISchedule schedule)

:base(schedule)

{



}



public override void Execute(object param)

{

//一定要保留

base.Execute(param);



Console.WriteLine("Begin to execute a long job...NextExecuteTime:{0}",this.NextExecuteTime);



Thread.Sleep(5000);



Console.WriteLine("Test Tast execute job.Last ExecuteTime {0}, ",

this.LastExecuteTime

);







}



/// <summary>;

/// 应用程序的主入口点。

/// </summary>;

[STAThread]

static void Main(string[] args)

{

//

// TODO: 在此处添加代码以启动应用程序

//



TaskCenter center = new TaskCenter();



//Test immedialte task

Task newTask = new TestTask(new ImmediateExecution());

newTask.JobParam = "Test immedialte task";



//Test excute once task

DateTime sheduleTime = DateTime.Now.AddSeconds(10);

ScheduleExecutionOnce future = new ScheduleExecutionOnce(sheduleTime);

Task sheduleTask = new TestTask(future);

sheduleTask.JobParam = "Test excute once task";



//Test cyc task at once

CycExecution cyc = new CycExecution(new TimeSpan(0, 0, 2));

Task cysTask = new TestTask(cyc);

cysTask.JobParam = "Test cyc task";



//Test cys task at schedule

CycExecution cycShedule = new CycExecution(DateTime.Now.AddSeconds(8), new TimeSpan(0, 0, 2));

Task cycSheduleTask = new TestTask(cycShedule);

cycSheduleTask.JobParam = "Test cyc Shedule task";





center.AddTask(newTask);

center.AddTask(sheduleTask);

center.AddTask(cysTask);

center.AddTask(cycSheduleTask);



center.StartAllTask();



Console.ReadLine();



Console.WriteLine("terminate all task");



center.TerminateAllTask();



Console.ReadLine();



}



}

}
