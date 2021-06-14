using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData
{
	// Token: 0x020001CA RID: 458
	internal sealed class ODataRawOutputContext : ODataOutputContext
	{
		// Token: 0x06000E39 RID: 3641 RVA: 0x00031DB0 File Offset: 0x0002FFB0
		internal ODataRawOutputContext(ODataFormat format, Stream messageStream, Encoding encoding, ODataMessageWriterSettings messageWriterSettings, bool writingResponse, bool synchronous, IEdmModel model, IODataUrlResolver urlResolver) : base(format, messageWriterSettings, writingResponse, synchronous, model, urlResolver)
		{
			try
			{
				this.messageOutputStream = messageStream;
				this.encoding = encoding;
				if (synchronous)
				{
					this.outputStream = messageStream;
				}
				else
				{
					this.asynchronousOutputStream = new AsyncBufferedStream(messageStream);
					this.outputStream = this.asynchronousOutputStream;
				}
			}
			catch
			{
				messageStream.Dispose();
				throw;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x00031E1C File Offset: 0x0003001C
		internal Stream OutputStream
		{
			get
			{
				return this.outputStream;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000E3B RID: 3643 RVA: 0x00031E24 File Offset: 0x00030024
		internal TextWriter TextWriter
		{
			get
			{
				return this.rawValueWriter.TextWriter;
			}
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00031E31 File Offset: 0x00030031
		internal void Flush()
		{
			if (this.rawValueWriter != null)
			{
				this.rawValueWriter.Flush();
			}
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00031E73 File Offset: 0x00030073
		internal Task FlushAsync()
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				if (this.rawValueWriter != null)
				{
					this.rawValueWriter.Flush();
				}
				return this.asynchronousOutputStream.FlushAsync();
			}).FollowOnSuccessWithTask((Task asyncBufferedStreamFlushTask) => this.messageOutputStream.FlushAsync());
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x00031E97 File Offset: 0x00030097
		internal override void WriteInStreamError(ODataError error, bool includeDebugInformation)
		{
			if (this.outputInStreamErrorListener != null)
			{
				this.outputInStreamErrorListener.OnInStreamError();
			}
			throw new ODataException(Strings.ODataMessageWriter_CannotWriteInStreamErrorForRawValues);
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x00031EB6 File Offset: 0x000300B6
		internal override Task WriteInStreamErrorAsync(ODataError error, bool includeDebugInformation)
		{
			if (this.outputInStreamErrorListener != null)
			{
				this.outputInStreamErrorListener.OnInStreamError();
			}
			throw new ODataException(Strings.ODataMessageWriter_CannotWriteInStreamErrorForRawValues);
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x00031ED5 File Offset: 0x000300D5
		internal override ODataBatchWriter CreateODataBatchWriter(string batchBoundary)
		{
			return this.CreateODataBatchWriterImplementation(batchBoundary);
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00031EFC File Offset: 0x000300FC
		internal override Task<ODataBatchWriter> CreateODataBatchWriterAsync(string batchBoundary)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataBatchWriter>(() => this.CreateODataBatchWriterImplementation(batchBoundary));
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x00031F2E File Offset: 0x0003012E
		internal override void WriteValue(object value)
		{
			this.WriteValueImplementation(value);
			this.Flush();
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x00031F64 File Offset: 0x00030164
		internal override Task WriteValueAsync(object value)
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.WriteValueImplementation(value);
				return this.FlushAsync();
			});
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x00031F96 File Offset: 0x00030196
		internal void InitializeRawValueWriter()
		{
			this.rawValueWriter = new RawValueWriter(base.MessageWriterSettings, this.outputStream, this.encoding);
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x00031FB5 File Offset: 0x000301B5
		internal void CloseWriter()
		{
			this.rawValueWriter.Dispose();
			this.rawValueWriter = null;
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x00031FC9 File Offset: 0x000301C9
		internal void VerifyNotDisposed()
		{
			if (this.messageOutputStream == null)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x00031FE4 File Offset: 0x000301E4
		internal void FlushBuffers()
		{
			if (this.asynchronousOutputStream != null)
			{
				this.asynchronousOutputStream.FlushSync();
			}
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x00031FF9 File Offset: 0x000301F9
		internal Task FlushBuffersAsync()
		{
			if (this.asynchronousOutputStream != null)
			{
				return this.asynchronousOutputStream.FlushAsync();
			}
			return TaskUtils.CompletedTask;
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x00032014 File Offset: 0x00030214
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			try
			{
				if (this.messageOutputStream != null)
				{
					if (this.rawValueWriter != null)
					{
						this.rawValueWriter.Flush();
					}
					if (this.asynchronousOutputStream != null)
					{
						this.asynchronousOutputStream.FlushSync();
						this.asynchronousOutputStream.Dispose();
					}
					this.messageOutputStream.Dispose();
				}
			}
			finally
			{
				this.messageOutputStream = null;
				this.asynchronousOutputStream = null;
				this.outputStream = null;
				this.rawValueWriter = null;
			}
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x0003209C File Offset: 0x0003029C
		private void WriteValueImplementation(object value)
		{
			byte[] array = value as byte[];
			if (array != null)
			{
				this.OutputStream.Write(array, 0, array.Length);
				return;
			}
			this.InitializeRawValueWriter();
			this.rawValueWriter.Start();
			this.rawValueWriter.WriteRawValue(value);
			this.rawValueWriter.End();
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x000320EC File Offset: 0x000302EC
		private ODataBatchWriter CreateODataBatchWriterImplementation(string batchBoundary)
		{
			this.encoding = (this.encoding ?? MediaTypeUtils.EncodingUtf8NoPreamble);
			ODataBatchWriter result = new ODataBatchWriter(this, batchBoundary);
			this.outputInStreamErrorListener = result;
			return result;
		}

		// Token: 0x040004B0 RID: 1200
		private Encoding encoding;

		// Token: 0x040004B1 RID: 1201
		private Stream messageOutputStream;

		// Token: 0x040004B2 RID: 1202
		private AsyncBufferedStream asynchronousOutputStream;

		// Token: 0x040004B3 RID: 1203
		private Stream outputStream;

		// Token: 0x040004B4 RID: 1204
		private IODataOutputInStreamErrorListener outputInStreamErrorListener;

		// Token: 0x040004B5 RID: 1205
		private RawValueWriter rawValueWriter;
	}
}
