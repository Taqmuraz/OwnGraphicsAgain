using System;
using System.Threading;

namespace OwnGraphicsAgain
{
    public static class AsyncTask
	{
		class ThreadAsyncTask : IAsyncTask
		{
			Thread thread;

            public ThreadAsyncTask(Thread thread)
            {
                this.thread = thread;
            }

            public void Wait()
            {
				while (thread.IsAlive) { }
            }
        }
		class MultipleAsyncTask : IAsyncTask
		{
			IAsyncTask[] tasks;

            public MultipleAsyncTask(IAsyncTask[] tasks)
            {
                this.tasks = tasks;
            }

            public void Wait()
            {
				foreach (var t in tasks) t.Wait();
            }
        }
		public static IAsyncTask Run(Action task)
		{
			Thread thread = new Thread(task.Invoke);
            thread.Start();
			return new ThreadAsyncTask(thread);
        }

		public static IAsyncTask Of(params IAsyncTask[] tasks)
		{
			return new MultipleAsyncTask(tasks);
		}
    }
}