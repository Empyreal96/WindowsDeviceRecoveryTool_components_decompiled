using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace Microsoft.WindowsAzure.Storage.Core.Executor
{
	// Token: 0x02000067 RID: 103
	internal abstract class ExecutorBase
	{
		// Token: 0x06000DCE RID: 3534 RVA: 0x00032E20 File Offset: 0x00031020
		protected static void ApplyUserHeaders<T>(ExecutionState<T> executionState)
		{
			if (!string.IsNullOrEmpty(executionState.OperationContext.ClientRequestID))
			{
				executionState.Req.Headers.Add("x-ms-client-request-id", executionState.OperationContext.ClientRequestID);
			}
			if (executionState.OperationContext.UserHeaders != null && executionState.OperationContext.UserHeaders.Count > 0)
			{
				foreach (string text in executionState.OperationContext.UserHeaders.Keys)
				{
					executionState.Req.Headers.Add(text, executionState.OperationContext.UserHeaders[text]);
				}
			}
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x00032EE4 File Offset: 0x000310E4
		protected static void StartRequestAttempt<T>(ExecutionState<T> executionState)
		{
			executionState.ExceptionRef = null;
			executionState.Cmd.CurrentResult = new RequestResult
			{
				StartTime = DateTime.Now
			};
			lock (executionState.OperationContext.RequestResults)
			{
				executionState.OperationContext.RequestResults.Add(executionState.Cmd.CurrentResult);
				executionState.Cmd.RequestResults.Add(executionState.Cmd.CurrentResult);
			}
			RESTCommand<T> restCMD = executionState.RestCMD;
			if (restCMD != null)
			{
				if (!restCMD.StorageUri.ValidateLocationMode(restCMD.LocationMode))
				{
					throw new InvalidOperationException("The Uri for the target storage location is not specified. Please consider changing the request's location mode.");
				}
				switch (restCMD.CommandLocationMode)
				{
				case CommandLocationMode.PrimaryOnly:
					if (restCMD.LocationMode == LocationMode.SecondaryOnly)
					{
						throw new InvalidOperationException("This operation can only be executed against the primary storage location.");
					}
					Logger.LogInformational(executionState.OperationContext, "This operation can only be executed against the primary storage location.", new object[0]);
					executionState.CurrentLocation = StorageLocation.Primary;
					restCMD.LocationMode = LocationMode.PrimaryOnly;
					break;
				case CommandLocationMode.SecondaryOnly:
					if (restCMD.LocationMode == LocationMode.PrimaryOnly)
					{
						throw new InvalidOperationException("This operation can only be executed against the secondary storage location.");
					}
					Logger.LogInformational(executionState.OperationContext, "This operation can only be executed against the secondary storage location.", new object[0]);
					executionState.CurrentLocation = StorageLocation.Secondary;
					restCMD.LocationMode = LocationMode.SecondaryOnly;
					break;
				}
			}
			executionState.Cmd.CurrentResult.TargetLocation = executionState.CurrentLocation;
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x00033048 File Offset: 0x00031248
		protected static StorageLocation GetNextLocation(StorageLocation lastLocation, LocationMode locationMode)
		{
			switch (locationMode)
			{
			case LocationMode.PrimaryOnly:
				return StorageLocation.Primary;
			case LocationMode.PrimaryThenSecondary:
			case LocationMode.SecondaryThenPrimary:
				if (lastLocation != StorageLocation.Primary)
				{
					return StorageLocation.Primary;
				}
				return StorageLocation.Secondary;
			case LocationMode.SecondaryOnly:
				return StorageLocation.Secondary;
			default:
				CommonUtility.ArgumentOutOfRange("LocationMode", locationMode);
				return StorageLocation.Primary;
			}
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0003308B File Offset: 0x0003128B
		protected static void FinishRequestAttempt<T>(ExecutionState<T> executionState)
		{
			executionState.Cmd.CurrentResult.EndTime = DateTime.Now;
			executionState.OperationContext.EndTime = DateTime.Now;
			ExecutorBase.FireRequestCompleted<T>(executionState);
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x000330B8 File Offset: 0x000312B8
		protected static void FireSendingRequest<T>(ExecutionState<T> executionState)
		{
			RequestEventArgs args = ExecutorBase.GenerateRequestEventArgs<T>(executionState);
			executionState.OperationContext.FireSendingRequest(args);
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x000330D8 File Offset: 0x000312D8
		protected static void FireResponseReceived<T>(ExecutionState<T> executionState)
		{
			RequestEventArgs args = ExecutorBase.GenerateRequestEventArgs<T>(executionState);
			executionState.OperationContext.FireResponseReceived(args);
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x000330F8 File Offset: 0x000312F8
		protected static void FireRequestCompleted<T>(ExecutionState<T> executionState)
		{
			RequestEventArgs args = ExecutorBase.GenerateRequestEventArgs<T>(executionState);
			executionState.OperationContext.FireRequestCompleted(args);
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x00033118 File Offset: 0x00031318
		protected static void FireRetrying<T>(ExecutionState<T> executionState)
		{
			RequestEventArgs args = ExecutorBase.GenerateRequestEventArgs<T>(executionState);
			executionState.OperationContext.FireRetrying(args);
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00033138 File Offset: 0x00031338
		private static RequestEventArgs GenerateRequestEventArgs<T>(ExecutionState<T> executionState)
		{
			return new RequestEventArgs(executionState.Cmd.CurrentResult)
			{
				Request = executionState.Req,
				Response = executionState.Resp
			};
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00033170 File Offset: 0x00031370
		protected static bool CheckTimeout<T>(ExecutionState<T> executionState, bool throwOnTimeout)
		{
			if (!executionState.ReqTimedOut && (executionState.OperationExpiryTime == null || executionState.Cmd.CurrentResult.StartTime.CompareTo(executionState.OperationExpiryTime.Value) <= 0))
			{
				return false;
			}
			executionState.ReqTimedOut = true;
			StorageException exceptionRef = Exceptions.GenerateTimeoutException(executionState.Cmd.CurrentResult, null);
			executionState.ExceptionRef = exceptionRef;
			if (throwOnTimeout)
			{
				throw executionState.ExceptionRef;
			}
			return true;
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x000331EC File Offset: 0x000313EC
		protected static bool CheckCancellation<T>(ExecutionState<T> executionState)
		{
			bool cancelRequested;
			lock (executionState.CancellationLockerObject)
			{
				if (executionState.CancelRequested)
				{
					executionState.ExceptionRef = Exceptions.GenerateCancellationException(executionState.Cmd.CurrentResult, null);
				}
				cancelRequested = executionState.CancelRequested;
			}
			return cancelRequested;
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x00033270 File Offset: 0x00031470
		internal static StorageException TranslateExceptionBasedOnParseError(Exception ex, RequestResult currentResult, HttpWebResponse response, Func<Stream, HttpWebResponse, string, StorageExtendedErrorInformation> parseError)
		{
			if (parseError != null)
			{
				return StorageException.TranslateException(ex, currentResult, (Stream stream) => parseError(stream, response, null));
			}
			return StorageException.TranslateException(ex, currentResult, null);
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x000332D4 File Offset: 0x000314D4
		internal static StorageException TranslateDataServiceExceptionBasedOnParseError(Exception ex, RequestResult currentResult, Func<Stream, IDictionary<string, string>, string, StorageExtendedErrorInformation> parseDataServiceError)
		{
			if (parseDataServiceError != null)
			{
				return StorageException.TranslateDataServiceException(ex, currentResult, (Stream stream, IDictionary<string, string> headers) => parseDataServiceError(stream, headers, null));
			}
			return StorageException.TranslateDataServiceException(ex, currentResult, null);
		}
	}
}
