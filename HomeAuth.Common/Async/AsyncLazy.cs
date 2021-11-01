using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace HomeAuth.Common.Async
{
    public class AsyncLazy<T> : Lazy<Task<T>>
    {
	    public TaskAwaiter<T> GetAwaiter() => Value.GetAwaiter();

	    public AsyncLazy(Func<Task<T>> factory)
			: base(factory)
	    {

	    }
    }
}
