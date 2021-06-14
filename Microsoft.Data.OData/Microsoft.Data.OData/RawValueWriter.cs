using System;
using System.IO;
using System.Text;

namespace Microsoft.Data.OData
{
	// Token: 0x0200015C RID: 348
	internal sealed class RawValueWriter : IDisposable
	{
		// Token: 0x0600098D RID: 2445 RVA: 0x0001DB70 File Offset: 0x0001BD70
		internal RawValueWriter(ODataMessageWriterSettings settings, Stream stream, Encoding encoding)
		{
			this.settings = settings;
			this.stream = stream;
			this.encoding = encoding;
			this.InitializeTextWriter();
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x0001DB93 File Offset: 0x0001BD93
		internal TextWriter TextWriter
		{
			get
			{
				return this.textWriter;
			}
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0001DB9B File Offset: 0x0001BD9B
		public void Dispose()
		{
			this.textWriter.Dispose();
			this.textWriter = null;
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0001DBAF File Offset: 0x0001BDAF
		internal void Start()
		{
			if (this.settings.HasJsonPaddingFunction())
			{
				this.textWriter.Write(this.settings.JsonPCallback);
				this.textWriter.Write("(");
			}
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0001DBE4 File Offset: 0x0001BDE4
		internal void End()
		{
			if (this.settings.HasJsonPaddingFunction())
			{
				this.textWriter.Write(")");
			}
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0001DC04 File Offset: 0x0001BE04
		internal void WriteRawValue(object value)
		{
			string value2;
			if (!AtomValueUtils.TryConvertPrimitiveToString(value, out value2))
			{
				throw new ODataException(Strings.ODataUtils_CannotConvertValueToRawPrimitive(value.GetType().FullName));
			}
			this.textWriter.Write(value2);
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0001DC3D File Offset: 0x0001BE3D
		internal void Flush()
		{
			if (this.TextWriter != null)
			{
				this.TextWriter.Flush();
			}
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0001DC54 File Offset: 0x0001BE54
		private void InitializeTextWriter()
		{
			Stream stream;
			if (MessageStreamWrapper.IsNonDisposingStream(this.stream) || this.stream is AsyncBufferedStream)
			{
				stream = this.stream;
			}
			else
			{
				stream = MessageStreamWrapper.CreateNonDisposingStream(this.stream);
			}
			this.textWriter = new StreamWriter(stream, this.encoding);
		}

		// Token: 0x0400037F RID: 895
		private readonly ODataMessageWriterSettings settings;

		// Token: 0x04000380 RID: 896
		private readonly Stream stream;

		// Token: 0x04000381 RID: 897
		private readonly Encoding encoding;

		// Token: 0x04000382 RID: 898
		private TextWriter textWriter;
	}
}
