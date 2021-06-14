using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x0200026E RID: 622
	public sealed class ODataBatchWriter : IODataBatchOperationListener, IODataOutputInStreamErrorListener
	{
		// Token: 0x06001475 RID: 5237 RVA: 0x0004C38F File Offset: 0x0004A58F
		internal ODataBatchWriter(ODataRawOutputContext rawOutputContext, string batchBoundary)
		{
			ExceptionUtils.CheckArgumentNotNull<string>(batchBoundary, "batchBoundary");
			this.rawOutputContext = rawOutputContext;
			this.batchBoundary = batchBoundary;
			this.urlResolver = new ODataBatchUrlResolver(rawOutputContext.UrlResolver);
			this.rawOutputContext.InitializeRawValueWriter();
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001476 RID: 5238 RVA: 0x0004C3CC File Offset: 0x0004A5CC
		// (set) Token: 0x06001477 RID: 5239 RVA: 0x0004C3D4 File Offset: 0x0004A5D4
		private ODataBatchOperationRequestMessage CurrentOperationRequestMessage
		{
			get
			{
				return this.currentOperationRequestMessage;
			}
			set
			{
				this.currentOperationRequestMessage = value;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001478 RID: 5240 RVA: 0x0004C3DD File Offset: 0x0004A5DD
		// (set) Token: 0x06001479 RID: 5241 RVA: 0x0004C3E5 File Offset: 0x0004A5E5
		private ODataBatchOperationResponseMessage CurrentOperationResponseMessage
		{
			get
			{
				return this.currentOperationResponseMessage;
			}
			set
			{
				this.currentOperationResponseMessage = value;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x0600147A RID: 5242 RVA: 0x0004C3EE File Offset: 0x0004A5EE
		private ODataBatchOperationMessage CurrentOperationMessage
		{
			get
			{
				if (this.currentOperationRequestMessage != null)
				{
					return this.currentOperationRequestMessage.OperationMessage;
				}
				if (this.currentOperationResponseMessage != null)
				{
					return this.currentOperationResponseMessage.OperationMessage;
				}
				return null;
			}
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x0004C419 File Offset: 0x0004A619
		public void WriteStartBatch()
		{
			this.VerifyCanWriteStartBatch(true);
			this.WriteStartBatchImplementation();
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x0004C428 File Offset: 0x0004A628
		public Task WriteStartBatchAsync()
		{
			this.VerifyCanWriteStartBatch(false);
			return TaskUtils.GetTaskForSynchronousOperation(new Action(this.WriteStartBatchImplementation));
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x0004C442 File Offset: 0x0004A642
		public void WriteEndBatch()
		{
			this.VerifyCanWriteEndBatch(true);
			this.WriteEndBatchImplementation();
			this.Flush();
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x0004C45F File Offset: 0x0004A65F
		public Task WriteEndBatchAsync()
		{
			this.VerifyCanWriteEndBatch(false);
			return TaskUtils.GetTaskForSynchronousOperation(new Action(this.WriteEndBatchImplementation)).FollowOnSuccessWithTask((Task task) => this.FlushAsync());
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x0004C48A File Offset: 0x0004A68A
		public void WriteStartChangeset()
		{
			this.VerifyCanWriteStartChangeset(true);
			this.WriteStartChangesetImplementation();
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x0004C499 File Offset: 0x0004A699
		public Task WriteStartChangesetAsync()
		{
			this.VerifyCanWriteStartChangeset(false);
			return TaskUtils.GetTaskForSynchronousOperation(new Action(this.WriteStartChangesetImplementation));
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x0004C4B3 File Offset: 0x0004A6B3
		public void WriteEndChangeset()
		{
			this.VerifyCanWriteEndChangeset(true);
			this.WriteEndChangesetImplementation();
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x0004C4C2 File Offset: 0x0004A6C2
		public Task WriteEndChangesetAsync()
		{
			this.VerifyCanWriteEndChangeset(false);
			return TaskUtils.GetTaskForSynchronousOperation(new Action(this.WriteEndChangesetImplementation));
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x0004C4DC File Offset: 0x0004A6DC
		public ODataBatchOperationRequestMessage CreateOperationRequestMessage(string method, Uri uri)
		{
			this.VerifyCanCreateOperationRequestMessage(true, method, uri);
			return this.CreateOperationRequestMessageImplementation(method, uri);
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0004C510 File Offset: 0x0004A710
		public Task<ODataBatchOperationRequestMessage> CreateOperationRequestMessageAsync(string method, Uri uri)
		{
			this.VerifyCanCreateOperationRequestMessage(false, method, uri);
			return TaskUtils.GetTaskForSynchronousOperation<ODataBatchOperationRequestMessage>(() => this.CreateOperationRequestMessageImplementation(method, uri));
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x0004C55C File Offset: 0x0004A75C
		public ODataBatchOperationResponseMessage CreateOperationResponseMessage()
		{
			this.VerifyCanCreateOperationResponseMessage(true);
			return this.CreateOperationResponseMessageImplementation();
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x0004C56B File Offset: 0x0004A76B
		public Task<ODataBatchOperationResponseMessage> CreateOperationResponseMessageAsync()
		{
			this.VerifyCanCreateOperationResponseMessage(false);
			return TaskUtils.GetTaskForSynchronousOperation<ODataBatchOperationResponseMessage>(new Func<ODataBatchOperationResponseMessage>(this.CreateOperationResponseMessageImplementation));
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x0004C588 File Offset: 0x0004A788
		public void Flush()
		{
			this.VerifyCanFlush(true);
			try
			{
				this.rawOutputContext.Flush();
			}
			catch
			{
				this.SetState(ODataBatchWriter.BatchWriterState.Error);
				throw;
			}
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x0004C5CD File Offset: 0x0004A7CD
		public Task FlushAsync()
		{
			this.VerifyCanFlush(false);
			return this.rawOutputContext.FlushAsync().FollowOnFaultWith(delegate(Task t)
			{
				this.SetState(ODataBatchWriter.BatchWriterState.Error);
			});
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x0004C5F2 File Offset: 0x0004A7F2
		void IODataBatchOperationListener.BatchOperationContentStreamRequested()
		{
			this.StartBatchOperationContent();
			this.rawOutputContext.FlushBuffers();
			this.DisposeBatchWriterAndSetContentStreamRequestedState();
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x0004C613 File Offset: 0x0004A813
		Task IODataBatchOperationListener.BatchOperationContentStreamRequestedAsync()
		{
			this.StartBatchOperationContent();
			return this.rawOutputContext.FlushBuffersAsync().FollowOnSuccessWith(delegate(Task task)
			{
				this.DisposeBatchWriterAndSetContentStreamRequestedState();
			});
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x0004C637 File Offset: 0x0004A837
		void IODataBatchOperationListener.BatchOperationContentStreamDisposed()
		{
			this.SetState(ODataBatchWriter.BatchWriterState.OperationStreamDisposed);
			this.CurrentOperationRequestMessage = null;
			this.CurrentOperationResponseMessage = null;
			this.rawOutputContext.InitializeRawValueWriter();
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x0004C659 File Offset: 0x0004A859
		void IODataOutputInStreamErrorListener.OnInStreamError()
		{
			this.rawOutputContext.VerifyNotDisposed();
			this.SetState(ODataBatchWriter.BatchWriterState.Error);
			this.rawOutputContext.TextWriter.Flush();
			throw new ODataException(Strings.ODataBatchWriter_CannotWriteInStreamErrorForBatch);
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x0004C687 File Offset: 0x0004A887
		private static bool IsErrorState(ODataBatchWriter.BatchWriterState state)
		{
			return state == ODataBatchWriter.BatchWriterState.Error;
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x0004C68D File Offset: 0x0004A88D
		private void VerifyCanWriteStartBatch(bool synchronousCall)
		{
			this.ValidateWriterReady();
			this.VerifyCallAllowed(synchronousCall);
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x0004C69C File Offset: 0x0004A89C
		private void WriteStartBatchImplementation()
		{
			this.SetState(ODataBatchWriter.BatchWriterState.BatchStarted);
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x0004C6A5 File Offset: 0x0004A8A5
		private void VerifyCanWriteEndBatch(bool synchronousCall)
		{
			this.ValidateWriterReady();
			this.VerifyCallAllowed(synchronousCall);
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x0004C6B4 File Offset: 0x0004A8B4
		private void WriteEndBatchImplementation()
		{
			this.WritePendingMessageData(true);
			this.SetState(ODataBatchWriter.BatchWriterState.BatchCompleted);
			ODataBatchWriterUtils.WriteEndBoundary(this.rawOutputContext.TextWriter, this.batchBoundary, !this.batchStartBoundaryWritten);
			this.rawOutputContext.TextWriter.WriteLine();
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x0004C6F3 File Offset: 0x0004A8F3
		private void VerifyCanWriteStartChangeset(bool synchronousCall)
		{
			this.ValidateWriterReady();
			this.VerifyCallAllowed(synchronousCall);
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x0004C704 File Offset: 0x0004A904
		private void WriteStartChangesetImplementation()
		{
			this.WritePendingMessageData(true);
			this.SetState(ODataBatchWriter.BatchWriterState.ChangeSetStarted);
			this.ResetChangeSetSize();
			this.InterceptException(new Action(this.IncreaseBatchSize));
			ODataBatchWriterUtils.WriteStartBoundary(this.rawOutputContext.TextWriter, this.batchBoundary, !this.batchStartBoundaryWritten);
			this.batchStartBoundaryWritten = true;
			ODataBatchWriterUtils.WriteChangeSetPreamble(this.rawOutputContext.TextWriter, this.changeSetBoundary);
			this.changesetStartBoundaryWritten = false;
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x0004C77A File Offset: 0x0004A97A
		private void VerifyCanWriteEndChangeset(bool synchronousCall)
		{
			this.ValidateWriterReady();
			this.VerifyCallAllowed(synchronousCall);
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x0004C78C File Offset: 0x0004A98C
		private void WriteEndChangesetImplementation()
		{
			this.WritePendingMessageData(true);
			string boundary = this.changeSetBoundary;
			this.SetState(ODataBatchWriter.BatchWriterState.ChangeSetCompleted);
			ODataBatchWriterUtils.WriteEndBoundary(this.rawOutputContext.TextWriter, boundary, !this.changesetStartBoundaryWritten);
			this.urlResolver.Reset();
			this.currentOperationContentId = null;
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x0004C7DC File Offset: 0x0004A9DC
		private void VerifyCanCreateOperationRequestMessage(bool synchronousCall, string method, Uri uri)
		{
			this.ValidateWriterReady();
			this.VerifyCallAllowed(synchronousCall);
			if (this.rawOutputContext.WritingResponse)
			{
				this.ThrowODataException(Strings.ODataBatchWriter_CannotCreateRequestOperationWhenWritingResponse);
			}
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(method, "method");
			if (this.changeSetBoundary == null)
			{
				if (!HttpUtils.IsQueryMethod(method))
				{
					this.ThrowODataException(Strings.ODataBatch_InvalidHttpMethodForQueryOperation(method));
				}
			}
			else if (HttpUtils.IsQueryMethod(method))
			{
				this.ThrowODataException(Strings.ODataBatch_InvalidHttpMethodForChangeSetRequest(method));
			}
			ExceptionUtils.CheckArgumentNotNull<Uri>(uri, "uri");
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x0004C894 File Offset: 0x0004AA94
		private ODataBatchOperationRequestMessage CreateOperationRequestMessageImplementation(string method, Uri uri)
		{
			if (this.changeSetBoundary == null)
			{
				this.InterceptException(new Action(this.IncreaseBatchSize));
			}
			else
			{
				this.InterceptException(new Action(this.IncreaseChangeSetSize));
			}
			this.WritePendingMessageData(true);
			if (this.currentOperationContentId != null)
			{
				this.urlResolver.AddContentId(this.currentOperationContentId);
			}
			this.InterceptException(delegate
			{
				uri = ODataBatchUtils.CreateOperationRequestUri(uri, this.rawOutputContext.MessageWriterSettings.BaseUri, this.urlResolver);
			});
			this.CurrentOperationRequestMessage = ODataBatchOperationRequestMessage.CreateWriteMessage(this.rawOutputContext.OutputStream, method, uri, this, this.urlResolver);
			this.SetState(ODataBatchWriter.BatchWriterState.OperationCreated);
			this.WriteStartBoundaryForOperation();
			ODataBatchWriterUtils.WriteRequestPreamble(this.rawOutputContext.TextWriter, method, uri);
			return this.CurrentOperationRequestMessage;
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x0004C963 File Offset: 0x0004AB63
		private void VerifyCanCreateOperationResponseMessage(bool synchronousCall)
		{
			this.ValidateWriterReady();
			this.VerifyCallAllowed(synchronousCall);
			if (!this.rawOutputContext.WritingResponse)
			{
				this.ThrowODataException(Strings.ODataBatchWriter_CannotCreateResponseOperationWhenWritingRequest);
			}
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x0004C98C File Offset: 0x0004AB8C
		private ODataBatchOperationResponseMessage CreateOperationResponseMessageImplementation()
		{
			this.WritePendingMessageData(true);
			this.CurrentOperationResponseMessage = ODataBatchOperationResponseMessage.CreateWriteMessage(this.rawOutputContext.OutputStream, this, this.urlResolver.BatchMessageUrlResolver);
			this.SetState(ODataBatchWriter.BatchWriterState.OperationCreated);
			this.WriteStartBoundaryForOperation();
			ODataBatchWriterUtils.WriteResponsePreamble(this.rawOutputContext.TextWriter);
			return this.CurrentOperationResponseMessage;
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x0004C9E5 File Offset: 0x0004ABE5
		private void StartBatchOperationContent()
		{
			this.WritePendingMessageData(false);
			this.rawOutputContext.TextWriter.Flush();
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x0004C9FE File Offset: 0x0004ABFE
		private void DisposeBatchWriterAndSetContentStreamRequestedState()
		{
			this.rawOutputContext.CloseWriter();
			this.SetState(ODataBatchWriter.BatchWriterState.OperationStreamRequested);
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x0004CA12 File Offset: 0x0004AC12
		private void RememberContentIdHeader(string contentId)
		{
			this.currentOperationContentId = contentId;
			if (contentId != null && this.urlResolver.ContainsContentId(contentId))
			{
				throw new ODataException(Strings.ODataBatchWriter_DuplicateContentIDsNotAllowed(contentId));
			}
		}

		// Token: 0x0600149D RID: 5277 RVA: 0x0004CA38 File Offset: 0x0004AC38
		private void VerifyCanFlush(bool synchronousCall)
		{
			this.rawOutputContext.VerifyNotDisposed();
			this.VerifyCallAllowed(synchronousCall);
			if (this.state == ODataBatchWriter.BatchWriterState.OperationStreamRequested)
			{
				this.ThrowODataException(Strings.ODataBatchWriter_FlushOrFlushAsyncCalledInStreamRequestedState);
			}
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x0004CA60 File Offset: 0x0004AC60
		private void VerifyCallAllowed(bool synchronousCall)
		{
			if (synchronousCall)
			{
				if (!this.rawOutputContext.Synchronous)
				{
					throw new ODataException(Strings.ODataBatchWriter_SyncCallOnAsyncWriter);
				}
			}
			else if (this.rawOutputContext.Synchronous)
			{
				throw new ODataException(Strings.ODataBatchWriter_AsyncCallOnSyncWriter);
			}
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x0004CA98 File Offset: 0x0004AC98
		private void InterceptException(Action action)
		{
			try
			{
				action();
			}
			catch
			{
				if (!ODataBatchWriter.IsErrorState(this.state))
				{
					this.SetState(ODataBatchWriter.BatchWriterState.Error);
				}
				throw;
			}
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x0004CAF0 File Offset: 0x0004ACF0
		private void SetState(ODataBatchWriter.BatchWriterState newState)
		{
			this.InterceptException(delegate
			{
				this.ValidateTransition(newState);
			});
			ODataBatchWriter.BatchWriterState newState2 = newState;
			switch (newState2)
			{
			case ODataBatchWriter.BatchWriterState.BatchStarted:
				break;
			case ODataBatchWriter.BatchWriterState.ChangeSetStarted:
				this.changeSetBoundary = ODataBatchWriterUtils.CreateChangeSetBoundary(this.rawOutputContext.WritingResponse);
				break;
			default:
				if (newState2 == ODataBatchWriter.BatchWriterState.ChangeSetCompleted)
				{
					this.changeSetBoundary = null;
				}
				break;
			}
			this.state = newState;
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x0004CB6C File Offset: 0x0004AD6C
		private void ValidateTransition(ODataBatchWriter.BatchWriterState newState)
		{
			if (!ODataBatchWriter.IsErrorState(this.state) && ODataBatchWriter.IsErrorState(newState))
			{
				return;
			}
			if (newState == ODataBatchWriter.BatchWriterState.ChangeSetStarted && this.changeSetBoundary != null)
			{
				throw new ODataException(Strings.ODataBatchWriter_CannotStartChangeSetWithActiveChangeSet);
			}
			if (newState == ODataBatchWriter.BatchWriterState.ChangeSetCompleted && this.changeSetBoundary == null)
			{
				throw new ODataException(Strings.ODataBatchWriter_CannotCompleteChangeSetWithoutActiveChangeSet);
			}
			if (newState == ODataBatchWriter.BatchWriterState.BatchCompleted && this.changeSetBoundary != null)
			{
				throw new ODataException(Strings.ODataBatchWriter_CannotCompleteBatchWithActiveChangeSet);
			}
			switch (this.state)
			{
			case ODataBatchWriter.BatchWriterState.Start:
				if (newState != ODataBatchWriter.BatchWriterState.BatchStarted)
				{
					throw new ODataException(Strings.ODataBatchWriter_InvalidTransitionFromStart);
				}
				break;
			case ODataBatchWriter.BatchWriterState.BatchStarted:
				if (newState != ODataBatchWriter.BatchWriterState.ChangeSetStarted && newState != ODataBatchWriter.BatchWriterState.OperationCreated && newState != ODataBatchWriter.BatchWriterState.BatchCompleted)
				{
					throw new ODataException(Strings.ODataBatchWriter_InvalidTransitionFromBatchStarted);
				}
				break;
			case ODataBatchWriter.BatchWriterState.ChangeSetStarted:
				if (newState != ODataBatchWriter.BatchWriterState.OperationCreated && newState != ODataBatchWriter.BatchWriterState.ChangeSetCompleted)
				{
					throw new ODataException(Strings.ODataBatchWriter_InvalidTransitionFromChangeSetStarted);
				}
				break;
			case ODataBatchWriter.BatchWriterState.OperationCreated:
				if (newState != ODataBatchWriter.BatchWriterState.OperationCreated && newState != ODataBatchWriter.BatchWriterState.OperationStreamRequested && newState != ODataBatchWriter.BatchWriterState.ChangeSetStarted && newState != ODataBatchWriter.BatchWriterState.ChangeSetCompleted && newState != ODataBatchWriter.BatchWriterState.BatchCompleted)
				{
					throw new ODataException(Strings.ODataBatchWriter_InvalidTransitionFromOperationCreated);
				}
				break;
			case ODataBatchWriter.BatchWriterState.OperationStreamRequested:
				if (newState != ODataBatchWriter.BatchWriterState.OperationStreamDisposed)
				{
					throw new ODataException(Strings.ODataBatchWriter_InvalidTransitionFromOperationContentStreamRequested);
				}
				break;
			case ODataBatchWriter.BatchWriterState.OperationStreamDisposed:
				if (newState != ODataBatchWriter.BatchWriterState.OperationCreated && newState != ODataBatchWriter.BatchWriterState.ChangeSetStarted && newState != ODataBatchWriter.BatchWriterState.ChangeSetCompleted && newState != ODataBatchWriter.BatchWriterState.BatchCompleted)
				{
					throw new ODataException(Strings.ODataBatchWriter_InvalidTransitionFromOperationContentStreamDisposed);
				}
				break;
			case ODataBatchWriter.BatchWriterState.ChangeSetCompleted:
				if (newState != ODataBatchWriter.BatchWriterState.BatchCompleted && newState != ODataBatchWriter.BatchWriterState.ChangeSetStarted && newState != ODataBatchWriter.BatchWriterState.OperationCreated)
				{
					throw new ODataException(Strings.ODataBatchWriter_InvalidTransitionFromChangeSetCompleted);
				}
				break;
			case ODataBatchWriter.BatchWriterState.BatchCompleted:
				throw new ODataException(Strings.ODataBatchWriter_InvalidTransitionFromBatchCompleted);
			case ODataBatchWriter.BatchWriterState.Error:
				if (newState != ODataBatchWriter.BatchWriterState.Error)
				{
					throw new ODataException(Strings.ODataWriterCore_InvalidTransitionFromError(this.state.ToString(), newState.ToString()));
				}
				break;
			default:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataBatchWriter_ValidateTransition_UnreachableCodePath));
			}
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x0004CD0E File Offset: 0x0004AF0E
		private void ValidateWriterReady()
		{
			this.rawOutputContext.VerifyNotDisposed();
			if (this.state == ODataBatchWriter.BatchWriterState.OperationStreamRequested)
			{
				throw new ODataException(Strings.ODataBatchWriter_InvalidTransitionFromOperationContentStreamRequested);
			}
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x0004CD30 File Offset: 0x0004AF30
		private void WritePendingMessageData(bool reportMessageCompleted)
		{
			if (this.CurrentOperationMessage != null)
			{
				if (this.CurrentOperationResponseMessage != null)
				{
					int statusCode = this.CurrentOperationResponseMessage.StatusCode;
					string statusMessage = HttpUtils.GetStatusMessage(statusCode);
					this.rawOutputContext.TextWriter.WriteLine("{0} {1} {2}", "HTTP/1.1", statusCode, statusMessage);
				}
				bool flag = this.CurrentOperationRequestMessage != null && this.changeSetBoundary != null;
				string contentId = null;
				IEnumerable<KeyValuePair<string, string>> headers = this.CurrentOperationMessage.Headers;
				if (headers != null)
				{
					foreach (KeyValuePair<string, string> keyValuePair in headers)
					{
						string key = keyValuePair.Key;
						string value = keyValuePair.Value;
						this.rawOutputContext.TextWriter.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}: {1}", new object[]
						{
							key,
							value
						}));
						if (flag && string.CompareOrdinal("Content-ID", key) == 0)
						{
							contentId = value;
						}
					}
				}
				if (flag)
				{
					this.RememberContentIdHeader(contentId);
				}
				this.rawOutputContext.TextWriter.WriteLine();
				if (reportMessageCompleted)
				{
					this.CurrentOperationMessage.PartHeaderProcessingCompleted();
					this.CurrentOperationRequestMessage = null;
					this.CurrentOperationResponseMessage = null;
				}
			}
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x0004CE80 File Offset: 0x0004B080
		private void WriteStartBoundaryForOperation()
		{
			if (this.changeSetBoundary == null)
			{
				ODataBatchWriterUtils.WriteStartBoundary(this.rawOutputContext.TextWriter, this.batchBoundary, !this.batchStartBoundaryWritten);
				this.batchStartBoundaryWritten = true;
				return;
			}
			ODataBatchWriterUtils.WriteStartBoundary(this.rawOutputContext.TextWriter, this.changeSetBoundary, !this.changesetStartBoundaryWritten);
			this.changesetStartBoundaryWritten = true;
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x0004CEE2 File Offset: 0x0004B0E2
		private void ThrowODataException(string errorMessage)
		{
			this.SetState(ODataBatchWriter.BatchWriterState.Error);
			throw new ODataException(errorMessage);
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x0004CEF4 File Offset: 0x0004B0F4
		private void IncreaseBatchSize()
		{
			this.currentBatchSize += 1U;
			if ((ulong)this.currentBatchSize > (ulong)((long)this.rawOutputContext.MessageWriterSettings.MessageQuotas.MaxPartsPerBatch))
			{
				throw new ODataException(Strings.ODataBatchWriter_MaxBatchSizeExceeded(this.rawOutputContext.MessageWriterSettings.MessageQuotas.MaxPartsPerBatch));
			}
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x0004CF54 File Offset: 0x0004B154
		private void IncreaseChangeSetSize()
		{
			this.currentChangeSetSize += 1U;
			if ((ulong)this.currentChangeSetSize > (ulong)((long)this.rawOutputContext.MessageWriterSettings.MessageQuotas.MaxOperationsPerChangeset))
			{
				throw new ODataException(Strings.ODataBatchWriter_MaxChangeSetSizeExceeded(this.rawOutputContext.MessageWriterSettings.MessageQuotas.MaxOperationsPerChangeset));
			}
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x0004CFB3 File Offset: 0x0004B1B3
		private void ResetChangeSetSize()
		{
			this.currentChangeSetSize = 0U;
		}

		// Token: 0x04000737 RID: 1847
		private readonly ODataRawOutputContext rawOutputContext;

		// Token: 0x04000738 RID: 1848
		private readonly string batchBoundary;

		// Token: 0x04000739 RID: 1849
		private readonly ODataBatchUrlResolver urlResolver;

		// Token: 0x0400073A RID: 1850
		private ODataBatchWriter.BatchWriterState state;

		// Token: 0x0400073B RID: 1851
		private string changeSetBoundary;

		// Token: 0x0400073C RID: 1852
		private bool batchStartBoundaryWritten;

		// Token: 0x0400073D RID: 1853
		private bool changesetStartBoundaryWritten;

		// Token: 0x0400073E RID: 1854
		private ODataBatchOperationRequestMessage currentOperationRequestMessage;

		// Token: 0x0400073F RID: 1855
		private ODataBatchOperationResponseMessage currentOperationResponseMessage;

		// Token: 0x04000740 RID: 1856
		private string currentOperationContentId;

		// Token: 0x04000741 RID: 1857
		private uint currentBatchSize;

		// Token: 0x04000742 RID: 1858
		private uint currentChangeSetSize;

		// Token: 0x0200026F RID: 623
		private enum BatchWriterState
		{
			// Token: 0x04000744 RID: 1860
			Start,
			// Token: 0x04000745 RID: 1861
			BatchStarted,
			// Token: 0x04000746 RID: 1862
			ChangeSetStarted,
			// Token: 0x04000747 RID: 1863
			OperationCreated,
			// Token: 0x04000748 RID: 1864
			OperationStreamRequested,
			// Token: 0x04000749 RID: 1865
			OperationStreamDisposed,
			// Token: 0x0400074A RID: 1866
			ChangeSetCompleted,
			// Token: 0x0400074B RID: 1867
			BatchCompleted,
			// Token: 0x0400074C RID: 1868
			Error
		}
	}
}
