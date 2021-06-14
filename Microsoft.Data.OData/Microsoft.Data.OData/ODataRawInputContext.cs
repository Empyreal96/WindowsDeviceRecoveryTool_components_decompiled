using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData
{
	// Token: 0x0200023D RID: 573
	internal sealed class ODataRawInputContext : ODataInputContext
	{
		// Token: 0x06001248 RID: 4680 RVA: 0x0004472C File Offset: 0x0004292C
		internal ODataRawInputContext(ODataFormat format, Stream messageStream, Encoding encoding, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, bool readingResponse, bool synchronous, IEdmModel model, IODataUrlResolver urlResolver, ODataPayloadKind readerPayloadKind) : base(format, messageReaderSettings, version, readingResponse, synchronous, model, urlResolver)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataFormat>(format, "format");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageReaderSettings>(messageReaderSettings, "messageReaderSettings");
			try
			{
				this.stream = messageStream;
				this.encoding = encoding;
				this.readerPayloadKind = readerPayloadKind;
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

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06001249 RID: 4681 RVA: 0x000447A0 File Offset: 0x000429A0
		internal Stream Stream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x000447A8 File Offset: 0x000429A8
		internal override ODataBatchReader CreateBatchReader(string batchBoundary)
		{
			return this.CreateBatchReaderImplementation(batchBoundary, true);
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x000447D0 File Offset: 0x000429D0
		internal override Task<ODataBatchReader> CreateBatchReaderAsync(string batchBoundary)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataBatchReader>(() => this.CreateBatchReaderImplementation(batchBoundary, false));
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x00044802 File Offset: 0x00042A02
		internal override object ReadValue(IEdmPrimitiveTypeReference expectedPrimitiveTypeReference)
		{
			return this.ReadValueImplementation(expectedPrimitiveTypeReference);
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x00044828 File Offset: 0x00042A28
		internal override Task<object> ReadValueAsync(IEdmPrimitiveTypeReference expectedPrimitiveTypeReference)
		{
			return TaskUtils.GetTaskForSynchronousOperation<object>(() => this.ReadValueImplementation(expectedPrimitiveTypeReference));
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x0004485C File Offset: 0x00042A5C
		protected override void DisposeImplementation()
		{
			try
			{
				if (this.textReader != null)
				{
					this.textReader.Dispose();
				}
				else if (this.stream != null)
				{
					this.stream.Dispose();
				}
			}
			finally
			{
				this.textReader = null;
				this.stream = null;
			}
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x000448B4 File Offset: 0x00042AB4
		private ODataBatchReader CreateBatchReaderImplementation(string batchBoundary, bool synchronous)
		{
			return new ODataBatchReader(this, batchBoundary, this.encoding, synchronous);
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x000448C4 File Offset: 0x00042AC4
		private object ReadValueImplementation(IEdmPrimitiveTypeReference expectedPrimitiveTypeReference)
		{
			bool flag;
			if (expectedPrimitiveTypeReference == null)
			{
				flag = (this.readerPayloadKind == ODataPayloadKind.BinaryValue);
			}
			else
			{
				flag = (expectedPrimitiveTypeReference.PrimitiveKind() == EdmPrimitiveTypeKind.Binary);
			}
			if (flag)
			{
				return this.ReadBinaryValue();
			}
			this.textReader = ((this.encoding == null) ? new StreamReader(this.stream) : new StreamReader(this.stream, this.encoding));
			return this.ReadRawValue(expectedPrimitiveTypeReference);
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x0004492C File Offset: 0x00042B2C
		private byte[] ReadBinaryValue()
		{
			long num = 0L;
			List<byte[]> list = new List<byte[]>();
			byte[] array;
			int num2;
			do
			{
				array = new byte[4096];
				num2 = this.stream.Read(array, 0, array.Length);
				num += (long)num2;
				list.Add(array);
			}
			while (num2 == array.Length);
			array = new byte[num];
			for (int i = 0; i < list.Count - 1; i++)
			{
				Buffer.BlockCopy(list[i], 0, array, i * 4096, 4096);
			}
			Buffer.BlockCopy(list[list.Count - 1], 0, array, (list.Count - 1) * 4096, num2);
			return array;
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x000449D0 File Offset: 0x00042BD0
		private object ReadRawValue(IEdmPrimitiveTypeReference expectedPrimitiveTypeReference)
		{
			string text = this.textReader.ReadToEnd();
			object result;
			if (expectedPrimitiveTypeReference != null && !base.MessageReaderSettings.DisablePrimitiveTypeConversion)
			{
				result = AtomValueUtils.ConvertStringToPrimitive(text, expectedPrimitiveTypeReference);
			}
			else
			{
				result = text;
			}
			return result;
		}

		// Token: 0x0400069D RID: 1693
		private const int BufferSize = 4096;

		// Token: 0x0400069E RID: 1694
		private readonly ODataPayloadKind readerPayloadKind;

		// Token: 0x0400069F RID: 1695
		private readonly Encoding encoding;

		// Token: 0x040006A0 RID: 1696
		private Stream stream;

		// Token: 0x040006A1 RID: 1697
		private TextReader textReader;
	}
}
