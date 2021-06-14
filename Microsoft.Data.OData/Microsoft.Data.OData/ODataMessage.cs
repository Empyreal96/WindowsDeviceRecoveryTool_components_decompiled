using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x0200025E RID: 606
	internal abstract class ODataMessage
	{
		// Token: 0x060013FB RID: 5115 RVA: 0x0004AA9E File Offset: 0x00048C9E
		protected ODataMessage(bool writing, bool disableMessageStreamDisposal, long maxMessageSize)
		{
			this.writing = writing;
			this.disableMessageStreamDisposal = disableMessageStreamDisposal;
			this.maxMessageSize = maxMessageSize;
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x060013FC RID: 5116
		public abstract IEnumerable<KeyValuePair<string, string>> Headers { get; }

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x060013FD RID: 5117 RVA: 0x0004AABB File Offset: 0x00048CBB
		protected internal BufferingReadStream BufferingReadStream
		{
			get
			{
				return this.bufferingReadStream;
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x060013FE RID: 5118 RVA: 0x0004AAC3 File Offset: 0x00048CC3
		// (set) Token: 0x060013FF RID: 5119 RVA: 0x0004AACB File Offset: 0x00048CCB
		protected internal bool? UseBufferingReadStream
		{
			get
			{
				return this.useBufferingReadStream;
			}
			set
			{
				this.useBufferingReadStream = value;
			}
		}

		// Token: 0x06001400 RID: 5120
		public abstract string GetHeader(string headerName);

		// Token: 0x06001401 RID: 5121
		public abstract void SetHeader(string headerName, string headerValue);

		// Token: 0x06001402 RID: 5122
		public abstract Stream GetStream();

		// Token: 0x06001403 RID: 5123
		public abstract Task<Stream> GetStreamAsync();

		// Token: 0x06001404 RID: 5124
		internal abstract TInterface QueryInterface<TInterface>() where TInterface : class;

		// Token: 0x06001405 RID: 5125 RVA: 0x0004AAD4 File Offset: 0x00048CD4
		protected internal Stream GetStream(Func<Stream> messageStreamFunc, bool isRequest)
		{
			if (!this.writing)
			{
				BufferingReadStream bufferingReadStream = this.TryGetBufferingReadStream();
				if (bufferingReadStream != null)
				{
					return bufferingReadStream;
				}
			}
			Stream stream = messageStreamFunc();
			ODataMessage.ValidateMessageStream(stream, isRequest);
			bool flag = !this.writing && this.maxMessageSize > 0L;
			if (this.disableMessageStreamDisposal && flag)
			{
				stream = MessageStreamWrapper.CreateNonDisposingStreamWithMaxSize(stream, this.maxMessageSize);
			}
			else if (this.disableMessageStreamDisposal)
			{
				stream = MessageStreamWrapper.CreateNonDisposingStream(stream);
			}
			else if (flag)
			{
				stream = MessageStreamWrapper.CreateStreamWithMaxSize(stream, this.maxMessageSize);
			}
			if (!this.writing && this.useBufferingReadStream == true)
			{
				this.bufferingReadStream = new BufferingReadStream(stream);
				stream = this.bufferingReadStream;
			}
			return stream;
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x0004AC74 File Offset: 0x00048E74
		protected internal Task<Stream> GetStreamAsync(Func<Task<Stream>> streamFuncAsync, bool isRequest)
		{
			if (!this.writing)
			{
				Stream stream = this.TryGetBufferingReadStream();
				if (stream != null)
				{
					return TaskUtils.GetCompletedTask<Stream>(stream);
				}
			}
			Task<Stream> task = streamFuncAsync();
			ODataMessage.ValidateMessageStreamTask(task, isRequest);
			task = task.FollowOnSuccessWith(delegate(Task<Stream> streamTask)
			{
				Stream stream2 = streamTask.Result;
				ODataMessage.ValidateMessageStream(stream2, isRequest);
				bool flag = !this.writing && this.maxMessageSize > 0L;
				if (this.disableMessageStreamDisposal && flag)
				{
					stream2 = MessageStreamWrapper.CreateNonDisposingStreamWithMaxSize(stream2, this.maxMessageSize);
				}
				else if (this.disableMessageStreamDisposal)
				{
					stream2 = MessageStreamWrapper.CreateNonDisposingStream(stream2);
				}
				else if (flag)
				{
					stream2 = MessageStreamWrapper.CreateStreamWithMaxSize(stream2, this.maxMessageSize);
				}
				return stream2;
			});
			if (!this.writing)
			{
				task = task.FollowOnSuccessWithTask((Task<Stream> streamTask) => BufferedReadStream.BufferStreamAsync(streamTask.Result)).FollowOnSuccessWith((Task<BufferedReadStream> streamTask) => streamTask.Result);
				if (this.useBufferingReadStream == true)
				{
					task = task.FollowOnSuccessWith(delegate(Task<Stream> streamTask)
					{
						Stream result = streamTask.Result;
						this.bufferingReadStream = new BufferingReadStream(result);
						return this.bufferingReadStream;
					});
				}
			}
			return task;
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x0004AD5A File Offset: 0x00048F5A
		protected void VerifyCanSetHeader()
		{
			if (!this.writing)
			{
				throw new ODataException(Strings.ODataMessage_MustNotModifyMessage);
			}
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x0004AD70 File Offset: 0x00048F70
		private static void ValidateMessageStream(Stream stream, bool isRequest)
		{
			if (stream == null)
			{
				string message = isRequest ? Strings.ODataRequestMessage_MessageStreamIsNull : Strings.ODataResponseMessage_MessageStreamIsNull;
				throw new ODataException(message);
			}
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0004AD98 File Offset: 0x00048F98
		private static void ValidateMessageStreamTask(Task<Stream> streamTask, bool isRequest)
		{
			if (streamTask == null)
			{
				string message = isRequest ? Strings.ODataRequestMessage_StreamTaskIsNull : Strings.ODataResponseMessage_StreamTaskIsNull;
				throw new ODataException(message);
			}
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x0004ADC0 File Offset: 0x00048FC0
		private BufferingReadStream TryGetBufferingReadStream()
		{
			if (this.bufferingReadStream == null)
			{
				return null;
			}
			BufferingReadStream result = this.bufferingReadStream;
			if (this.bufferingReadStream.IsBuffering)
			{
				this.bufferingReadStream.ResetStream();
			}
			else
			{
				this.bufferingReadStream = null;
			}
			return result;
		}

		// Token: 0x04000718 RID: 1816
		private readonly bool writing;

		// Token: 0x04000719 RID: 1817
		private readonly bool disableMessageStreamDisposal;

		// Token: 0x0400071A RID: 1818
		private readonly long maxMessageSize;

		// Token: 0x0400071B RID: 1819
		private bool? useBufferingReadStream;

		// Token: 0x0400071C RID: 1820
		private BufferingReadStream bufferingReadStream;
	}
}
