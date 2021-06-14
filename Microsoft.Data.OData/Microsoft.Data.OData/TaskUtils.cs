using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x020002AE RID: 686
	internal static class TaskUtils
	{
		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001741 RID: 5953 RVA: 0x000533C8 File Offset: 0x000515C8
		internal static Task CompletedTask
		{
			get
			{
				if (TaskUtils.completedTask == null)
				{
					TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
					taskCompletionSource.SetResult(null);
					TaskUtils.completedTask = taskCompletionSource.Task;
				}
				return TaskUtils.completedTask;
			}
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x000533FC File Offset: 0x000515FC
		internal static Task<T> GetCompletedTask<T>(T value)
		{
			TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
			taskCompletionSource.SetResult(value);
			return taskCompletionSource.Task;
		}

		// Token: 0x06001743 RID: 5955 RVA: 0x0005341C File Offset: 0x0005161C
		internal static Task GetFaultedTask(Exception exception)
		{
			return TaskUtils.GetFaultedTask<object>(exception);
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x00053424 File Offset: 0x00051624
		internal static Task<T> GetFaultedTask<T>(Exception exception)
		{
			TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
			taskCompletionSource.SetException(exception);
			return taskCompletionSource.Task;
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x00053444 File Offset: 0x00051644
		internal static Task GetTaskForSynchronousOperation(Action synchronousOperation)
		{
			Task faultedTask;
			try
			{
				synchronousOperation();
				faultedTask = TaskUtils.CompletedTask;
			}
			catch (Exception ex)
			{
				if (!ExceptionUtils.IsCatchableExceptionType(ex))
				{
					throw;
				}
				faultedTask = TaskUtils.GetFaultedTask(ex);
			}
			return faultedTask;
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x00053484 File Offset: 0x00051684
		internal static Task<T> GetTaskForSynchronousOperation<T>(Func<T> synchronousOperation)
		{
			Task<T> faultedTask;
			try
			{
				T value = synchronousOperation();
				faultedTask = TaskUtils.GetCompletedTask<T>(value);
			}
			catch (Exception ex)
			{
				if (!ExceptionUtils.IsCatchableExceptionType(ex))
				{
					throw;
				}
				faultedTask = TaskUtils.GetFaultedTask<T>(ex);
			}
			return faultedTask;
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x000534C8 File Offset: 0x000516C8
		internal static Task GetTaskForSynchronousOperationReturningTask(Func<Task> synchronousOperation)
		{
			Task result;
			try
			{
				result = synchronousOperation();
			}
			catch (Exception ex)
			{
				if (!ExceptionUtils.IsCatchableExceptionType(ex))
				{
					throw;
				}
				result = TaskUtils.GetFaultedTask(ex);
			}
			return result;
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x00053504 File Offset: 0x00051704
		internal static Task<TResult> GetTaskForSynchronousOperationReturningTask<TResult>(Func<Task<TResult>> synchronousOperation)
		{
			Task<TResult> result;
			try
			{
				result = synchronousOperation();
			}
			catch (Exception ex)
			{
				if (!ExceptionUtils.IsCatchableExceptionType(ex))
				{
					throw;
				}
				result = TaskUtils.GetFaultedTask<TResult>(ex);
			}
			return result;
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x00053558 File Offset: 0x00051758
		internal static Task FollowOnSuccessWith(this Task antecedentTask, Action<Task> operation)
		{
			return TaskUtils.FollowOnSuccessWithImplementation<object>(antecedentTask, delegate(Task t)
			{
				operation(t);
				return null;
			});
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x00053584 File Offset: 0x00051784
		internal static Task<TFollowupTaskResult> FollowOnSuccessWith<TFollowupTaskResult>(this Task antecedentTask, Func<Task, TFollowupTaskResult> operation)
		{
			return TaskUtils.FollowOnSuccessWithImplementation<TFollowupTaskResult>(antecedentTask, operation);
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x000535AC File Offset: 0x000517AC
		internal static Task FollowOnSuccessWith<TAntecedentTaskResult>(this Task<TAntecedentTaskResult> antecedentTask, Action<Task<TAntecedentTaskResult>> operation)
		{
			return TaskUtils.FollowOnSuccessWithImplementation<object>(antecedentTask, delegate(Task t)
			{
				operation((Task<TAntecedentTaskResult>)t);
				return null;
			});
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x000535F4 File Offset: 0x000517F4
		internal static Task<TFollowupTaskResult> FollowOnSuccessWith<TAntecedentTaskResult, TFollowupTaskResult>(this Task<TAntecedentTaskResult> antecedentTask, Func<Task<TAntecedentTaskResult>, TFollowupTaskResult> operation)
		{
			return TaskUtils.FollowOnSuccessWithImplementation<TFollowupTaskResult>(antecedentTask, (Task t) => operation((Task<TAntecedentTaskResult>)t));
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x0005363C File Offset: 0x0005183C
		internal static Task FollowOnSuccessWithTask(this Task antecedentTask, Func<Task, Task> operation)
		{
			TaskCompletionSource<Task> taskCompletionSource = new TaskCompletionSource<Task>();
			antecedentTask.ContinueWith(delegate(Task taskToContinueOn)
			{
				TaskUtils.FollowOnSuccessWithContinuation<Task>(taskToContinueOn, taskCompletionSource, operation);
			}, TaskContinuationOptions.ExecuteSynchronously);
			return taskCompletionSource.Task.Unwrap();
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x000536A8 File Offset: 0x000518A8
		internal static Task<TFollowupTaskResult> FollowOnSuccessWithTask<TFollowupTaskResult>(this Task antecedentTask, Func<Task, Task<TFollowupTaskResult>> operation)
		{
			TaskCompletionSource<Task<TFollowupTaskResult>> taskCompletionSource = new TaskCompletionSource<Task<TFollowupTaskResult>>();
			antecedentTask.ContinueWith(delegate(Task taskToContinueOn)
			{
				TaskUtils.FollowOnSuccessWithContinuation<Task<TFollowupTaskResult>>(taskToContinueOn, taskCompletionSource, operation);
			}, TaskContinuationOptions.ExecuteSynchronously);
			return taskCompletionSource.Task.Unwrap<TFollowupTaskResult>();
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x0005372C File Offset: 0x0005192C
		internal static Task FollowOnSuccessWithTask<TAntecedentTaskResult>(this Task<TAntecedentTaskResult> antecedentTask, Func<Task<TAntecedentTaskResult>, Task> operation)
		{
			TaskCompletionSource<Task> taskCompletionSource = new TaskCompletionSource<Task>();
			antecedentTask.ContinueWith(delegate(Task<TAntecedentTaskResult> taskToContinueOn)
			{
				TaskUtils.FollowOnSuccessWithContinuation<Task>(taskToContinueOn, taskCompletionSource, (Task taskForOperation) => operation((Task<TAntecedentTaskResult>)taskForOperation));
			}, TaskContinuationOptions.ExecuteSynchronously);
			return taskCompletionSource.Task.Unwrap();
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x000537B0 File Offset: 0x000519B0
		internal static Task<TFollowupTaskResult> FollowOnSuccessWithTask<TAntecedentTaskResult, TFollowupTaskResult>(this Task<TAntecedentTaskResult> antecedentTask, Func<Task<TAntecedentTaskResult>, Task<TFollowupTaskResult>> operation)
		{
			TaskCompletionSource<Task<TFollowupTaskResult>> taskCompletionSource = new TaskCompletionSource<Task<TFollowupTaskResult>>();
			antecedentTask.ContinueWith(delegate(Task<TAntecedentTaskResult> taskToContinueOn)
			{
				TaskUtils.FollowOnSuccessWithContinuation<Task<TFollowupTaskResult>>(taskToContinueOn, taskCompletionSource, (Task taskForOperation) => operation((Task<TAntecedentTaskResult>)taskForOperation));
			}, TaskContinuationOptions.ExecuteSynchronously);
			return taskCompletionSource.Task.Unwrap<TFollowupTaskResult>();
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x00053800 File Offset: 0x00051A00
		internal static Task FollowOnFaultWith(this Task antecedentTask, Action<Task> operation)
		{
			return TaskUtils.FollowOnFaultWithImplementation<object>(antecedentTask, (Task t) => null, operation);
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x00053850 File Offset: 0x00051A50
		internal static Task<TResult> FollowOnFaultWith<TResult>(this Task<TResult> antecedentTask, Action<Task<TResult>> operation)
		{
			return TaskUtils.FollowOnFaultWithImplementation<TResult>(antecedentTask, (Task t) => ((Task<TResult>)t).Result, delegate(Task t)
			{
				operation((Task<TResult>)t);
			});
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x00053895 File Offset: 0x00051A95
		internal static Task<TResult> FollowOnFaultAndCatchExceptionWith<TResult, TExceptionType>(this Task<TResult> antecedentTask, Func<TExceptionType, TResult> catchBlock) where TExceptionType : Exception
		{
			return TaskUtils.FollowOnFaultAndCatchExceptionWithImplementation<TResult, TExceptionType>(antecedentTask, (Task t) => ((Task<TResult>)t).Result, catchBlock);
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x000538AD File Offset: 0x00051AAD
		internal static Task FollowAlwaysWith(this Task antecedentTask, Action<Task> operation)
		{
			return antecedentTask.FollowAlwaysWithImplementation((Task t) => null, operation);
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x000538FC File Offset: 0x00051AFC
		internal static Task<TResult> FollowAlwaysWith<TResult>(this Task<TResult> antecedentTask, Action<Task<TResult>> operation)
		{
			return antecedentTask.FollowAlwaysWithImplementation((Task t) => ((Task<TResult>)t).Result, delegate(Task t)
			{
				operation((Task<TResult>)t);
			});
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x0005393D File Offset: 0x00051B3D
		internal static Task IgnoreExceptions(this Task task)
		{
			task.ContinueWith(delegate(Task t)
			{
				AggregateException exception = t.Exception;
			}, CancellationToken.None, TaskContinuationOptions.NotOnRanToCompletion | TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
			return task;
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x00053973 File Offset: 0x00051B73
		internal static TaskScheduler GetTargetScheduler(this TaskFactory factory)
		{
			return factory.Scheduler ?? TaskScheduler.Current;
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x00053A74 File Offset: 0x00051C74
		internal static Task Iterate(this TaskFactory factory, IEnumerable<Task> source)
		{
			IEnumerator<Task> enumerator = source.GetEnumerator();
			TaskCompletionSource<object> trc = new TaskCompletionSource<object>(null, factory.CreationOptions);
			trc.Task.ContinueWith(delegate(Task<object> _)
			{
				enumerator.Dispose();
			}, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
			Action<Task> recursiveBody = null;
			recursiveBody = delegate(Task antecedent)
			{
				try
				{
					if (antecedent != null && antecedent.IsFaulted)
					{
						trc.TrySetException(antecedent.Exception);
					}
					else if (enumerator.MoveNext())
					{
						Task task = enumerator.Current;
						task.ContinueWith(recursiveBody).IgnoreExceptions();
					}
					else
					{
						trc.TrySetResult(null);
					}
				}
				catch (Exception ex)
				{
					if (!ExceptionUtils.IsCatchableExceptionType(ex))
					{
						throw;
					}
					OperationCanceledException ex2 = ex as OperationCanceledException;
					if (ex2 != null && ex2.CancellationToken == factory.CancellationToken)
					{
						trc.TrySetCanceled();
					}
					else
					{
						trc.TrySetException(ex);
					}
				}
			};
			factory.StartNew(delegate()
			{
				recursiveBody(null);
			}, CancellationToken.None, TaskCreationOptions.None, factory.GetTargetScheduler()).IgnoreExceptions();
			return trc.Task;
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x00053B30 File Offset: 0x00051D30
		private static void FollowOnSuccessWithContinuation<TResult>(Task antecedentTask, TaskCompletionSource<TResult> taskCompletionSource, Func<Task, TResult> operation)
		{
			switch (antecedentTask.Status)
			{
			case TaskStatus.RanToCompletion:
				try
				{
					taskCompletionSource.TrySetResult(operation(antecedentTask));
					return;
				}
				catch (Exception ex)
				{
					if (!ExceptionUtils.IsCatchableExceptionType(ex))
					{
						throw;
					}
					taskCompletionSource.TrySetException(ex);
					return;
				}
				break;
			case TaskStatus.Canceled:
				taskCompletionSource.TrySetCanceled();
				return;
			case TaskStatus.Faulted:
				break;
			default:
				return;
			}
			taskCompletionSource.TrySetException(antecedentTask.Exception);
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x00053BC0 File Offset: 0x00051DC0
		private static Task<TResult> FollowOnSuccessWithImplementation<TResult>(Task antecedentTask, Func<Task, TResult> operation)
		{
			TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
			antecedentTask.ContinueWith(delegate(Task taskToContinueOn)
			{
				TaskUtils.FollowOnSuccessWithContinuation<TResult>(taskToContinueOn, taskCompletionSource, operation);
			}, TaskContinuationOptions.ExecuteSynchronously).IgnoreExceptions();
			return taskCompletionSource.Task;
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x00053CCC File Offset: 0x00051ECC
		private static Task<TResult> FollowOnFaultWithImplementation<TResult>(Task antecedentTask, Func<Task, TResult> getTaskResult, Action<Task> operation)
		{
			TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
			antecedentTask.ContinueWith(delegate(Task t)
			{
				switch (t.Status)
				{
				case TaskStatus.RanToCompletion:
					taskCompletionSource.TrySetResult(getTaskResult(t));
					return;
				case TaskStatus.Canceled:
					break;
				case TaskStatus.Faulted:
					try
					{
						operation(t);
						taskCompletionSource.TrySetException(t.Exception);
						return;
					}
					catch (Exception ex)
					{
						if (!ExceptionUtils.IsCatchableExceptionType(ex))
						{
							throw;
						}
						AggregateException exception = new AggregateException(new Exception[]
						{
							t.Exception,
							ex
						});
						taskCompletionSource.TrySetException(exception);
						return;
					}
					break;
				default:
					return;
				}
				taskCompletionSource.TrySetCanceled();
			}, TaskContinuationOptions.ExecuteSynchronously).IgnoreExceptions();
			return taskCompletionSource.Task;
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x00053E24 File Offset: 0x00052024
		private static Task<TResult> FollowOnFaultAndCatchExceptionWithImplementation<TResult, TExceptionType>(Task antecedentTask, Func<Task, TResult> getTaskResult, Func<TExceptionType, TResult> catchBlock) where TExceptionType : Exception
		{
			TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
			antecedentTask.ContinueWith(delegate(Task t)
			{
				switch (t.Status)
				{
				case TaskStatus.RanToCompletion:
					taskCompletionSource.TrySetResult(getTaskResult(t));
					return;
				case TaskStatus.Canceled:
					taskCompletionSource.TrySetCanceled();
					break;
				case TaskStatus.Faulted:
				{
					Exception ex = t.Exception;
					AggregateException ex2 = ex as AggregateException;
					if (ex2 != null)
					{
						ex2 = ex2.Flatten();
						if (ex2.InnerExceptions.Count == 1)
						{
							ex = ex2.InnerExceptions[0];
						}
					}
					if (ex is TExceptionType)
					{
						try
						{
							taskCompletionSource.TrySetResult(catchBlock((TExceptionType)((object)ex)));
							break;
						}
						catch (Exception ex3)
						{
							if (!ExceptionUtils.IsCatchableExceptionType(ex3))
							{
								throw;
							}
							AggregateException exception = new AggregateException(new Exception[]
							{
								ex,
								ex3
							});
							taskCompletionSource.TrySetException(exception);
							break;
						}
					}
					taskCompletionSource.TrySetException(ex);
					return;
				}
				default:
					return;
				}
			}, TaskContinuationOptions.ExecuteSynchronously).IgnoreExceptions();
			return taskCompletionSource.Task;
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x00053F54 File Offset: 0x00052154
		private static Task<TResult> FollowAlwaysWithImplementation<TResult>(this Task antecedentTask, Func<Task, TResult> getTaskResult, Action<Task> operation)
		{
			TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
			antecedentTask.ContinueWith(delegate(Task t)
			{
				Exception ex = null;
				try
				{
					operation(t);
				}
				catch (Exception ex2)
				{
					if (!ExceptionUtils.IsCatchableExceptionType(ex2))
					{
						throw;
					}
					ex = ex2;
				}
				switch (t.Status)
				{
				case TaskStatus.RanToCompletion:
					if (ex != null)
					{
						taskCompletionSource.TrySetException(ex);
						return;
					}
					taskCompletionSource.TrySetResult(getTaskResult(t));
					return;
				case TaskStatus.Canceled:
					if (ex != null)
					{
						taskCompletionSource.TrySetException(ex);
						return;
					}
					taskCompletionSource.TrySetCanceled();
					return;
				case TaskStatus.Faulted:
				{
					Exception ex3 = t.Exception;
					if (ex != null)
					{
						ex3 = new AggregateException(new Exception[]
						{
							ex3,
							ex
						});
					}
					taskCompletionSource.TrySetException(ex3);
					return;
				}
				default:
					return;
				}
			}, TaskContinuationOptions.ExecuteSynchronously).IgnoreExceptions();
			return taskCompletionSource.Task;
		}

		// Token: 0x040009A2 RID: 2466
		private static Task completedTask;
	}
}
