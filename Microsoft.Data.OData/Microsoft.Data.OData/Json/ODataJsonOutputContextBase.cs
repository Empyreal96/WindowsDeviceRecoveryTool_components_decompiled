using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Json
{
	// Token: 0x02000175 RID: 373
	internal abstract class ODataJsonOutputContextBase : ODataOutputContext
	{
		// Token: 0x06000A94 RID: 2708 RVA: 0x000231BD File Offset: 0x000213BD
		protected internal ODataJsonOutputContextBase(ODataFormat format, TextWriter textWriter, ODataMessageWriterSettings messageWriterSettings, IEdmModel model) : base(format, messageWriterSettings, false, true, model, null)
		{
			this.textWriter = textWriter;
			this.jsonWriter = new JsonWriter(this.textWriter, messageWriterSettings.Indent, format);
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x000231EC File Offset: 0x000213EC
		protected internal ODataJsonOutputContextBase(ODataFormat format, Stream messageStream, Encoding encoding, ODataMessageWriterSettings messageWriterSettings, bool writingResponse, bool synchronous, IEdmModel model, IODataUrlResolver urlResolver) : base(format, messageWriterSettings, writingResponse, synchronous, model, urlResolver)
		{
			try
			{
				this.messageOutputStream = messageStream;
				Stream stream;
				if (synchronous)
				{
					stream = messageStream;
				}
				else
				{
					this.asynchronousOutputStream = new AsyncBufferedStream(messageStream);
					stream = this.asynchronousOutputStream;
				}
				this.textWriter = new StreamWriter(stream, encoding);
				this.jsonWriter = new JsonWriter(this.textWriter, messageWriterSettings.Indent, format);
			}
			catch (Exception e)
			{
				if (ExceptionUtils.IsCatchableExceptionType(e) && messageStream != null)
				{
					messageStream.Dispose();
				}
				throw;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000A96 RID: 2710 RVA: 0x00023278 File Offset: 0x00021478
		internal IJsonWriter JsonWriter
		{
			get
			{
				return this.jsonWriter;
			}
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00023280 File Offset: 0x00021480
		internal void VerifyNotDisposed()
		{
			if (this.messageOutputStream == null)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0002329B File Offset: 0x0002149B
		internal void Flush()
		{
			this.jsonWriter.Flush();
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x000232CD File Offset: 0x000214CD
		internal Task FlushAsync()
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.jsonWriter.Flush();
				return this.asynchronousOutputStream.FlushAsync();
			}).FollowOnSuccessWithTask((Task asyncBufferedStreamFlushTask) => this.messageOutputStream.FlushAsync());
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x000232F4 File Offset: 0x000214F4
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			try
			{
				if (this.messageOutputStream != null)
				{
					this.jsonWriter.Flush();
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
				this.textWriter = null;
				this.jsonWriter = null;
			}
		}

		// Token: 0x040003EF RID: 1007
		protected IODataOutputInStreamErrorListener outputInStreamErrorListener;

		// Token: 0x040003F0 RID: 1008
		private Stream messageOutputStream;

		// Token: 0x040003F1 RID: 1009
		private AsyncBufferedStream asynchronousOutputStream;

		// Token: 0x040003F2 RID: 1010
		private TextWriter textWriter;

		// Token: 0x040003F3 RID: 1011
		private IJsonWriter jsonWriter;
	}
}
