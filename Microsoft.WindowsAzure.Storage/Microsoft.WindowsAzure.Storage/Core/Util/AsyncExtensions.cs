using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
	// Token: 0x0200005E RID: 94
	internal static class AsyncExtensions
	{
		// Token: 0x06000D74 RID: 3444 RVA: 0x0003177C File Offset: 0x0002F97C
		private static CancellationTokenRegistration? RegisterCancellationToken(CancellationToken cancellationToken, out CancellableOperationBase cancellableOperation)
		{
			if (cancellationToken.CanBeCanceled)
			{
				cancellableOperation = new CancellableOperationBase();
				return new CancellationTokenRegistration?(cancellationToken.Register(new Action(cancellableOperation.Cancel)));
			}
			cancellableOperation = null;
			return null;
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x000317BF File Offset: 0x0002F9BF
		private static void AssignCancellableOperation(CancellableOperationBase cancellableOperation, ICancellableAsyncResult asyncResult, CancellationToken cancellationToken)
		{
			if (cancellableOperation != null)
			{
				cancellableOperation.CancelDelegate = new Action(asyncResult.Cancel);
				if (cancellationToken.IsCancellationRequested)
				{
					cancellableOperation.Cancel();
				}
			}
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x000318C8 File Offset: 0x0002FAC8
		private static AsyncCallback CreateCallback<TResult>(TaskCompletionSource<TResult> taskCompletionSource, CancellationTokenRegistration? registration, Func<IAsyncResult, TResult> endMethod)
		{
			return delegate(IAsyncResult ar)
			{
				try
				{
					if (registration != null)
					{
						registration.Value.Dispose();
					}
					TResult result = endMethod(ar);
					taskCompletionSource.TrySetResult(result);
				}
				catch (OperationCanceledException)
				{
					taskCompletionSource.TrySetCanceled();
				}
				catch (StorageException ex)
				{
					bool flag = false;
					for (Exception innerException = ex.InnerException; innerException != null; innerException = innerException.InnerException)
					{
						if (innerException is OperationCanceledException)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						taskCompletionSource.TrySetCanceled();
					}
					else
					{
						taskCompletionSource.TrySetException(ex);
					}
				}
				catch (Exception exception)
				{
					taskCompletionSource.TrySetException(exception);
				}
			};
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x000319D8 File Offset: 0x0002FBD8
		private static AsyncCallback CreateCallbackVoid(TaskCompletionSource<object> taskCompletionSource, CancellationTokenRegistration? registration, Action<IAsyncResult> endMethod)
		{
			return delegate(IAsyncResult ar)
			{
				try
				{
					if (registration != null)
					{
						registration.Value.Dispose();
					}
					endMethod(ar);
					taskCompletionSource.TrySetResult(null);
				}
				catch (OperationCanceledException)
				{
					taskCompletionSource.TrySetCanceled();
				}
				catch (StorageException ex)
				{
					bool flag = false;
					for (Exception innerException = ex.InnerException; innerException != null; innerException = innerException.InnerException)
					{
						if (innerException is OperationCanceledException)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						taskCompletionSource.TrySetCanceled();
					}
					else
					{
						taskCompletionSource.TrySetException(ex);
					}
				}
				catch (Exception exception)
				{
					taskCompletionSource.TrySetException(exception);
				}
			};
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00031A0C File Offset: 0x0002FC0C
		internal static Task TaskFromVoidApm(Func<AsyncCallback, object, ICancellableAsyncResult> beginMethod, Action<IAsyncResult> endMethod, CancellationToken cancellationToken)
		{
			TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
			if (cancellationToken.IsCancellationRequested)
			{
				taskCompletionSource.TrySetCanceled();
			}
			else
			{
				CancellableOperationBase cancellableOperation;
				CancellationTokenRegistration? registration = AsyncExtensions.RegisterCancellationToken(cancellationToken, out cancellableOperation);
				ICancellableAsyncResult asyncResult = beginMethod(AsyncExtensions.CreateCallbackVoid(taskCompletionSource, registration, endMethod), null);
				AsyncExtensions.AssignCancellableOperation(cancellableOperation, asyncResult, cancellationToken);
			}
			return taskCompletionSource.Task;
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x00031A58 File Offset: 0x0002FC58
		internal static Task TaskFromVoidApm<T1>(Func<T1, AsyncCallback, object, ICancellableAsyncResult> beginMethod, Action<IAsyncResult> endMethod, T1 arg1, CancellationToken cancellationToken)
		{
			TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
			if (cancellationToken.IsCancellationRequested)
			{
				taskCompletionSource.TrySetCanceled();
			}
			else
			{
				CancellableOperationBase cancellableOperation;
				CancellationTokenRegistration? registration = AsyncExtensions.RegisterCancellationToken(cancellationToken, out cancellableOperation);
				ICancellableAsyncResult asyncResult = beginMethod(arg1, AsyncExtensions.CreateCallbackVoid(taskCompletionSource, registration, endMethod), null);
				AsyncExtensions.AssignCancellableOperation(cancellableOperation, asyncResult, cancellationToken);
			}
			return taskCompletionSource.Task;
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00031AA8 File Offset: 0x0002FCA8
		internal static Task TaskFromVoidApm<T1, T2>(Func<T1, T2, AsyncCallback, object, ICancellableAsyncResult> beginMethod, Action<IAsyncResult> endMethod, T1 arg1, T2 arg2, CancellationToken cancellationToken)
		{
			TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
			if (cancellationToken.IsCancellationRequested)
			{
				taskCompletionSource.TrySetCanceled();
			}
			else
			{
				CancellableOperationBase cancellableOperation;
				CancellationTokenRegistration? registration = AsyncExtensions.RegisterCancellationToken(cancellationToken, out cancellableOperation);
				ICancellableAsyncResult asyncResult = beginMethod(arg1, arg2, AsyncExtensions.CreateCallbackVoid(taskCompletionSource, registration, endMethod), null);
				AsyncExtensions.AssignCancellableOperation(cancellableOperation, asyncResult, cancellationToken);
			}
			return taskCompletionSource.Task;
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x00031AF8 File Offset: 0x0002FCF8
		internal static Task TaskFromVoidApm<T1, T2, T3>(Func<T1, T2, T3, AsyncCallback, object, ICancellableAsyncResult> beginMethod, Action<IAsyncResult> endMethod, T1 arg1, T2 arg2, T3 arg3, CancellationToken cancellationToken)
		{
			TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
			if (cancellationToken.IsCancellationRequested)
			{
				taskCompletionSource.TrySetCanceled();
			}
			else
			{
				CancellableOperationBase cancellableOperation;
				CancellationTokenRegistration? registration = AsyncExtensions.RegisterCancellationToken(cancellationToken, out cancellableOperation);
				ICancellableAsyncResult asyncResult = beginMethod(arg1, arg2, arg3, AsyncExtensions.CreateCallbackVoid(taskCompletionSource, registration, endMethod), null);
				AsyncExtensions.AssignCancellableOperation(cancellableOperation, asyncResult, cancellationToken);
			}
			return taskCompletionSource.Task;
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x00031B4C File Offset: 0x0002FD4C
		internal static Task TaskFromVoidApm<T1, T2, T3, T4>(Func<T1, T2, T3, T4, AsyncCallback, object, ICancellableAsyncResult> beginMethod, Action<IAsyncResult> endMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken cancellationToken)
		{
			TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
			if (cancellationToken.IsCancellationRequested)
			{
				taskCompletionSource.TrySetCanceled();
			}
			else
			{
				CancellableOperationBase cancellableOperation;
				CancellationTokenRegistration? registration = AsyncExtensions.RegisterCancellationToken(cancellationToken, out cancellableOperation);
				ICancellableAsyncResult asyncResult = beginMethod(arg1, arg2, arg3, arg4, AsyncExtensions.CreateCallbackVoid(taskCompletionSource, registration, endMethod), null);
				AsyncExtensions.AssignCancellableOperation(cancellableOperation, asyncResult, cancellationToken);
			}
			return taskCompletionSource.Task;
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x00031BA0 File Offset: 0x0002FDA0
		internal static Task TaskFromVoidApm<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, AsyncCallback, object, ICancellableAsyncResult> beginMethod, Action<IAsyncResult> endMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, CancellationToken cancellationToken)
		{
			TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
			if (cancellationToken.IsCancellationRequested)
			{
				taskCompletionSource.TrySetCanceled();
			}
			else
			{
				CancellableOperationBase cancellableOperation;
				CancellationTokenRegistration? registration = AsyncExtensions.RegisterCancellationToken(cancellationToken, out cancellableOperation);
				ICancellableAsyncResult asyncResult = beginMethod(arg1, arg2, arg3, arg4, arg5, AsyncExtensions.CreateCallbackVoid(taskCompletionSource, registration, endMethod), null);
				AsyncExtensions.AssignCancellableOperation(cancellableOperation, asyncResult, cancellationToken);
			}
			return taskCompletionSource.Task;
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x00031BF8 File Offset: 0x0002FDF8
		internal static Task TaskFromVoidApm<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, AsyncCallback, object, ICancellableAsyncResult> beginMethod, Action<IAsyncResult> endMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, CancellationToken cancellationToken)
		{
			TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
			if (cancellationToken.IsCancellationRequested)
			{
				taskCompletionSource.TrySetCanceled();
			}
			else
			{
				CancellableOperationBase cancellableOperation;
				CancellationTokenRegistration? registration = AsyncExtensions.RegisterCancellationToken(cancellationToken, out cancellableOperation);
				ICancellableAsyncResult asyncResult = beginMethod(arg1, arg2, arg3, arg4, arg5, arg6, AsyncExtensions.CreateCallbackVoid(taskCompletionSource, registration, endMethod), null);
				AsyncExtensions.AssignCancellableOperation(cancellableOperation, asyncResult, cancellationToken);
			}
			return taskCompletionSource.Task;
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x00031C50 File Offset: 0x0002FE50
		internal static Task TaskFromVoidApm<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, AsyncCallback, object, ICancellableAsyncResult> beginMethod, Action<IAsyncResult> endMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, CancellationToken cancellationToken)
		{
			TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
			if (cancellationToken.IsCancellationRequested)
			{
				taskCompletionSource.TrySetCanceled();
			}
			else
			{
				CancellableOperationBase cancellableOperation;
				CancellationTokenRegistration? registration = AsyncExtensions.RegisterCancellationToken(cancellationToken, out cancellableOperation);
				ICancellableAsyncResult asyncResult = beginMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, AsyncExtensions.CreateCallbackVoid(taskCompletionSource, registration, endMethod), null);
				AsyncExtensions.AssignCancellableOperation(cancellableOperation, asyncResult, cancellationToken);
			}
			return taskCompletionSource.Task;
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x00031CAC File Offset: 0x0002FEAC
		internal static Task<TResult> TaskFromApm<TResult>(Func<AsyncCallback, object, ICancellableAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, CancellationToken cancellationToken)
		{
			TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
			if (cancellationToken.IsCancellationRequested)
			{
				taskCompletionSource.TrySetCanceled();
			}
			else
			{
				CancellableOperationBase cancellableOperation;
				CancellationTokenRegistration? registration = AsyncExtensions.RegisterCancellationToken(cancellationToken, out cancellableOperation);
				ICancellableAsyncResult asyncResult = beginMethod(AsyncExtensions.CreateCallback<TResult>(taskCompletionSource, registration, endMethod), null);
				AsyncExtensions.AssignCancellableOperation(cancellableOperation, asyncResult, cancellationToken);
			}
			return taskCompletionSource.Task;
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x00031CF8 File Offset: 0x0002FEF8
		internal static Task<TResult> TaskFromApm<T1, TResult>(Func<T1, AsyncCallback, object, ICancellableAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, T1 arg1, CancellationToken cancellationToken)
		{
			TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
			if (cancellationToken.IsCancellationRequested)
			{
				taskCompletionSource.TrySetCanceled();
			}
			else
			{
				CancellableOperationBase cancellableOperation;
				CancellationTokenRegistration? registration = AsyncExtensions.RegisterCancellationToken(cancellationToken, out cancellableOperation);
				ICancellableAsyncResult asyncResult = beginMethod(arg1, AsyncExtensions.CreateCallback<TResult>(taskCompletionSource, registration, endMethod), null);
				AsyncExtensions.AssignCancellableOperation(cancellableOperation, asyncResult, cancellationToken);
			}
			return taskCompletionSource.Task;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x00031D48 File Offset: 0x0002FF48
		internal static Task<TResult> TaskFromApm<T1, T2, TResult>(Func<T1, T2, AsyncCallback, object, ICancellableAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, T1 arg1, T2 arg2, CancellationToken cancellationToken)
		{
			TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
			if (cancellationToken.IsCancellationRequested)
			{
				taskCompletionSource.TrySetCanceled();
			}
			else
			{
				CancellableOperationBase cancellableOperation;
				CancellationTokenRegistration? registration = AsyncExtensions.RegisterCancellationToken(cancellationToken, out cancellableOperation);
				ICancellableAsyncResult asyncResult = beginMethod(arg1, arg2, AsyncExtensions.CreateCallback<TResult>(taskCompletionSource, registration, endMethod), null);
				AsyncExtensions.AssignCancellableOperation(cancellableOperation, asyncResult, cancellationToken);
			}
			return taskCompletionSource.Task;
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x00031D98 File Offset: 0x0002FF98
		internal static Task<TResult> TaskFromApm<T1, T2, T3, TResult>(Func<T1, T2, T3, AsyncCallback, object, ICancellableAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, T1 arg1, T2 arg2, T3 arg3, CancellationToken cancellationToken)
		{
			TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
			if (cancellationToken.IsCancellationRequested)
			{
				taskCompletionSource.TrySetCanceled();
			}
			else
			{
				CancellableOperationBase cancellableOperation;
				CancellationTokenRegistration? registration = AsyncExtensions.RegisterCancellationToken(cancellationToken, out cancellableOperation);
				ICancellableAsyncResult asyncResult = beginMethod(arg1, arg2, arg3, AsyncExtensions.CreateCallback<TResult>(taskCompletionSource, registration, endMethod), null);
				AsyncExtensions.AssignCancellableOperation(cancellableOperation, asyncResult, cancellationToken);
			}
			return taskCompletionSource.Task;
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x00031DEC File Offset: 0x0002FFEC
		internal static Task<TResult> TaskFromApm<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, AsyncCallback, object, ICancellableAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken cancellationToken)
		{
			TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
			if (cancellationToken.IsCancellationRequested)
			{
				taskCompletionSource.TrySetCanceled();
			}
			else
			{
				CancellableOperationBase cancellableOperation;
				CancellationTokenRegistration? registration = AsyncExtensions.RegisterCancellationToken(cancellationToken, out cancellableOperation);
				ICancellableAsyncResult asyncResult = beginMethod(arg1, arg2, arg3, arg4, AsyncExtensions.CreateCallback<TResult>(taskCompletionSource, registration, endMethod), null);
				AsyncExtensions.AssignCancellableOperation(cancellableOperation, asyncResult, cancellationToken);
			}
			return taskCompletionSource.Task;
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x00031E40 File Offset: 0x00030040
		internal static Task<TResult> TaskFromApm<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, AsyncCallback, object, ICancellableAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, CancellationToken cancellationToken)
		{
			TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
			if (cancellationToken.IsCancellationRequested)
			{
				taskCompletionSource.TrySetCanceled();
			}
			else
			{
				CancellableOperationBase cancellableOperation;
				CancellationTokenRegistration? registration = AsyncExtensions.RegisterCancellationToken(cancellationToken, out cancellableOperation);
				ICancellableAsyncResult asyncResult = beginMethod(arg1, arg2, arg3, arg4, arg5, AsyncExtensions.CreateCallback<TResult>(taskCompletionSource, registration, endMethod), null);
				AsyncExtensions.AssignCancellableOperation(cancellableOperation, asyncResult, cancellationToken);
			}
			return taskCompletionSource.Task;
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00031E98 File Offset: 0x00030098
		internal static Task<TResult> TaskFromApm<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, AsyncCallback, object, ICancellableAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, CancellationToken cancellationToken)
		{
			TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
			if (cancellationToken.IsCancellationRequested)
			{
				taskCompletionSource.TrySetCanceled();
			}
			else
			{
				CancellableOperationBase cancellableOperation;
				CancellationTokenRegistration? registration = AsyncExtensions.RegisterCancellationToken(cancellationToken, out cancellableOperation);
				ICancellableAsyncResult asyncResult = beginMethod(arg1, arg2, arg3, arg4, arg5, arg6, AsyncExtensions.CreateCallback<TResult>(taskCompletionSource, registration, endMethod), null);
				AsyncExtensions.AssignCancellableOperation(cancellableOperation, asyncResult, cancellationToken);
			}
			return taskCompletionSource.Task;
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x00031EF0 File Offset: 0x000300F0
		internal static Task<TResult> TaskFromApm<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, AsyncCallback, object, ICancellableAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, CancellationToken cancellationToken)
		{
			TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
			if (cancellationToken.IsCancellationRequested)
			{
				taskCompletionSource.TrySetCanceled();
			}
			else
			{
				CancellableOperationBase cancellableOperation;
				CancellationTokenRegistration? registration = AsyncExtensions.RegisterCancellationToken(cancellationToken, out cancellableOperation);
				ICancellableAsyncResult asyncResult = beginMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, AsyncExtensions.CreateCallback<TResult>(taskCompletionSource, registration, endMethod), null);
				AsyncExtensions.AssignCancellableOperation(cancellableOperation, asyncResult, cancellationToken);
			}
			return taskCompletionSource.Task;
		}
	}
}
