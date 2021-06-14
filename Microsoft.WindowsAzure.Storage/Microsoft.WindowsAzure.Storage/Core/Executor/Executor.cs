using System;
using System.IO;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Core.Executor
{
	// Token: 0x02000068 RID: 104
	internal class Executor : ExecutorBase
	{
		// Token: 0x06000DDC RID: 3548 RVA: 0x00033324 File Offset: 0x00031524
		public static ICancellableAsyncResult BeginExecuteAsync<T>(RESTCommand<T> cmd, IRetryPolicy policy, OperationContext operationContext, AsyncCallback callback, object asyncState)
		{
			ExecutionState<T> executionState = new ExecutionState<T>(cmd, policy, operationContext, callback, asyncState);
			Executor.InitRequest<T>(executionState);
			return executionState;
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x00033344 File Offset: 0x00031544
		public static T EndExecuteAsync<T>(IAsyncResult result)
		{
			CommonUtility.AssertNotNull("result", result);
			T result2;
			using (ExecutionState<T> executionState = (ExecutionState<T>)result)
			{
				executionState.End();
				executionState.RestCMD.SendStream = null;
				executionState.RestCMD.DestinationStream = null;
				if (executionState.ExceptionRef != null)
				{
					throw executionState.ExceptionRef;
				}
				result2 = executionState.Result;
			}
			return result2;
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x000333B4 File Offset: 0x000315B4
		public static void InitRequest<T>(ExecutionState<T> executionState)
		{
			try
			{
				executionState.Init();
				ExecutorBase.StartRequestAttempt<T>(executionState);
				Executor.ProcessStartOfRequest<T>(executionState, "Starting asynchronous request to {0}.");
				if (ExecutorBase.CheckTimeout<T>(executionState, false))
				{
					Executor.EndOperation<T>(executionState);
				}
				else if (ExecutorBase.CheckCancellation<T>(executionState))
				{
					Executor.EndOperation<T>(executionState);
				}
				else if (executionState.RestCMD.SendStream != null)
				{
					Executor.BeginGetRequestStream<T>(executionState);
				}
				else
				{
					Executor.BeginGetResponse<T>(executionState);
				}
			}
			catch (Exception ex)
			{
				Logger.LogError(executionState.OperationContext, "Exception thrown while initializing request: {0}.", new object[]
				{
					ex.Message
				});
				StorageException ex2 = ExecutorBase.TranslateExceptionBasedOnParseError(ex, executionState.Cmd.CurrentResult, executionState.Resp, executionState.Cmd.ParseError);
				ex2.IsRetryable = false;
				executionState.ExceptionRef = ex2;
				executionState.OnComplete();
			}
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x00033484 File Offset: 0x00031684
		private static void BeginGetRequestStream<T>(ExecutionState<T> executionState)
		{
			executionState.CurrentOperation = ExecutorOperation.BeginGetRequestStream;
			Logger.LogInformational(executionState.OperationContext, "Preparing to write request data.", new object[0]);
			try
			{
				APMWithTimeout.RunWithTimeout(new Func<AsyncCallback, object, IAsyncResult>(executionState.Req.BeginGetRequestStream), new AsyncCallback(Executor.EndGetRequestStream<T>), new TimerCallback(Executor.AbortRequest<T>), executionState, executionState.RemainingTimeout);
			}
			catch (Exception ex)
			{
				Logger.LogWarning(executionState.OperationContext, "Exception thrown while preparing to write request data: {0}.", new object[]
				{
					ex.Message
				});
				executionState.ExceptionRef = ExecutorBase.TranslateExceptionBasedOnParseError(ex, executionState.Cmd.CurrentResult, executionState.Resp, executionState.Cmd.ParseError);
				Executor.EndOperation<T>(executionState);
			}
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x00033548 File Offset: 0x00031748
		private static void EndGetRequestStream<T>(IAsyncResult getRequestStreamResult)
		{
			ExecutionState<T> executionState = (ExecutionState<T>)getRequestStreamResult.AsyncState;
			executionState.CurrentOperation = ExecutorOperation.EndGetRequestStream;
			try
			{
				executionState.UpdateCompletedSynchronously(getRequestStreamResult.CompletedSynchronously);
				executionState.ReqStream = executionState.Req.EndGetRequestStream(getRequestStreamResult);
				executionState.CurrentOperation = ExecutorOperation.BeginUploadRequest;
				Logger.LogInformational(executionState.OperationContext, "Writing request data.", new object[0]);
				MultiBufferMemoryStream multiBufferMemoryStream = executionState.RestCMD.SendStream as MultiBufferMemoryStream;
				if (multiBufferMemoryStream != null && executionState.RestCMD.SendStreamLength == null)
				{
					multiBufferMemoryStream.BeginFastCopyTo(executionState.ReqStream, executionState.OperationExpiryTime, new AsyncCallback(Executor.EndFastCopyTo<T>), executionState);
				}
				else
				{
					executionState.RestCMD.SendStream.WriteToAsync(executionState.ReqStream, executionState.RestCMD.SendStreamLength, null, false, executionState, null, new Action<ExecutionState<T>>(Executor.EndSendStreamCopy<T>));
				}
			}
			catch (Exception ex)
			{
				Logger.LogWarning(executionState.OperationContext, "Exception thrown while writing request data: {0}.", new object[]
				{
					ex.Message
				});
				executionState.ExceptionRef = ExecutorBase.TranslateExceptionBasedOnParseError(ex, executionState.Cmd.CurrentResult, executionState.Resp, executionState.Cmd.ParseError);
				Executor.EndOperation<T>(executionState);
			}
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0003368C File Offset: 0x0003188C
		private static void EndFastCopyTo<T>(IAsyncResult fastCopyToResult)
		{
			ExecutionState<T> executionState = (ExecutionState<T>)fastCopyToResult.AsyncState;
			try
			{
				executionState.UpdateCompletedSynchronously(fastCopyToResult.CompletedSynchronously);
				MultiBufferMemoryStream multiBufferMemoryStream = (MultiBufferMemoryStream)executionState.RestCMD.SendStream;
				multiBufferMemoryStream.EndFastCopyTo(fastCopyToResult);
				Executor.EndSendStreamCopy<T>(executionState);
			}
			catch (Exception ex)
			{
				Logger.LogWarning(executionState.OperationContext, "Exception thrown while writing request data: {0}.", new object[]
				{
					ex.Message
				});
				executionState.ExceptionRef = ExecutorBase.TranslateExceptionBasedOnParseError(ex, executionState.Cmd.CurrentResult, executionState.Resp, executionState.Cmd.ParseError);
				Executor.EndOperation<T>(executionState);
			}
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00033734 File Offset: 0x00031934
		private static void EndSendStreamCopy<T>(ExecutionState<T> executionState)
		{
			executionState.CurrentOperation = ExecutorOperation.EndUploadRequest;
			ExecutorBase.CheckCancellation<T>(executionState);
			if (executionState.ExceptionRef != null)
			{
				try
				{
					executionState.Req.Abort();
				}
				catch (Exception)
				{
				}
				Executor.EndOperation<T>(executionState);
				return;
			}
			try
			{
				executionState.ReqStream.Flush();
				executionState.ReqStream.Dispose();
				executionState.ReqStream = null;
			}
			catch (Exception)
			{
			}
			Executor.BeginGetResponse<T>(executionState);
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x000337B4 File Offset: 0x000319B4
		private static void BeginGetResponse<T>(ExecutionState<T> executionState)
		{
			executionState.CurrentOperation = ExecutorOperation.BeginGetResponse;
			Logger.LogInformational(executionState.OperationContext, "Waiting for response.", new object[0]);
			try
			{
				APMWithTimeout.RunWithTimeout(new Func<AsyncCallback, object, IAsyncResult>(executionState.Req.BeginGetResponse), new AsyncCallback(Executor.EndGetResponse<T>), new TimerCallback(Executor.AbortRequest<T>), executionState, executionState.RemainingTimeout);
			}
			catch (Exception ex)
			{
				Logger.LogWarning(executionState.OperationContext, "Exception thrown while waiting for response: {0}.", new object[]
				{
					ex.Message
				});
				executionState.ExceptionRef = ExecutorBase.TranslateExceptionBasedOnParseError(ex, executionState.Cmd.CurrentResult, executionState.Resp, executionState.Cmd.ParseError);
				Executor.EndOperation<T>(executionState);
			}
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x00033878 File Offset: 0x00031A78
		private static void EndGetResponse<T>(IAsyncResult getResponseResult)
		{
			ExecutionState<T> executionState = (ExecutionState<T>)getResponseResult.AsyncState;
			executionState.CurrentOperation = ExecutorOperation.EndGetResponse;
			try
			{
				executionState.UpdateCompletedSynchronously(getResponseResult.CompletedSynchronously);
				try
				{
					executionState.Resp = (executionState.Req.EndGetResponse(getResponseResult) as HttpWebResponse);
				}
				catch (WebException ex)
				{
					Logger.LogWarning(executionState.OperationContext, "Exception thrown while waiting for response: {0}.", new object[]
					{
						ex.Message
					});
					executionState.Resp = (HttpWebResponse)ex.Response;
					if (ex.Status == WebExceptionStatus.Timeout || executionState.ReqTimedOut)
					{
						throw new TimeoutException();
					}
					if (executionState.Resp == null)
					{
						throw;
					}
					executionState.ExceptionRef = ex;
				}
				Logger.LogInformational(executionState.OperationContext, "Response received. Status code = {0}, Request ID = {1}, Content-MD5 = {2}, ETag = {3}.", new object[]
				{
					executionState.Cmd.CurrentResult.HttpStatusCode,
					executionState.Cmd.CurrentResult.ServiceRequestID,
					executionState.Cmd.CurrentResult.ContentMd5,
					executionState.Cmd.CurrentResult.Etag
				});
				ExecutorBase.FireResponseReceived<T>(executionState);
				if (executionState.RestCMD.PreProcessResponse != null)
				{
					executionState.CurrentOperation = ExecutorOperation.PreProcess;
					try
					{
						executionState.Result = executionState.RestCMD.PreProcessResponse(executionState.RestCMD, executionState.Resp, executionState.ExceptionRef, executionState.OperationContext);
						executionState.ExceptionRef = null;
						Logger.LogInformational(executionState.OperationContext, "Response headers were processed successfully, proceeding with the rest of the operation.", new object[0]);
					}
					catch (Exception exceptionRef)
					{
						executionState.ExceptionRef = exceptionRef;
					}
				}
				ExecutorBase.CheckCancellation<T>(executionState);
				executionState.CurrentOperation = ExecutorOperation.GetResponseStream;
				executionState.RestCMD.ResponseStream = executionState.Resp.GetResponseStream();
				if (executionState.ExceptionRef != null)
				{
					executionState.CurrentOperation = ExecutorOperation.BeginDownloadResponse;
					Logger.LogInformational(executionState.OperationContext, "Downloading error response body.", new object[0]);
					executionState.RestCMD.ErrorStream = new MemoryStream();
					executionState.RestCMD.ResponseStream.WriteToAsync(executionState.RestCMD.ErrorStream, null, null, false, executionState, new StreamDescriptor(), new Action<ExecutionState<T>>(Executor.EndResponseStreamCopy<T>));
				}
				else
				{
					if (!executionState.RestCMD.RetrieveResponseStream)
					{
						executionState.RestCMD.DestinationStream = Stream.Null;
					}
					if (executionState.RestCMD.DestinationStream != null)
					{
						if (executionState.RestCMD.StreamCopyState == null)
						{
							executionState.RestCMD.StreamCopyState = new StreamDescriptor();
						}
						executionState.CurrentOperation = ExecutorOperation.BeginDownloadResponse;
						Logger.LogInformational(executionState.OperationContext, "Downloading response body.", new object[0]);
						executionState.RestCMD.ResponseStream.WriteToAsync(executionState.RestCMD.DestinationStream, null, null, executionState.RestCMD.CalculateMd5ForResponseStream, executionState, executionState.RestCMD.StreamCopyState, new Action<ExecutionState<T>>(Executor.EndResponseStreamCopy<T>));
					}
					else
					{
						Executor.EndOperation<T>(executionState);
					}
				}
			}
			catch (Exception ex2)
			{
				Logger.LogWarning(executionState.OperationContext, "Exception thrown while processing response: {0}.", new object[]
				{
					ex2.Message
				});
				executionState.ExceptionRef = ExecutorBase.TranslateExceptionBasedOnParseError(ex2, executionState.Cmd.CurrentResult, executionState.Resp, executionState.Cmd.ParseError);
				Executor.EndOperation<T>(executionState);
			}
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x00033C34 File Offset: 0x00031E34
		private static void EndResponseStreamCopy<T>(ExecutionState<T> executionState)
		{
			if (executionState.RestCMD.ErrorStream != null)
			{
				executionState.RestCMD.ErrorStream.Seek(0L, SeekOrigin.Begin);
				if (executionState.Cmd.ParseError != null)
				{
					executionState.ExceptionRef = StorageException.TranslateExceptionWithPreBufferedStream(executionState.ExceptionRef, executionState.Cmd.CurrentResult, (Stream stream) => executionState.Cmd.ParseError(stream, executionState.Resp, null), executionState.RestCMD.ErrorStream);
				}
				else
				{
					executionState.ExceptionRef = StorageException.TranslateExceptionWithPreBufferedStream(executionState.ExceptionRef, executionState.Cmd.CurrentResult, null, executionState.RestCMD.ErrorStream);
				}
				try
				{
					executionState.RestCMD.ErrorStream.Dispose();
					executionState.RestCMD.ErrorStream = null;
				}
				catch (Exception)
				{
				}
			}
			try
			{
				if (executionState.RestCMD.ResponseStream != null)
				{
					executionState.RestCMD.ResponseStream.Dispose();
					executionState.RestCMD.ResponseStream = null;
				}
			}
			catch (Exception)
			{
			}
			executionState.CurrentOperation = ExecutorOperation.EndDownloadResponse;
			Executor.EndOperation<T>(executionState);
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x00033E20 File Offset: 0x00032020
		private static void EndOperation<T>(ExecutionState<T> executionState)
		{
			ExecutorBase.FinishRequestAttempt<T>(executionState);
			try
			{
				ExecutorBase.CheckCancellation<T>(executionState);
				ExecutorBase.CheckTimeout<T>(executionState, true);
				if (executionState.ExceptionRef == null)
				{
					Executor.ProcessEndOfRequest<T>(executionState);
					executionState.OnComplete();
					return;
				}
			}
			catch (Exception ex)
			{
				Logger.LogWarning(executionState.OperationContext, "Exception thrown while ending operation: {0}.", new object[]
				{
					ex.Message
				});
				executionState.ExceptionRef = ExecutorBase.TranslateExceptionBasedOnParseError(ex, executionState.Cmd.CurrentResult, executionState.Resp, executionState.Cmd.ParseError);
			}
			finally
			{
				try
				{
					if (executionState.ReqStream != null)
					{
						executionState.ReqStream.Dispose();
						executionState.ReqStream = null;
					}
					if (executionState.Resp != null)
					{
						executionState.Resp.Close();
						executionState.Resp = null;
					}
				}
				catch (Exception)
				{
				}
			}
			try
			{
				StorageException ex2 = ExecutorBase.TranslateExceptionBasedOnParseError(executionState.ExceptionRef, executionState.Cmd.CurrentResult, executionState.Resp, executionState.Cmd.ParseError);
				executionState.ExceptionRef = ex2;
				Logger.LogInformational(executionState.OperationContext, "Checking if the operation should be retried. Retry count = {0}, HTTP status code = {1}, Retryable exception = {2}, Exception = {3}.", new object[]
				{
					executionState.RetryCount,
					executionState.Cmd.CurrentResult.HttpStatusCode,
					ex2.IsRetryable ? "yes" : "no",
					ex2.Message
				});
				bool flag = false;
				TimeSpan timeSpan = TimeSpan.Zero;
				if (ex2.IsRetryable && executionState.RetryPolicy != null)
				{
					executionState.CurrentLocation = ExecutorBase.GetNextLocation(executionState.CurrentLocation, executionState.RestCMD.LocationMode);
					Logger.LogInformational(executionState.OperationContext, "The next location has been set to {0}, based on the location mode.", new object[]
					{
						executionState.CurrentLocation
					});
					IExtendedRetryPolicy extendedRetryPolicy = executionState.RetryPolicy as IExtendedRetryPolicy;
					if (extendedRetryPolicy != null)
					{
						RetryContext retryContext = new RetryContext(executionState.RetryCount++, executionState.Cmd.CurrentResult, executionState.CurrentLocation, executionState.RestCMD.LocationMode);
						RetryInfo retryInfo = extendedRetryPolicy.Evaluate(retryContext, executionState.OperationContext);
						if (retryInfo != null)
						{
							Logger.LogInformational(executionState.OperationContext, "The extended retry policy set the next location to {0} and updated the location mode to {1}.", new object[]
							{
								retryInfo.TargetLocation,
								retryInfo.UpdatedLocationMode
							});
							flag = true;
							executionState.CurrentLocation = retryInfo.TargetLocation;
							executionState.RestCMD.LocationMode = retryInfo.UpdatedLocationMode;
							timeSpan = retryInfo.RetryInterval;
						}
					}
					else
					{
						flag = executionState.RetryPolicy.ShouldRetry(executionState.RetryCount++, executionState.Cmd.CurrentResult.HttpStatusCode, executionState.ExceptionRef, out timeSpan, executionState.OperationContext);
					}
					if (timeSpan < TimeSpan.Zero || timeSpan > Constants.MaximumRetryBackoff)
					{
						timeSpan = Constants.MaximumRetryBackoff;
					}
				}
				if (!flag || (executionState.OperationExpiryTime != null && (DateTime.Now + timeSpan).CompareTo(executionState.OperationExpiryTime.Value) > 0))
				{
					Logger.LogError(executionState.OperationContext, flag ? "Operation cannot be retried because the maximum execution time has been reached. Failing with {0}." : "Retry policy did not allow for a retry. Failing with {0}.", new object[]
					{
						executionState.ExceptionRef.Message
					});
					executionState.OnComplete();
				}
				else
				{
					if (executionState.Cmd.RecoveryAction != null)
					{
						executionState.Cmd.RecoveryAction(executionState.Cmd, executionState.ExceptionRef, executionState.OperationContext);
					}
					if (timeSpan > TimeSpan.Zero)
					{
						Logger.LogInformational(executionState.OperationContext, "Operation will be retried after {0}ms.", new object[]
						{
							(int)timeSpan.TotalMilliseconds
						});
						executionState.UpdateCompletedSynchronously(false);
						if (executionState.BackoffTimer == null)
						{
							executionState.BackoffTimer = new Timer(new TimerCallback(Executor.RetryRequest<T>), executionState, (int)timeSpan.TotalMilliseconds, -1);
						}
						else
						{
							executionState.BackoffTimer.Change((int)timeSpan.TotalMilliseconds, -1);
						}
						executionState.CancelDelegate = delegate()
						{
							Timer backoffTimer = executionState.BackoffTimer;
							if (backoffTimer != null)
							{
								executionState.BackoffTimer = null;
								backoffTimer.Dispose();
							}
							Logger.LogWarning(executionState.OperationContext, "Aborting pending retry due to user request.", new object[0]);
							ExecutorBase.CheckCancellation<T>(executionState);
							executionState.OnComplete();
						};
					}
					else
					{
						Executor.RetryRequest<T>(executionState);
					}
				}
			}
			catch (Exception ex3)
			{
				Logger.LogWarning(executionState.OperationContext, "Exception thrown while retrying operation: {0}.", new object[]
				{
					ex3.Message
				});
				executionState.ExceptionRef = ExecutorBase.TranslateExceptionBasedOnParseError(ex3, executionState.Cmd.CurrentResult, executionState.Resp, executionState.Cmd.ParseError);
				executionState.OnComplete();
			}
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x000344BC File Offset: 0x000326BC
		private static void RetryRequest<T>(object state)
		{
			ExecutionState<T> executionState = (ExecutionState<T>)state;
			Logger.LogInformational(executionState.OperationContext, "Retrying failed operation.", new object[0]);
			ExecutorBase.FireRetrying<T>(executionState);
			Executor.InitRequest<T>(executionState);
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x000344F4 File Offset: 0x000326F4
		private static void AbortRequest<T>(object state)
		{
			ExecutionState<T> executionState = (ExecutionState<T>)state;
			Logger.LogInformational(executionState.OperationContext, "Aborting pending request due to timeout.", new object[0]);
			try
			{
				executionState.ReqTimedOut = true;
				executionState.Req.Abort();
			}
			catch (Exception ex)
			{
				Logger.LogError(executionState.OperationContext, "Could not abort pending request because of {0}.", new object[]
				{
					ex.Message
				});
			}
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00034568 File Offset: 0x00032768
		public static T ExecuteSync<T>(RESTCommand<T> cmd, IRetryPolicy policy, OperationContext operationContext)
		{
			using (ExecutionState<T> executionState = new ExecutionState<T>(cmd, policy, operationContext))
			{
				bool flag = false;
				TimeSpan timeSpan = TimeSpan.Zero;
				for (;;)
				{
					try
					{
						executionState.Init();
						ExecutorBase.StartRequestAttempt<T>(executionState);
						Executor.ProcessStartOfRequest<T>(executionState, "Starting synchronous request to {0}.");
						ExecutorBase.CheckTimeout<T>(executionState, true);
					}
					catch (Exception ex)
					{
						Logger.LogError(executionState.OperationContext, "Exception thrown while initializing request: {0}.", new object[]
						{
							ex.Message
						});
						StorageException ex2 = ExecutorBase.TranslateExceptionBasedOnParseError(ex, executionState.Cmd.CurrentResult, executionState.Resp, executionState.Cmd.ParseError);
						ex2.IsRetryable = false;
						executionState.ExceptionRef = ex2;
						throw executionState.ExceptionRef;
					}
					try
					{
						if (cmd.SendStream != null)
						{
							executionState.CurrentOperation = ExecutorOperation.BeginGetRequestStream;
							Logger.LogInformational(executionState.OperationContext, "Preparing to write request data.", new object[0]);
							executionState.Req.Timeout = (int)executionState.RemainingTimeout.TotalMilliseconds;
							executionState.ReqStream = executionState.Req.GetRequestStream();
							executionState.CurrentOperation = ExecutorOperation.BeginUploadRequest;
							Logger.LogInformational(executionState.OperationContext, "Writing request data.", new object[0]);
							MultiBufferMemoryStream multiBufferMemoryStream = cmd.SendStream as MultiBufferMemoryStream;
							try
							{
								if (multiBufferMemoryStream != null && cmd.SendStreamLength == null)
								{
									multiBufferMemoryStream.FastCopyTo(executionState.ReqStream, executionState.OperationExpiryTime);
								}
								else
								{
									executionState.RestCMD.SendStream.WriteToSync(executionState.ReqStream, cmd.SendStreamLength, null, false, true, executionState, null);
								}
								executionState.ReqStream.Flush();
								executionState.ReqStream.Dispose();
								executionState.ReqStream = null;
							}
							catch (Exception)
							{
								executionState.Req.Abort();
								throw;
							}
						}
						try
						{
							executionState.CurrentOperation = ExecutorOperation.BeginGetResponse;
							Logger.LogInformational(executionState.OperationContext, "Waiting for response.", new object[0]);
							executionState.Req.Timeout = (int)executionState.RemainingTimeout.TotalMilliseconds;
							executionState.Resp = (HttpWebResponse)executionState.Req.GetResponse();
							executionState.CurrentOperation = ExecutorOperation.EndGetResponse;
						}
						catch (WebException ex3)
						{
							Logger.LogWarning(executionState.OperationContext, "Exception thrown while waiting for response: {0}.", new object[]
							{
								ex3.Message
							});
							executionState.Resp = (HttpWebResponse)ex3.Response;
							if (ex3.Status == WebExceptionStatus.Timeout || executionState.ReqTimedOut)
							{
								throw new TimeoutException();
							}
							if (executionState.Resp == null)
							{
								throw;
							}
							executionState.ExceptionRef = ExecutorBase.TranslateExceptionBasedOnParseError(ex3, executionState.Cmd.CurrentResult, executionState.Resp, executionState.Cmd.ParseError);
						}
						Logger.LogInformational(executionState.OperationContext, "Response received. Status code = {0}, Request ID = {1}, Content-MD5 = {2}, ETag = {3}.", new object[]
						{
							executionState.Cmd.CurrentResult.HttpStatusCode,
							executionState.Cmd.CurrentResult.ServiceRequestID,
							executionState.Cmd.CurrentResult.ContentMd5,
							executionState.Cmd.CurrentResult.Etag
						});
						ExecutorBase.FireResponseReceived<T>(executionState);
						if (cmd.PreProcessResponse != null)
						{
							executionState.CurrentOperation = ExecutorOperation.PreProcess;
							executionState.Result = cmd.PreProcessResponse(cmd, executionState.Resp, executionState.ExceptionRef, executionState.OperationContext);
							executionState.ExceptionRef = null;
							Logger.LogInformational(executionState.OperationContext, "Response headers were processed successfully, proceeding with the rest of the operation.", new object[0]);
						}
						executionState.CurrentOperation = ExecutorOperation.GetResponseStream;
						cmd.ResponseStream = executionState.Resp.GetResponseStream();
						if (!cmd.RetrieveResponseStream)
						{
							cmd.DestinationStream = Stream.Null;
						}
						if (cmd.DestinationStream != null)
						{
							if (cmd.StreamCopyState == null)
							{
								cmd.StreamCopyState = new StreamDescriptor();
							}
							try
							{
								executionState.CurrentOperation = ExecutorOperation.BeginDownloadResponse;
								Logger.LogInformational(executionState.OperationContext, "Downloading response body.", new object[0]);
								cmd.ResponseStream.WriteToSync(cmd.DestinationStream, null, null, cmd.CalculateMd5ForResponseStream, false, executionState, cmd.StreamCopyState);
							}
							finally
							{
								cmd.ResponseStream.Dispose();
								cmd.ResponseStream = null;
							}
						}
						Executor.ProcessEndOfRequest<T>(executionState);
						ExecutorBase.FinishRequestAttempt<T>(executionState);
						return executionState.Result;
					}
					catch (Exception ex4)
					{
						Logger.LogWarning(executionState.OperationContext, "Exception thrown during the operation: {0}.", new object[]
						{
							ex4.Message
						});
						ExecutorBase.FinishRequestAttempt<T>(executionState);
						StorageException ex5 = ExecutorBase.TranslateExceptionBasedOnParseError(ex4, executionState.Cmd.CurrentResult, executionState.Resp, executionState.Cmd.ParseError);
						executionState.ExceptionRef = ex5;
						Logger.LogInformational(executionState.OperationContext, "Checking if the operation should be retried. Retry count = {0}, HTTP status code = {1}, Retryable exception = {2}, Exception = {3}.", new object[]
						{
							executionState.RetryCount,
							executionState.Cmd.CurrentResult.HttpStatusCode,
							ex5.IsRetryable ? "yes" : "no",
							ex5.Message
						});
						flag = false;
						if (ex5.IsRetryable && executionState.RetryPolicy != null)
						{
							executionState.CurrentLocation = ExecutorBase.GetNextLocation(executionState.CurrentLocation, cmd.LocationMode);
							Logger.LogInformational(executionState.OperationContext, "The next location has been set to {0}, based on the location mode.", new object[]
							{
								executionState.CurrentLocation
							});
							IExtendedRetryPolicy extendedRetryPolicy = executionState.RetryPolicy as IExtendedRetryPolicy;
							if (extendedRetryPolicy != null)
							{
								RetryContext retryContext = new RetryContext(executionState.RetryCount++, cmd.CurrentResult, executionState.CurrentLocation, cmd.LocationMode);
								RetryInfo retryInfo = extendedRetryPolicy.Evaluate(retryContext, executionState.OperationContext);
								if (retryInfo != null)
								{
									Logger.LogInformational(executionState.OperationContext, "The extended retry policy set the next location to {0} and updated the location mode to {1}.", new object[]
									{
										retryInfo.TargetLocation,
										retryInfo.UpdatedLocationMode
									});
									flag = true;
									executionState.CurrentLocation = retryInfo.TargetLocation;
									cmd.LocationMode = retryInfo.UpdatedLocationMode;
									timeSpan = retryInfo.RetryInterval;
								}
							}
							else
							{
								flag = executionState.RetryPolicy.ShouldRetry(executionState.RetryCount++, cmd.CurrentResult.HttpStatusCode, executionState.ExceptionRef, out timeSpan, executionState.OperationContext);
							}
							if (timeSpan < TimeSpan.Zero || timeSpan > Constants.MaximumRetryBackoff)
							{
								timeSpan = Constants.MaximumRetryBackoff;
							}
						}
					}
					finally
					{
						if (executionState.Resp != null)
						{
							executionState.Resp.Close();
							executionState.Resp = null;
						}
					}
					if (!flag || (executionState.OperationExpiryTime != null && (DateTime.Now + timeSpan).CompareTo(executionState.OperationExpiryTime.Value) > 0))
					{
						break;
					}
					if (executionState.Cmd.RecoveryAction != null)
					{
						executionState.Cmd.RecoveryAction(executionState.Cmd, executionState.ExceptionRef, executionState.OperationContext);
					}
					if (timeSpan > TimeSpan.Zero)
					{
						Logger.LogInformational(executionState.OperationContext, "Operation will be retried after {0}ms.", new object[]
						{
							(int)timeSpan.TotalMilliseconds
						});
						Thread.Sleep(timeSpan);
					}
					Logger.LogInformational(executionState.OperationContext, "Retrying failed operation.", new object[0]);
					ExecutorBase.FireRetrying<T>(executionState);
					if (!flag)
					{
						goto Block_10;
					}
				}
				Logger.LogError(executionState.OperationContext, flag ? "Operation cannot be retried because the maximum execution time has been reached. Failing with {0}." : "Retry policy did not allow for a retry. Failing with {0}.", new object[]
				{
					executionState.ExceptionRef.Message
				});
				throw executionState.ExceptionRef;
				Block_10:;
			}
			throw new NotImplementedException("Unexpected internal storage client error.");
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x00034D80 File Offset: 0x00032F80
		private static void ProcessStartOfRequest<T>(ExecutionState<T> executionState, string startLogMessage)
		{
			executionState.CurrentOperation = ExecutorOperation.BeginOperation;
			Uri uri = executionState.RestCMD.StorageUri.GetUri(executionState.CurrentLocation);
			Uri uri2 = executionState.RestCMD.Credentials.TransformUri(uri);
			Logger.LogInformational(executionState.OperationContext, startLogMessage, new object[]
			{
				uri2
			});
			UriQueryBuilder arg = new UriQueryBuilder(executionState.RestCMD.Builder);
			executionState.Req = executionState.RestCMD.BuildRequestDelegate(uri2, arg, executionState.Cmd.ServerTimeoutInSeconds, true, executionState.OperationContext);
			executionState.CancelDelegate = new Action(executionState.Req.Abort);
			ExecutorBase.ApplyUserHeaders<T>(executionState);
			if (executionState.RestCMD.SetHeaders != null)
			{
				executionState.RestCMD.SetHeaders(executionState.Req, executionState.OperationContext);
			}
			if (executionState.RestCMD.SendStream != null)
			{
				long length = executionState.RestCMD.SendStreamLength ?? (executionState.RestCMD.SendStream.Length - executionState.RestCMD.SendStream.Position);
				CommonUtility.ApplyRequestOptimizations(executionState.Req, length);
			}
			else
			{
				CommonUtility.ApplyRequestOptimizations(executionState.Req, -1L);
			}
			ExecutorBase.FireSendingRequest<T>(executionState);
			if (executionState.RestCMD.SignRequest != null)
			{
				executionState.RestCMD.SignRequest(executionState.Req, executionState.OperationContext);
			}
			executionState.Req.Timeout = (int)executionState.RemainingTimeout.TotalMilliseconds;
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x00034F10 File Offset: 0x00033110
		private static void ProcessEndOfRequest<T>(ExecutionState<T> executionState)
		{
			if (executionState.RestCMD.PostProcessResponse != null)
			{
				executionState.CurrentOperation = ExecutorOperation.PostProcess;
				Logger.LogInformational(executionState.OperationContext, "Processing response body.", new object[0]);
				executionState.Result = executionState.RestCMD.PostProcessResponse(executionState.RestCMD, executionState.Resp, executionState.OperationContext);
			}
			if (executionState.RestCMD.DisposeAction != null)
			{
				Logger.LogInformational(executionState.OperationContext, "Disposing action invoked.", new object[0]);
				executionState.RestCMD.DisposeAction(executionState.RestCMD);
				executionState.RestCMD.DisposeAction = null;
			}
			executionState.CurrentOperation = ExecutorOperation.EndOperation;
			Logger.LogInformational(executionState.OperationContext, "Operation completed successfully.", new object[0]);
			executionState.CancelDelegate = null;
		}
	}
}
