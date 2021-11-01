using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAuth.Common.Async
{
	public record struct Void;

    public class AsyncEvent<T>
    {
	    private readonly List<Func<T, Task>> _eventHandlers;

	    public AsyncEvent()
	    {
		    _eventHandlers = new List<Func<T, Task>>();
	    }

	    public async Task Raise(T eventArgs)
	    {
		    var tasks = _eventHandlers.Select(x => x(eventArgs)).ToList();

			try
		    {
			    await Task.WhenAll(tasks);
		    }
		    catch (Exception _)
		    {
			    if (tasks.Count(x => x.IsFaulted) == 1)
			    {
				    throw;
			    }

			    throw new AggregateException(tasks
				    .Where(x => x.Exception is not null)
				    .Select(x => x.Exception)
				    // ReSharper disable once SuspiciousTypeConversion.Global
				    .Cast<Exception>());

		    }
	    }

	    public void RegisterHandler(Func<T, Task> handler) => _eventHandlers.Add(handler);
    }

    public class AsyncEvent : AsyncEvent<Void> { }
}
