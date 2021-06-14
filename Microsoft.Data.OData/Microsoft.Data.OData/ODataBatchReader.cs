using System;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x0200024A RID: 586
	public sealed class ODataBatchReader : IODataBatchOperationListener
	{
		// Token: 0x060012C2 RID: 4802 RVA: 0x00046379 File Offset: 0x00044579
		internal ODataBatchReader(ODataRawInputContext inputContext, string batchBoundary, Encoding batchEncoding, bool synchronous)
		{
			this.inputContext = inputContext;
			this.synchronous = synchronous;
			this.urlResolver = new ODataBatchUrlResolver(inputContext.UrlResolver);
			this.batchStream = new ODataBatchReaderStream(inputContext, batchBoundary, batchEncoding);
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x060012C3 RID: 4803 RVA: 0x000463AF File Offset: 0x000445AF
		// (set) Token: 0x060012C4 RID: 4804 RVA: 0x000463C2 File Offset: 0x000445C2
		public ODataBatchReaderState State
		{
			get
			{
				this.inputContext.VerifyNotDisposed();
				return this.batchReaderState;
			}
			private set
			{
				this.batchReaderState = value;
			}
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x000463CB File Offset: 0x000445CB
		public bool Read()
		{
			this.VerifyCanRead(true);
			return this.InterceptException<bool>(new Func<bool>(this.ReadSynchronously));
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x000463EF File Offset: 0x000445EF
		public Task<bool> ReadAsync()
		{
			this.VerifyCanRead(false);
			return this.ReadAsynchronously().FollowOnFaultWith(delegate(Task<bool> t)
			{
				this.State = ODataBatchReaderState.Exception;
			});
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x0004640F File Offset: 0x0004460F
		public ODataBatchOperationRequestMessage CreateOperationRequestMessage()
		{
			this.VerifyCanCreateOperationRequestMessage(true);
			return this.InterceptException<ODataBatchOperationRequestMessage>(new Func<ODataBatchOperationRequestMessage>(this.CreateOperationRequestMessageImplementation));
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x00046433 File Offset: 0x00044633
		public Task<ODataBatchOperationRequestMessage> CreateOperationRequestMessageAsync()
		{
			this.VerifyCanCreateOperationRequestMessage(false);
			return TaskUtils.GetTaskForSynchronousOperation<ODataBatchOperationRequestMessage>(new Func<ODataBatchOperationRequestMessage>(this.CreateOperationRequestMessageImplementation)).FollowOnFaultWith(delegate(Task<ODataBatchOperationRequestMessage> t)
			{
				this.State = ODataBatchReaderState.Exception;
			});
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x0004645E File Offset: 0x0004465E
		public ODataBatchOperationResponseMessage CreateOperationResponseMessage()
		{
			this.VerifyCanCreateOperationResponseMessage(true);
			return this.InterceptException<ODataBatchOperationResponseMessage>(new Func<ODataBatchOperationResponseMessage>(this.CreateOperationResponseMessageImplementation));
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00046482 File Offset: 0x00044682
		public Task<ODataBatchOperationResponseMessage> CreateOperationResponseMessageAsync()
		{
			this.VerifyCanCreateOperationResponseMessage(false);
			return TaskUtils.GetTaskForSynchronousOperation<ODataBatchOperationResponseMessage>(new Func<ODataBatchOperationResponseMessage>(this.CreateOperationResponseMessageImplementation)).FollowOnFaultWith(delegate(Task<ODataBatchOperationResponseMessage> t)
			{
				this.State = ODataBatchReaderState.Exception;
			});
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x000464AD File Offset: 0x000446AD
		void IODataBatchOperationListener.BatchOperationContentStreamRequested()
		{
			this.operationState = ODataBatchReader.OperationState.StreamRequested;
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x000464B6 File Offset: 0x000446B6
		Task IODataBatchOperationListener.BatchOperationContentStreamRequestedAsync()
		{
			this.operationState = ODataBatchReader.OperationState.StreamRequested;
			return TaskUtils.CompletedTask;
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x000464C4 File Offset: 0x000446C4
		void IODataBatchOperationListener.BatchOperationContentStreamDisposed()
		{
			this.operationState = ODataBatchReader.OperationState.StreamDisposed;
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x000464D0 File Offset: 0x000446D0
		private ODataBatchReaderState GetEndBoundaryState()
		{
			switch (this.batchReaderState)
			{
			case ODataBatchReaderState.Initial:
				return ODataBatchReaderState.Completed;
			case ODataBatchReaderState.Operation:
				if (this.batchStream.ChangeSetBoundary != null)
				{
					return ODataBatchReaderState.ChangesetEnd;
				}
				return ODataBatchReaderState.Completed;
			case ODataBatchReaderState.ChangesetStart:
				return ODataBatchReaderState.ChangesetEnd;
			case ODataBatchReaderState.ChangesetEnd:
				return ODataBatchReaderState.Completed;
			case ODataBatchReaderState.Completed:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataBatchReader_GetEndBoundary_Completed));
			case ODataBatchReaderState.Exception:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataBatchReader_GetEndBoundary_Exception));
			default:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataBatchReader_GetEndBoundary_UnknownValue));
			}
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x00046550 File Offset: 0x00044750
		private bool ReadSynchronously()
		{
			return this.ReadImplementation();
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x00046558 File Offset: 0x00044758
		private Task<bool> ReadAsynchronously()
		{
			return TaskUtils.GetTaskForSynchronousOperation<bool>(new Func<bool>(this.ReadImplementation));
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x0004656C File Offset: 0x0004476C
		private bool ReadImplementation()
		{
			switch (this.State)
			{
			case ODataBatchReaderState.Initial:
				this.batchReaderState = this.SkipToNextPartAndReadHeaders();
				break;
			case ODataBatchReaderState.Operation:
				if (this.operationState == ODataBatchReader.OperationState.None)
				{
					throw new ODataException(Strings.ODataBatchReader_NoMessageWasCreatedForOperation);
				}
				this.operationState = ODataBatchReader.OperationState.None;
				if (this.contentIdToAddOnNextRead != null)
				{
					this.urlResolver.AddContentId(this.contentIdToAddOnNextRead);
					this.contentIdToAddOnNextRead = null;
				}
				this.batchReaderState = this.SkipToNextPartAndReadHeaders();
				break;
			case ODataBatchReaderState.ChangesetStart:
				this.batchReaderState = this.SkipToNextPartAndReadHeaders();
				break;
			case ODataBatchReaderState.ChangesetEnd:
				this.ResetChangeSetSize();
				this.batchStream.ResetChangeSetBoundary();
				this.batchReaderState = this.SkipToNextPartAndReadHeaders();
				break;
			case ODataBatchReaderState.Completed:
			case ODataBatchReaderState.Exception:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataBatchReader_ReadImplementation));
			default:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataBatchReader_ReadImplementation));
			}
			return this.batchReaderState != ODataBatchReaderState.Completed && this.batchReaderState != ODataBatchReaderState.Exception;
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x00046664 File Offset: 0x00044864
		private ODataBatchReaderState SkipToNextPartAndReadHeaders()
		{
			bool flag;
			bool flag2;
			if (this.batchStream.SkipToBoundary(out flag, out flag2))
			{
				ODataBatchReaderState odataBatchReaderState;
				if (flag || flag2)
				{
					odataBatchReaderState = this.GetEndBoundaryState();
					if (odataBatchReaderState == ODataBatchReaderState.ChangesetEnd)
					{
						this.urlResolver.Reset();
					}
				}
				else
				{
					bool flag3 = this.batchStream.ChangeSetBoundary != null;
					bool flag4 = this.batchStream.ProcessPartHeader();
					if (flag3)
					{
						odataBatchReaderState = ODataBatchReaderState.Operation;
						this.IncreaseChangeSetSize();
					}
					else
					{
						odataBatchReaderState = (flag4 ? ODataBatchReaderState.ChangesetStart : ODataBatchReaderState.Operation);
						this.IncreaseBatchSize();
					}
				}
				return odataBatchReaderState;
			}
			if (this.batchStream.ChangeSetBoundary == null)
			{
				return ODataBatchReaderState.Completed;
			}
			return ODataBatchReaderState.ChangesetEnd;
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x000466F4 File Offset: 0x000448F4
		private ODataBatchOperationRequestMessage CreateOperationRequestMessageImplementation()
		{
			this.operationState = ODataBatchReader.OperationState.MessageCreated;
			string requestLine = this.batchStream.ReadFirstNonEmptyLine();
			string method;
			Uri requestUrl;
			this.ParseRequestLine(requestLine, out method, out requestUrl);
			ODataBatchOperationHeaders odataBatchOperationHeaders = this.batchStream.ReadHeaders();
			ODataBatchOperationRequestMessage result = ODataBatchOperationRequestMessage.CreateReadMessage(this.batchStream, method, requestUrl, odataBatchOperationHeaders, this, this.urlResolver);
			string text;
			if (odataBatchOperationHeaders.TryGetValue("Content-ID", out text))
			{
				if (text != null && this.urlResolver.ContainsContentId(text))
				{
					throw new ODataException(Strings.ODataBatchReader_DuplicateContentIDsNotAllowed(text));
				}
				this.contentIdToAddOnNextRead = text;
			}
			return result;
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x0004677C File Offset: 0x0004497C
		private ODataBatchOperationResponseMessage CreateOperationResponseMessageImplementation()
		{
			this.operationState = ODataBatchReader.OperationState.MessageCreated;
			string responseLine = this.batchStream.ReadFirstNonEmptyLine();
			int statusCode = this.ParseResponseLine(responseLine);
			ODataBatchOperationHeaders headers = this.batchStream.ReadHeaders();
			return ODataBatchOperationResponseMessage.CreateReadMessage(this.batchStream, statusCode, headers, this, this.urlResolver.BatchMessageUrlResolver);
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x000467CC File Offset: 0x000449CC
		private void ParseRequestLine(string requestLine, out string httpMethod, out Uri requestUri)
		{
			int num = requestLine.IndexOf(' ');
			if (num <= 0 || requestLine.Length - 3 <= num)
			{
				throw new ODataException(Strings.ODataBatchReaderStream_InvalidRequestLine(requestLine));
			}
			int num2 = requestLine.LastIndexOf(' ');
			if (num2 < 0 || num2 - num - 1 <= 0 || requestLine.Length - 1 <= num2)
			{
				throw new ODataException(Strings.ODataBatchReaderStream_InvalidRequestLine(requestLine));
			}
			httpMethod = requestLine.Substring(0, num);
			string uriString = requestLine.Substring(num + 1, num2 - num - 1);
			string text = requestLine.Substring(num2 + 1);
			if (string.CompareOrdinal("HTTP/1.1", text) != 0)
			{
				throw new ODataException(Strings.ODataBatchReaderStream_InvalidHttpVersionSpecified(text, "HTTP/1.1"));
			}
			HttpUtils.ValidateHttpMethod(httpMethod);
			if (this.batchStream.ChangeSetBoundary == null)
			{
				if (!HttpUtils.IsQueryMethod(httpMethod))
				{
					throw new ODataException(Strings.ODataBatch_InvalidHttpMethodForQueryOperation(httpMethod));
				}
			}
			else if (HttpUtils.IsQueryMethod(httpMethod))
			{
				throw new ODataException(Strings.ODataBatch_InvalidHttpMethodForChangeSetRequest(httpMethod));
			}
			requestUri = new Uri(uriString, UriKind.RelativeOrAbsolute);
			requestUri = ODataBatchUtils.CreateOperationRequestUri(requestUri, this.inputContext.MessageReaderSettings.BaseUri, this.urlResolver);
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x000468D4 File Offset: 0x00044AD4
		private int ParseResponseLine(string responseLine)
		{
			int num = responseLine.IndexOf(' ');
			if (num <= 0 || responseLine.Length - 3 <= num)
			{
				throw new ODataException(Strings.ODataBatchReaderStream_InvalidResponseLine(responseLine));
			}
			int num2 = responseLine.IndexOf(' ', num + 1);
			if (num2 < 0 || num2 - num - 1 <= 0 || responseLine.Length - 1 <= num2)
			{
				throw new ODataException(Strings.ODataBatchReaderStream_InvalidResponseLine(responseLine));
			}
			string text = responseLine.Substring(0, num);
			string text2 = responseLine.Substring(num + 1, num2 - num - 1);
			if (string.CompareOrdinal("HTTP/1.1", text) != 0)
			{
				throw new ODataException(Strings.ODataBatchReaderStream_InvalidHttpVersionSpecified(text, "HTTP/1.1"));
			}
			int result;
			if (!int.TryParse(text2, out result))
			{
				throw new ODataException(Strings.ODataBatchReaderStream_NonIntegerHttpStatusCode(text2));
			}
			return result;
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x00046984 File Offset: 0x00044B84
		private void VerifyCanCreateOperationRequestMessage(bool synchronousCall)
		{
			this.VerifyReaderReady();
			this.VerifyCallAllowed(synchronousCall);
			if (this.inputContext.ReadingResponse)
			{
				this.ThrowODataException(Strings.ODataBatchReader_CannotCreateRequestOperationWhenReadingResponse);
			}
			if (this.State != ODataBatchReaderState.Operation)
			{
				this.ThrowODataException(Strings.ODataBatchReader_InvalidStateForCreateOperationRequestMessage(this.State));
			}
			if (this.operationState != ODataBatchReader.OperationState.None)
			{
				this.ThrowODataException(Strings.ODataBatchReader_OperationRequestMessageAlreadyCreated);
			}
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x000469E8 File Offset: 0x00044BE8
		private void VerifyCanCreateOperationResponseMessage(bool synchronousCall)
		{
			this.VerifyReaderReady();
			this.VerifyCallAllowed(synchronousCall);
			if (!this.inputContext.ReadingResponse)
			{
				this.ThrowODataException(Strings.ODataBatchReader_CannotCreateResponseOperationWhenReadingRequest);
			}
			if (this.State != ODataBatchReaderState.Operation)
			{
				this.ThrowODataException(Strings.ODataBatchReader_InvalidStateForCreateOperationResponseMessage(this.State));
			}
			if (this.operationState != ODataBatchReader.OperationState.None)
			{
				this.ThrowODataException(Strings.ODataBatchReader_OperationResponseMessageAlreadyCreated);
			}
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x00046A4C File Offset: 0x00044C4C
		private void VerifyCanRead(bool synchronousCall)
		{
			this.VerifyReaderReady();
			this.VerifyCallAllowed(synchronousCall);
			if (this.State == ODataBatchReaderState.Exception || this.State == ODataBatchReaderState.Completed)
			{
				throw new ODataException(Strings.ODataBatchReader_ReadOrReadAsyncCalledInInvalidState(this.State));
			}
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x00046A83 File Offset: 0x00044C83
		private void VerifyReaderReady()
		{
			this.inputContext.VerifyNotDisposed();
			if (this.operationState == ODataBatchReader.OperationState.StreamRequested)
			{
				throw new ODataException(Strings.ODataBatchReader_CannotUseReaderWhileOperationStreamActive);
			}
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x00046AA4 File Offset: 0x00044CA4
		private void VerifyCallAllowed(bool synchronousCall)
		{
			if (synchronousCall)
			{
				if (!this.synchronous)
				{
					throw new ODataException(Strings.ODataBatchReader_SyncCallOnAsyncReader);
				}
			}
			else if (this.synchronous)
			{
				throw new ODataException(Strings.ODataBatchReader_AsyncCallOnSyncReader);
			}
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x00046AD0 File Offset: 0x00044CD0
		private void IncreaseBatchSize()
		{
			this.currentBatchSize += 1U;
			if ((ulong)this.currentBatchSize > (ulong)((long)this.inputContext.MessageReaderSettings.MessageQuotas.MaxPartsPerBatch))
			{
				throw new ODataException(Strings.ODataBatchReader_MaxBatchSizeExceeded(this.inputContext.MessageReaderSettings.MessageQuotas.MaxPartsPerBatch));
			}
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x00046B30 File Offset: 0x00044D30
		private void IncreaseChangeSetSize()
		{
			this.currentChangeSetSize += 1U;
			if ((ulong)this.currentChangeSetSize > (ulong)((long)this.inputContext.MessageReaderSettings.MessageQuotas.MaxOperationsPerChangeset))
			{
				throw new ODataException(Strings.ODataBatchReader_MaxChangeSetSizeExceeded(this.inputContext.MessageReaderSettings.MessageQuotas.MaxOperationsPerChangeset));
			}
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x00046B8F File Offset: 0x00044D8F
		private void ResetChangeSetSize()
		{
			this.currentChangeSetSize = 0U;
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x00046B98 File Offset: 0x00044D98
		private void ThrowODataException(string errorMessage)
		{
			this.State = ODataBatchReaderState.Exception;
			throw new ODataException(errorMessage);
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x00046BA8 File Offset: 0x00044DA8
		private T InterceptException<T>(Func<T> action)
		{
			T result;
			try
			{
				result = action();
			}
			catch (Exception e)
			{
				if (ExceptionUtils.IsCatchableExceptionType(e))
				{
					this.State = ODataBatchReaderState.Exception;
				}
				throw;
			}
			return result;
		}

		// Token: 0x040006BA RID: 1722
		private readonly ODataRawInputContext inputContext;

		// Token: 0x040006BB RID: 1723
		private readonly ODataBatchReaderStream batchStream;

		// Token: 0x040006BC RID: 1724
		private readonly bool synchronous;

		// Token: 0x040006BD RID: 1725
		private readonly ODataBatchUrlResolver urlResolver;

		// Token: 0x040006BE RID: 1726
		private ODataBatchReaderState batchReaderState;

		// Token: 0x040006BF RID: 1727
		private uint currentBatchSize;

		// Token: 0x040006C0 RID: 1728
		private uint currentChangeSetSize;

		// Token: 0x040006C1 RID: 1729
		private ODataBatchReader.OperationState operationState;

		// Token: 0x040006C2 RID: 1730
		private string contentIdToAddOnNextRead;

		// Token: 0x0200024B RID: 587
		private enum OperationState
		{
			// Token: 0x040006C4 RID: 1732
			None,
			// Token: 0x040006C5 RID: 1733
			MessageCreated,
			// Token: 0x040006C6 RID: 1734
			StreamRequested,
			// Token: 0x040006C7 RID: 1735
			StreamDisposed
		}
	}
}
