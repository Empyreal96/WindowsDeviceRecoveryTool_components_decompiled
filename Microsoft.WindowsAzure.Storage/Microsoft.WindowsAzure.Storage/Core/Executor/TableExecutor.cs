using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure.Storage.Table.DataServices;

namespace Microsoft.WindowsAzure.Storage.Core.Executor
{
	// Token: 0x0200006C RID: 108
	[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
	internal class TableExecutor : ExecutorBase
	{
		// Token: 0x06000DF6 RID: 3574 RVA: 0x00035080 File Offset: 0x00033280
		public static ICancellableAsyncResult BeginExecuteAsync<T, INTERMEDIATE_TYPE>(TableCommand<T, INTERMEDIATE_TYPE> cmd, IRetryPolicy policy, OperationContext operationContext, AsyncCallback callback, object asyncState)
		{
			ExecutionState<T> executionState = new ExecutionState<T>(cmd, policy, operationContext, callback, asyncState);
			TableExecutor.AcquireContext<T>(cmd.Context, executionState);
			TableExecutor.InitRequest<T, INTERMEDIATE_TYPE>(executionState);
			return executionState;
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x000350AC File Offset: 0x000332AC
		public static T EndExecuteAsync<T, INTERMEDIATE_TYPE>(IAsyncResult result)
		{
			ExecutionState<T> executionState = result as ExecutionState<T>;
			CommonUtility.AssertNotNull("result", executionState);
			executionState.End();
			TableCommand<T, INTERMEDIATE_TYPE> tableCommand = executionState.Cmd as TableCommand<T, INTERMEDIATE_TYPE>;
			TableExecutor.ReleaseContext(tableCommand.Context);
			if (executionState.ExceptionRef != null)
			{
				throw executionState.ExceptionRef;
			}
			return executionState.Result;
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0003569C File Offset: 0x0003389C
		public static void InitRequest<T, INTERMEDIATE_TYPE>(ExecutionState<T> executionState)
		{
			try
			{
				executionState.Init();
				ExecutorBase.StartRequestAttempt<T>(executionState);
				if (ExecutorBase.CheckTimeout<T>(executionState, false))
				{
					TableExecutor.EndOperation<T, INTERMEDIATE_TYPE>(executionState);
				}
				else
				{
					bool flag = false;
					try
					{
						object cancellationLockerObject;
						Monitor.Enter(cancellationLockerObject = executionState.CancellationLockerObject, ref flag);
						if (ExecutorBase.CheckCancellation<T>(executionState))
						{
							TableExecutor.EndOperation<T, INTERMEDIATE_TYPE>(executionState);
						}
						else
						{
							TableCommand<T, INTERMEDIATE_TYPE> tableCommandRef = executionState.Cmd as TableCommand<T, INTERMEDIATE_TYPE>;
							Logger.LogInformational(executionState.OperationContext, "Starting asynchronous request to {0}.", new object[]
							{
								tableCommandRef.Context.BaseUri
							});
							tableCommandRef.Begin(delegate(IAsyncResult res)
							{
								executionState.UpdateCompletedSynchronously(res.CompletedSynchronously);
								INTERMEDIATE_TYPE intermediate_TYPE = default(INTERMEDIATE_TYPE);
								try
								{
									intermediate_TYPE = tableCommandRef.End(res);
									executionState.Result = tableCommandRef.ParseResponse(intermediate_TYPE, executionState.Cmd.CurrentResult, tableCommandRef);
									if (executionState.Req != null)
									{
										DataServiceResponse dataServiceResponse = intermediate_TYPE as DataServiceResponse;
										QueryOperationResponse queryOperationResponse = intermediate_TYPE as QueryOperationResponse;
										if (dataServiceResponse != null)
										{
											if (dataServiceResponse.IsBatchResponse)
											{
												if (executionState.Req != null)
												{
													TableExecutor.SetExecutionStateCommandResult<T>(executionState, dataServiceResponse);
													Logger.LogInformational(executionState.OperationContext, "Response received. Status code = {0}, Request ID = {1}, Content-MD5 = {2}, ETag = {3}.", new object[]
													{
														executionState.Cmd.CurrentResult.HttpStatusCode,
														executionState.Cmd.CurrentResult.ServiceRequestID,
														executionState.Cmd.CurrentResult.ContentMd5,
														executionState.Cmd.CurrentResult.Etag
													});
													goto IL_376;
												}
												goto IL_376;
											}
											else
											{
												int num = 0;
												using (IEnumerator<OperationResponse> enumerator = dataServiceResponse.GetEnumerator())
												{
													while (enumerator.MoveNext())
													{
														OperationResponse response = enumerator.Current;
														if (executionState.Req != null)
														{
															TableExecutor.SetStorageCmdRequestResults(executionState.Cmd.RequestResults.ElementAt(num), response);
															Logger.LogInformational(executionState.OperationContext, "Response received. Status code = {0}, Request ID = {1}, Content-MD5 = {2}, ETag = {3}.", new object[]
															{
																executionState.Cmd.RequestResults.ElementAt(num).HttpStatusCode,
																executionState.Cmd.RequestResults.ElementAt(num).ServiceRequestID,
																executionState.Cmd.RequestResults.ElementAt(num).ContentMd5,
																executionState.Cmd.RequestResults.ElementAt(num).Etag
															});
															num++;
														}
													}
													goto IL_376;
												}
											}
										}
										if (queryOperationResponse != null && executionState.Req != null)
										{
											TableExecutor.SetStorageCmdRequestResults(executionState.Cmd.CurrentResult, queryOperationResponse);
											Logger.LogInformational(executionState.OperationContext, "Response received. Status code = {0}, Request ID = {1}, Content-MD5 = {2}, ETag = {3}.", new object[]
											{
												executionState.Cmd.CurrentResult.HttpStatusCode,
												executionState.Cmd.CurrentResult.ServiceRequestID,
												executionState.Cmd.CurrentResult.ContentMd5,
												executionState.Cmd.CurrentResult.Etag
											});
										}
									}
									IL_376:;
								}
								catch (Exception ex2)
								{
									Logger.LogWarning(executionState.OperationContext, "Exception thrown during the operation: {0}.", new object[]
									{
										ex2.Message
									});
									lock (executionState.CancellationLockerObject)
									{
										if (executionState.CancelRequested)
										{
											return;
										}
									}
									if (executionState.ExceptionRef == null || !(executionState.ExceptionRef is StorageException))
									{
										executionState.ExceptionRef = ExecutorBase.TranslateDataServiceExceptionBasedOnParseError(ex2, executionState.Cmd.CurrentResult, executionState.Cmd.ParseDataServiceError);
									}
									try
									{
										executionState.Result = tableCommandRef.ParseResponse(intermediate_TYPE, executionState.Cmd.CurrentResult, tableCommandRef);
										executionState.ExceptionRef = null;
									}
									catch (Exception exceptionRef)
									{
										Logger.LogWarning(executionState.OperationContext, "Exception thrown during the operation: {0}.", new object[]
										{
											ex2.Message
										});
										executionState.ExceptionRef = exceptionRef;
									}
								}
								finally
								{
									TableExecutor.EndOperation<T, INTERMEDIATE_TYPE>(executionState);
								}
							}, null);
							if (tableCommandRef.Context != null)
							{
								executionState.CancelDelegate = new Action(tableCommandRef.Context.InternalCancel);
							}
						}
					}
					finally
					{
						if (flag)
						{
							object cancellationLockerObject;
							Monitor.Exit(cancellationLockerObject);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.LogWarning(executionState.OperationContext, "Exception thrown during the operation: {0}.", new object[]
				{
					ex.Message
				});
				if (executionState.ExceptionRef == null || !(executionState.ExceptionRef is StorageException))
				{
					executionState.ExceptionRef = ExecutorBase.TranslateDataServiceExceptionBasedOnParseError(ex, executionState.Cmd.CurrentResult, executionState.Cmd.ParseDataServiceError);
				}
				executionState.OnComplete();
			}
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x000358F8 File Offset: 0x00033AF8
		private static void EndOperation<T, INTERMEDIATE_TYPE>(ExecutionState<T> executionState)
		{
			ExecutorBase.FinishRequestAttempt<T>(executionState);
			lock (executionState.CancellationLockerObject)
			{
				executionState.CancelDelegate = null;
				ExecutorBase.CheckCancellation<T>(executionState);
				if (executionState.ExceptionRef == null)
				{
					Logger.LogInformational(executionState.OperationContext, "Operation completed successfully.", new object[0]);
					executionState.OnComplete();
					return;
				}
			}
			try
			{
				StorageException ex = ExecutorBase.TranslateDataServiceExceptionBasedOnParseError(executionState.ExceptionRef, executionState.Cmd.CurrentResult, executionState.Cmd.ParseDataServiceError);
				executionState.ExceptionRef = ex;
				Logger.LogInformational(executionState.OperationContext, "Checking if the operation should be retried. Retry count = {0}, HTTP status code = {1}, Retryable exception = {2}, Exception = {3}.", new object[]
				{
					executionState.RetryCount,
					executionState.Cmd.CurrentResult.HttpStatusCode,
					ex.IsRetryable ? "yes" : "no",
					ex.Message
				});
				bool flag2 = false;
				TimeSpan timeSpan = TimeSpan.Zero;
				if (ex.IsRetryable && executionState.RetryPolicy != null)
				{
					flag2 = executionState.RetryPolicy.ShouldRetry(executionState.RetryCount++, executionState.Cmd.CurrentResult.HttpStatusCode, executionState.ExceptionRef, out timeSpan, executionState.OperationContext);
					if (timeSpan < TimeSpan.Zero || timeSpan > Constants.MaximumRetryBackoff)
					{
						timeSpan = Constants.MaximumRetryBackoff;
					}
				}
				if (!flag2 || (executionState.OperationExpiryTime != null && (DateTime.Now + timeSpan).CompareTo(executionState.OperationExpiryTime.Value) > 0))
				{
					Logger.LogError(executionState.OperationContext, flag2 ? "Operation cannot be retried because the maximum execution time has been reached. Failing with {0}." : "Retry policy did not allow for a retry. Failing with {0}.", new object[]
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
							executionState.BackoffTimer = new Timer(new TimerCallback(TableExecutor.RetryRequest<T, INTERMEDIATE_TYPE>), executionState, (int)timeSpan.TotalMilliseconds, -1);
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
						Logger.LogInformational(executionState.OperationContext, "Retrying failed operation.", new object[0]);
						TableExecutor.InitRequest<T, INTERMEDIATE_TYPE>(executionState);
					}
				}
			}
			catch (Exception ex2)
			{
				Logger.LogWarning(executionState.OperationContext, "Exception thrown while retrying operation: {0}.", new object[]
				{
					ex2.Message
				});
				executionState.ExceptionRef = ExecutorBase.TranslateExceptionBasedOnParseError(ex2, executionState.Cmd.CurrentResult, executionState.Resp, executionState.Cmd.ParseError);
				executionState.OnComplete();
			}
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00035D78 File Offset: 0x00033F78
		private static void RetryRequest<T, INTERMEDIATE_TYPE>(object state)
		{
			ExecutionState<T> executionState = (ExecutionState<T>)state;
			Logger.LogInformational(executionState.OperationContext, "Retrying failed operation.", new object[0]);
			TableExecutor.InitRequest<T, INTERMEDIATE_TYPE>(executionState);
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x00035DA8 File Offset: 0x00033FA8
		public static T ExecuteSync<T, INTERMEDIATE_TYPE>(TableCommand<T, INTERMEDIATE_TYPE> cmd, IRetryPolicy policy, OperationContext operationContext)
		{
			ExecutionState<T> executionState = new ExecutionState<T>(cmd, policy, operationContext);
			TableExecutor.AcquireContext<T>(cmd.Context, executionState);
			bool flag = false;
			TimeSpan timeSpan = TimeSpan.Zero;
			try
			{
				for (;;)
				{
					executionState.Init();
					ExecutorBase.StartRequestAttempt<T>(executionState);
					ExecutorBase.CheckTimeout<T>(executionState, true);
					try
					{
						INTERMEDIATE_TYPE intermediate_TYPE = default(INTERMEDIATE_TYPE);
						try
						{
							Logger.LogInformational(executionState.OperationContext, "Starting synchronous request to {0}.", new object[]
							{
								cmd.Context.BaseUri
							});
							intermediate_TYPE = cmd.ExecuteFunc();
							executionState.Result = cmd.ParseResponse(intermediate_TYPE, executionState.Cmd.CurrentResult, cmd);
							DataServiceResponse dataServiceResponse = intermediate_TYPE as DataServiceResponse;
							QueryOperationResponse queryOperationResponse = intermediate_TYPE as QueryOperationResponse;
							if (dataServiceResponse != null)
							{
								if (dataServiceResponse.IsBatchResponse)
								{
									if (executionState.Req != null)
									{
										TableExecutor.SetExecutionStateCommandResult<T>(executionState, dataServiceResponse);
										Logger.LogInformational(executionState.OperationContext, "Response received. Status code = {0}, Request ID = {1}, Content-MD5 = {2}, ETag = {3}.", new object[]
										{
											executionState.Cmd.CurrentResult.HttpStatusCode,
											executionState.Cmd.CurrentResult.ServiceRequestID,
											executionState.Cmd.CurrentResult.ContentMd5,
											executionState.Cmd.CurrentResult.Etag
										});
										goto IL_2BF;
									}
									goto IL_2BF;
								}
								else
								{
									int num = 0;
									using (IEnumerator<OperationResponse> enumerator = dataServiceResponse.GetEnumerator())
									{
										while (enumerator.MoveNext())
										{
											OperationResponse response = enumerator.Current;
											if (executionState.Req != null)
											{
												TableExecutor.SetStorageCmdRequestResults(executionState.Cmd.RequestResults.ElementAt(num), response);
												Logger.LogInformational(executionState.OperationContext, "Response received. Status code = {0}, Request ID = {1}, Content-MD5 = {2}, ETag = {3}.", new object[]
												{
													executionState.Cmd.RequestResults.ElementAt(num).HttpStatusCode,
													executionState.Cmd.RequestResults.ElementAt(num).ServiceRequestID,
													executionState.Cmd.RequestResults.ElementAt(num).ContentMd5,
													executionState.Cmd.RequestResults.ElementAt(num).Etag
												});
												num++;
											}
										}
										goto IL_2BF;
									}
								}
							}
							if (queryOperationResponse != null && executionState.Req != null)
							{
								TableExecutor.SetStorageCmdRequestResults(executionState.Cmd.CurrentResult, queryOperationResponse);
								Logger.LogInformational(executionState.OperationContext, "Response received. Status code = {0}, Request ID = {1}, Content-MD5 = {2}, ETag = {3}.", new object[]
								{
									executionState.Cmd.CurrentResult.HttpStatusCode,
									executionState.Cmd.CurrentResult.ServiceRequestID,
									executionState.Cmd.CurrentResult.ContentMd5,
									executionState.Cmd.CurrentResult.Etag
								});
							}
							IL_2BF:;
						}
						catch (Exception ex)
						{
							Logger.LogWarning(executionState.OperationContext, "Exception thrown during the operation: {0}.", new object[]
							{
								ex.Message
							});
							if (executionState.ExceptionRef == null || !(executionState.ExceptionRef is StorageException))
							{
								executionState.ExceptionRef = ExecutorBase.TranslateDataServiceExceptionBasedOnParseError(ex, executionState.Cmd.CurrentResult, executionState.Cmd.ParseDataServiceError);
							}
							executionState.Result = cmd.ParseResponse(intermediate_TYPE, executionState.Cmd.CurrentResult, cmd);
							executionState.ExceptionRef = null;
						}
						ExecutorBase.FinishRequestAttempt<T>(executionState);
						Logger.LogInformational(executionState.OperationContext, "Operation completed successfully.", new object[0]);
						return executionState.Result;
					}
					catch (Exception ex2)
					{
						ExecutorBase.FinishRequestAttempt<T>(executionState);
						StorageException ex3 = ExecutorBase.TranslateDataServiceExceptionBasedOnParseError(ex2, executionState.Cmd.CurrentResult, executionState.Cmd.ParseDataServiceError);
						executionState.ExceptionRef = ex3;
						Logger.LogInformational(executionState.OperationContext, "Checking if the operation should be retried. Retry count = {0}, HTTP status code = {1}, Retryable exception = {2}, Exception = {3}.", new object[]
						{
							executionState.RetryCount,
							executionState.Cmd.CurrentResult.HttpStatusCode,
							ex3.IsRetryable ? "yes" : "no",
							ex3.Message
						});
						flag = false;
						if (ex3.IsRetryable && executionState.RetryPolicy != null)
						{
							flag = executionState.RetryPolicy.ShouldRetry(executionState.RetryCount++, executionState.Cmd.CurrentResult.HttpStatusCode, executionState.ExceptionRef, out timeSpan, executionState.OperationContext);
							if (timeSpan < TimeSpan.Zero || timeSpan > Constants.MaximumRetryBackoff)
							{
								timeSpan = Constants.MaximumRetryBackoff;
							}
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
					Logger.LogInformational(executionState.OperationContext, "Operation will be retried after {0}ms.", new object[]
					{
						(int)timeSpan.TotalMilliseconds
					});
					if (timeSpan > TimeSpan.Zero)
					{
						Thread.Sleep(timeSpan);
					}
					Logger.LogInformational(executionState.OperationContext, "Retrying failed operation.", new object[0]);
					if (!flag)
					{
						goto Block_8;
					}
				}
				Logger.LogError(executionState.OperationContext, flag ? "Operation cannot be retried because the maximum execution time has been reached. Failing with {0}." : "Retry policy did not allow for a retry. Failing with {0}.", new object[]
				{
					executionState.ExceptionRef.Message
				});
				throw executionState.ExceptionRef;
				Block_8:
				throw new NotImplementedException("Unexpected internal storage client error.");
			}
			finally
			{
				TableExecutor.ReleaseContext(cmd.Context);
			}
			T result;
			return result;
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x000363AC File Offset: 0x000345AC
		private static void Context_SendingSignedRequest<T>(ExecutionState<T> executionState, HttpWebRequest req)
		{
			if (executionState.Req != null)
			{
				ExecutorBase.FinishRequestAttempt<T>(executionState);
				ExecutorBase.StartRequestAttempt<T>(executionState);
				ExecutorBase.CheckTimeout<T>(executionState, true);
			}
			executionState.Req = req;
			ExecutorBase.ApplyUserHeaders<T>(executionState);
			ExecutorBase.FireSendingRequest<T>(executionState);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x000363F4 File Offset: 0x000345F4
		internal static void AcquireContext<T>(TableServiceContext ctx, ExecutionState<T> executionState)
		{
			if (!ctx.ContextSemaphore.WaitOne(20000))
			{
				throw new TimeoutException("Could not acquire exclusive use of the TableServiceContext, Concurrent operations are not supported.");
			}
			ctx.SendingSignedRequestAction = delegate(HttpWebRequest request)
			{
				TableExecutor.Context_SendingSignedRequest<T>(executionState, request);
			};
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0003643D File Offset: 0x0003463D
		internal static void ReleaseContext(TableServiceContext ctx)
		{
			ctx.SendingSignedRequestAction = null;
			ctx.ResetCancellation();
			ctx.ContextSemaphore.Release();
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x00036458 File Offset: 0x00034658
		internal static HttpWebResponse GetResponseForRequest<T>(ExecutionState<T> executionState)
		{
			HttpWebResponse result;
			try
			{
				result = (HttpWebResponse)executionState.Req.GetResponse();
			}
			catch (WebException ex)
			{
				Logger.LogWarning(executionState.OperationContext, "Exception thrown while waiting for response: {0}.", new object[]
				{
					ex.Message
				});
				result = (HttpWebResponse)ex.Response;
			}
			return result;
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x000364BC File Offset: 0x000346BC
		private static void SetExecutionStateCommandResult<T>(ExecutionState<T> executionState, DataServiceResponse response)
		{
			if (response.BatchHeaders != null)
			{
				if (response.BatchHeaders.ContainsKey("x-ms-request-id"))
				{
					executionState.Cmd.CurrentResult.ServiceRequestID = response.BatchHeaders["x-ms-request-id"];
				}
				if (response.BatchHeaders.ContainsKey("Content-MD5"))
				{
					executionState.Cmd.CurrentResult.ContentMd5 = response.BatchHeaders["Content-MD5"];
				}
				if (response.BatchHeaders.ContainsKey("Date"))
				{
					string text = response.BatchHeaders["Date"];
					executionState.Cmd.CurrentResult.RequestDate = (string.IsNullOrEmpty(text) ? DateTime.Now.ToString("R", CultureInfo.InvariantCulture) : text);
				}
				if (response.BatchHeaders.ContainsKey(HttpResponseHeader.ETag.ToString()))
				{
					executionState.Cmd.CurrentResult.Etag = response.BatchHeaders[HttpResponseHeader.ETag.ToString()];
				}
			}
			executionState.Cmd.CurrentResult.HttpStatusMessage = (executionState.Cmd.CurrentResult.HttpStatusMessage ?? ((HttpStatusCode)response.BatchStatusCode).ToString());
			if (executionState.Cmd.CurrentResult.HttpStatusCode == -1)
			{
				executionState.Cmd.CurrentResult.HttpStatusCode = response.BatchStatusCode;
			}
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0003662C File Offset: 0x0003482C
		private static void SetStorageCmdRequestResults(RequestResult currentResult, OperationResponse response)
		{
			if (response.Headers != null)
			{
				if (response.Headers.ContainsKey("x-ms-request-id"))
				{
					currentResult.ServiceRequestID = response.Headers["x-ms-request-id"];
				}
				if (response.Headers.ContainsKey("Content-MD5"))
				{
					currentResult.ContentMd5 = response.Headers["Content-MD5"];
				}
				if (response.Headers.ContainsKey("Date"))
				{
					string text = response.Headers["Date"];
					currentResult.RequestDate = (string.IsNullOrEmpty(text) ? DateTime.Now.ToString("R", CultureInfo.InvariantCulture) : text);
				}
				if (response.Headers.ContainsKey(HttpResponseHeader.ETag.ToString()))
				{
					currentResult.Etag = response.Headers[HttpResponseHeader.ETag.ToString()];
				}
			}
			currentResult.HttpStatusMessage = (currentResult.HttpStatusMessage ?? ((HttpStatusCode)response.StatusCode).ToString());
			if (currentResult.HttpStatusCode == -1)
			{
				currentResult.HttpStatusCode = response.StatusCode;
			}
		}
	}
}
