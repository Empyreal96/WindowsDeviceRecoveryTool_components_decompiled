using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Core.Executor
{
	// Token: 0x02000090 RID: 144
	internal class ExecutionState<T> : StorageCommandAsyncResult
	{
		// Token: 0x06000FAC RID: 4012 RVA: 0x0003BAA8 File Offset: 0x00039CA8
		public ExecutionState(StorageCommandBase<T> cmd, IRetryPolicy policy, OperationContext operationContext)
		{
			this.Cmd = cmd;
			this.RetryPolicy = ((policy != null) ? policy.CreateInstance() : new NoRetry());
			this.OperationContext = (operationContext ?? new OperationContext());
			this.InitializeLocation();
			if (this.OperationContext.StartTime == DateTime.MinValue)
			{
				this.OperationContext.StartTime = DateTime.Now;
			}
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x0003BB20 File Offset: 0x00039D20
		public ExecutionState(StorageCommandBase<T> cmd, IRetryPolicy policy, OperationContext operationContext, AsyncCallback callback, object asyncState) : base(callback, asyncState)
		{
			this.Cmd = cmd;
			this.RetryPolicy = ((policy != null) ? policy.CreateInstance() : new NoRetry());
			this.OperationContext = (operationContext ?? new OperationContext());
			this.InitializeLocation();
			if (this.OperationContext.StartTime == DateTime.MinValue)
			{
				this.OperationContext.StartTime = DateTime.Now;
			}
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x0003BB9C File Offset: 0x00039D9C
		internal void Init()
		{
			this.Req = null;
			this.resp = null;
			this.ReqTimedOut = false;
			base.CancelDelegate = null;
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x0003BBBC File Offset: 0x00039DBC
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Timer backoffTimer = this.BackoffTimer;
				if (backoffTimer != null)
				{
					this.BackoffTimer = null;
					backoffTimer.Dispose();
				}
				this.CheckDisposeSendStream();
				this.CheckDisposeAction();
			}
			base.Dispose(disposing);
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000FB0 RID: 4016 RVA: 0x0003BBF6 File Offset: 0x00039DF6
		// (set) Token: 0x06000FB1 RID: 4017 RVA: 0x0003BBFE File Offset: 0x00039DFE
		internal Timer BackoffTimer { get; set; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x0003BC07 File Offset: 0x00039E07
		// (set) Token: 0x06000FB3 RID: 4019 RVA: 0x0003BC0F File Offset: 0x00039E0F
		internal OperationContext OperationContext { get; private set; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000FB4 RID: 4020 RVA: 0x0003BC18 File Offset: 0x00039E18
		internal DateTime? OperationExpiryTime
		{
			get
			{
				return this.Cmd.OperationExpiryTime;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x0003BC25 File Offset: 0x00039E25
		// (set) Token: 0x06000FB6 RID: 4022 RVA: 0x0003BC2D File Offset: 0x00039E2D
		internal IRetryPolicy RetryPolicy { get; private set; }

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000FB7 RID: 4023 RVA: 0x0003BC36 File Offset: 0x00039E36
		// (set) Token: 0x06000FB8 RID: 4024 RVA: 0x0003BC3E File Offset: 0x00039E3E
		internal StorageCommandBase<T> Cmd { get; private set; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000FB9 RID: 4025 RVA: 0x0003BC47 File Offset: 0x00039E47
		// (set) Token: 0x06000FBA RID: 4026 RVA: 0x0003BC4F File Offset: 0x00039E4F
		internal StorageLocation CurrentLocation { get; set; }

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000FBB RID: 4027 RVA: 0x0003BC58 File Offset: 0x00039E58
		internal RESTCommand<T> RestCMD
		{
			get
			{
				return this.Cmd as RESTCommand<T>;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000FBC RID: 4028 RVA: 0x0003BC65 File Offset: 0x00039E65
		// (set) Token: 0x06000FBD RID: 4029 RVA: 0x0003BC6D File Offset: 0x00039E6D
		internal ExecutorOperation CurrentOperation { get; set; }

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000FBE RID: 4030 RVA: 0x0003BC78 File Offset: 0x00039E78
		internal TimeSpan RemainingTimeout
		{
			get
			{
				if (this.OperationExpiryTime == null || this.OperationExpiryTime.Value.Equals(DateTime.MaxValue))
				{
					return Constants.DefaultClientSideTimeout;
				}
				TimeSpan timeSpan = this.OperationExpiryTime.Value - DateTime.Now;
				if (timeSpan <= TimeSpan.Zero)
				{
					throw Exceptions.GenerateTimeoutException(this.Cmd.CurrentResult, null);
				}
				return timeSpan;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000FBF RID: 4031 RVA: 0x0003BCF2 File Offset: 0x00039EF2
		// (set) Token: 0x06000FC0 RID: 4032 RVA: 0x0003BCFA File Offset: 0x00039EFA
		internal int RetryCount { get; set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x0003BD03 File Offset: 0x00039F03
		// (set) Token: 0x06000FC2 RID: 4034 RVA: 0x0003BD0B File Offset: 0x00039F0B
		internal Stream ReqStream
		{
			get
			{
				return this.reqStream;
			}
			set
			{
				this.reqStream = ((value == null) ? null : value.WrapWithByteCountingStream(this.Cmd.CurrentResult));
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x0003BD2A File Offset: 0x00039F2A
		// (set) Token: 0x06000FC4 RID: 4036 RVA: 0x0003BD34 File Offset: 0x00039F34
		internal Exception ExceptionRef
		{
			get
			{
				return this.exceptionRef;
			}
			set
			{
				this.exceptionRef = value;
				if (this.Cmd != null && this.Cmd.CurrentResult != null)
				{
					this.Cmd.CurrentResult.Exception = value;
				}
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x0003BD65 File Offset: 0x00039F65
		// (set) Token: 0x06000FC6 RID: 4038 RVA: 0x0003BD6D File Offset: 0x00039F6D
		internal T Result { get; set; }

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000FC7 RID: 4039 RVA: 0x0003BD78 File Offset: 0x00039F78
		// (set) Token: 0x06000FC8 RID: 4040 RVA: 0x0003BDBC File Offset: 0x00039FBC
		internal bool ReqTimedOut
		{
			get
			{
				bool result;
				lock (this.timeoutLockerObj)
				{
					result = this.reqTimedOut;
				}
				return result;
			}
			set
			{
				lock (this.timeoutLockerObj)
				{
					this.reqTimedOut = value;
				}
			}
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x0003BE00 File Offset: 0x0003A000
		private void CheckDisposeSendStream()
		{
			RESTCommand<T> restCMD = this.RestCMD;
			if (restCMD != null && restCMD.StreamToDispose != null)
			{
				restCMD.StreamToDispose.Dispose();
				restCMD.StreamToDispose = null;
			}
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x0003BE34 File Offset: 0x0003A034
		private void CheckDisposeAction()
		{
			RESTCommand<T> restCMD = this.RestCMD;
			if (restCMD != null && restCMD.DisposeAction != null)
			{
				Logger.LogInformational(this.OperationContext, "Disposing action invoked.", new object[0]);
				try
				{
					restCMD.DisposeAction(restCMD);
				}
				catch (Exception ex)
				{
					Logger.LogWarning(this.OperationContext, "Disposing action threw an exception : {0}.", new object[]
					{
						ex.Message
					});
				}
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000FCB RID: 4043 RVA: 0x0003BEAC File Offset: 0x0003A0AC
		// (set) Token: 0x06000FCC RID: 4044 RVA: 0x0003BEB4 File Offset: 0x0003A0B4
		internal HttpWebRequest Req { get; set; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000FCD RID: 4045 RVA: 0x0003BEBD File Offset: 0x0003A0BD
		// (set) Token: 0x06000FCE RID: 4046 RVA: 0x0003BEC8 File Offset: 0x0003A0C8
		internal HttpWebResponse Resp
		{
			get
			{
				return this.resp;
			}
			set
			{
				this.resp = value;
				if (this.resp != null)
				{
					if (value.Headers != null)
					{
						this.Cmd.CurrentResult.ServiceRequestID = HttpWebUtility.TryGetHeader(this.resp, "x-ms-request-id", null);
						this.Cmd.CurrentResult.ContentMd5 = HttpWebUtility.TryGetHeader(this.resp, "Content-MD5", null);
						string text = HttpWebUtility.TryGetHeader(this.resp, "Date", null);
						this.Cmd.CurrentResult.RequestDate = (string.IsNullOrEmpty(text) ? DateTime.Now.ToString("R", CultureInfo.InvariantCulture) : text);
						this.Cmd.CurrentResult.Etag = this.resp.Headers[HttpResponseHeader.ETag];
					}
					this.Cmd.CurrentResult.HttpStatusMessage = this.resp.StatusDescription;
					this.Cmd.CurrentResult.HttpStatusCode = (int)this.resp.StatusCode;
				}
			}
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x0003BFD0 File Offset: 0x0003A1D0
		private void InitializeLocation()
		{
			RESTCommand<T> restCMD = this.RestCMD;
			if (restCMD != null)
			{
				switch (restCMD.LocationMode)
				{
				case LocationMode.PrimaryOnly:
				case LocationMode.PrimaryThenSecondary:
					this.CurrentLocation = StorageLocation.Primary;
					break;
				case LocationMode.SecondaryOnly:
				case LocationMode.SecondaryThenPrimary:
					this.CurrentLocation = StorageLocation.Secondary;
					break;
				default:
					CommonUtility.ArgumentOutOfRange("LocationMode", restCMD.LocationMode);
					break;
				}
				Logger.LogInformational(this.OperationContext, "Starting operation with location {0} per location mode {1}.", new object[]
				{
					this.CurrentLocation,
					restCMD.LocationMode
				});
				return;
			}
			this.CurrentLocation = StorageLocation.Primary;
		}

		// Token: 0x0400037F RID: 895
		private Stream reqStream;

		// Token: 0x04000380 RID: 896
		private volatile Exception exceptionRef;

		// Token: 0x04000381 RID: 897
		private object timeoutLockerObj = new object();

		// Token: 0x04000382 RID: 898
		private bool reqTimedOut;

		// Token: 0x04000383 RID: 899
		private HttpWebResponse resp;
	}
}
